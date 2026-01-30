using KenketsuNoAshiato.Dto;
using KenketsuNoAshiato.EF;
using KenketsuNoAshiato.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KenketsuNoAshiato.Controllers
{
    public class UserController : Controller
    {
        AshiatoContext _context;
        public UserController(AshiatoContext context)
        {
            _context = context;
        }

        [Route("u/{id}")]
        public IActionResult Editable(string id)
        {
            return IndexInternal(id, false);
        }

        [Route("s/{id}")]
        public IActionResult Share(string id)
        {
            string? originalId = _context.ShareMappings
                .Where(sm => sm.ShareId == id)
                .Select(sm => sm.OriginalId)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(originalId))
            {
                return RedirectToAction("Index", "Home");
            }

            return IndexInternal(originalId, true, id);
        }

        [HttpPost]
        public string RegisterShareId(string originalId)
        {
            string generatedId = ID.GenerateId();
            AshiatoContext dbContext = new();
            while (dbContext.ShareMappings.Find(generatedId) != null)
            {
                generatedId = ID.GenerateId();
            }

            ShareMapping sm = new()
            {
                ShareId = generatedId,
                OriginalId = originalId
            };
            _context.ShareMappings.Add(sm);
            _context.SaveChanges();
            return generatedId;
        }

        [HttpPost]
        public IActionResult DeleteShareId(string shareId)
        {
            ShareMapping? sm = _context.ShareMappings.Find(shareId);
            if (sm != null)
            {
                _context.ShareMappings.Remove(sm);
                _context.SaveChanges();
            }
            return Ok();
        }

        private IActionResult IndexInternal(string id, bool isShare, string fromShareId = "")
        {
            User? u = _context.Users.Find(id);
            if (string.IsNullOrEmpty(id) || u == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                u.LastAccessAt = DateTime.Now;
                _context.SaveChanges();
            }

            UserModel usermodel = new()
            {
                User = u,
                Rooms = Master.KenketsuRooms,
                IsShare = isShare
            };
            int[] roomsPref = usermodel.Rooms.Select(r => r.PrefId).Distinct().ToArray();
            var pOrders = _context.PrefOrders
                .Where(po => po.UserId == id && roomsPref.Contains(po.PrefId))
                .ToDictionary(po => po.PrefId, po => po.DisplayOrder);
            usermodel.Prefectures = Master.Prefectures.Select(p => new Pref
            {
                PrefId = p.PrefId,
                DisplayOrder = p.DisplayOrder,
                CenterBlockId = p.CenterBlockId,
                PrefName = p.PrefName
            }).ToArray();
            if (pOrders.Count != 0)
            {
                foreach (var pref in usermodel.Prefectures)
                {
                    pref.DisplayOrder = pOrders[pref.PrefId];
                }
            }
            int[] roomsCenterBlock = usermodel.Prefectures.Select(p => p.CenterBlockId).Distinct().ToArray();
            var cOrders = _context.CenterBlockOrders
                .Where(co => co.UserId == id && roomsCenterBlock.Contains(co.CenterBlockId))
                .ToDictionary(co => co.CenterBlockId, co => co.DisplayOrder);
            usermodel.CenterBlocks = Master.CenterBlocks.Select(cb => new CenterBlock
            {
                CenterBlockId = cb.CenterBlockId,
                CenterBlockName = cb.CenterBlockName,
                DisplayOrder = cb.DisplayOrder
            }).ToArray();
            if (cOrders.Count != 0)
            {
                foreach (var cb in usermodel.CenterBlocks)
                {
                    cb.DisplayOrder = cOrders[cb.CenterBlockId];
                }
            }
            if (string.IsNullOrEmpty(fromShareId))
            {
                usermodel.ShareId = _context.ShareMappings.Where(sm => sm.OriginalId == id).FirstOrDefault()?.ShareId ?? string.Empty;
            }
            else if (isShare)
            {
                usermodel.ShareId = fromShareId;
            }
            else
            {
                usermodel.ShareId = string.Empty;
            }

            return View("Index", usermodel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDisplayOrder([FromBody] SaveDisplayOrderRequest request)
        {
            //Delete/Insert方式で更新する
            var cOders = _context.CenterBlockOrders.Where(cdo => cdo.UserId == request.UserId);
            _context.CenterBlockOrders.RemoveRange(cOders);
            var pOrders = _context.PrefOrders.Where(pdo => pdo.UserId == request.UserId);
            _context.PrefOrders.RemoveRange(pOrders);

            foreach (var item in request.Regions!)
            {
                CenterBlockOrder pOrder = new()
                {
                    UserId = request.UserId!,
                    CenterBlockId = item.CenterBlockId,
                    DisplayOrder = item.DisplayOrder
                };
                _context.CenterBlockOrders.Add(pOrder);
                foreach (var pref in item.Prefectures!)
                {
                    PrefOrder cOrder = new()
                    {
                        UserId = request.UserId!,
                        PrefId = pref.PrefId,
                        DisplayOrder = pref.DisplayOrder
                    };
                    _context.PrefOrders.Add(cOrder);
                }
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetDisplayOrder(string userId)
        {
            var cOders = _context.CenterBlockOrders.Where(cdo => cdo.UserId == userId);
            _context.CenterBlockOrders.RemoveRange(cOders);
            var pOrders = _context.PrefOrders.Where(pdo => pdo.UserId == userId);
            _context.PrefOrders.RemoveRange(pOrders);
            _context.SaveChanges();
            return Ok();
        }
    }
}
