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

            if(string.IsNullOrEmpty(originalId))
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
            if(sm != null)
            {
                _context.ShareMappings.Remove(sm);
                _context.SaveChanges();
            }
            return Ok();
        }

        private IActionResult IndexInternal(string id, bool isShare,string fromShareId = "")
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
            usermodel.Prefectures = Master.Prefectures;
            int[] roomsCenterBlock = usermodel.Prefectures.Select(p => p.CenterBlockId).Distinct().ToArray();
            usermodel.CenterBlocks = Master.CenterBlocks;
            if (string.IsNullOrEmpty(fromShareId))
            {
                usermodel.ShareId = _context.ShareMappings.Where(sm => sm.OriginalId == id).FirstOrDefault()?.ShareId ?? string.Empty;
            }
            else if(isShare)
            {
                usermodel.ShareId = fromShareId;
            }else
            {
                usermodel.ShareId = string.Empty;
            }

            return View("Index", usermodel);
        }
    }
}
