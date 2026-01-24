using KenketsuNoAshiato.EF;
using Microsoft.AspNetCore.Mvc;

namespace KenketsuNoAshiato.Controllers
{
    public class DataController : Controller
    {
        [HttpGet]
        public ActionResult GetAll(string userId)
        {
            AshiatoContext mc = new();
            var list = mc.VisitStamps.Where(v => v.UserId == userId)
                .Select(x => new {
                    roomId = x.RoomId,
                    date = x.VisitDate,
                    angle = x.Angle
                })
                .ToList();

            return Json(list);
        }

        [HttpPost]
        public ActionResult Save(string userId, int roomId, string date, double angle)
        {
            AshiatoContext mc = new();
            var existing = mc.VisitStamps.FirstOrDefault(x => x.RoomId == roomId && x.UserId == userId);
            var now = DateTime.Now;
            if (existing == null)
            {
                mc.VisitStamps.Add(new VisitStamp
                {
                    RoomId = roomId,
                    UserId = userId,
                    VisitDate = DateOnly.Parse(date),
                    Angle = angle,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
            else
            {
                existing.UpdatedAt = now;
                existing.VisitDate = DateOnly.Parse(date);
            }

            mc.SaveChanges();
            return Json(new { ok = true });
        }

        [HttpPost]
        public ActionResult Delete(string userId, int roomId)
        {
            AshiatoContext mc = new();
            var item = mc.VisitStamps.FirstOrDefault(x => x.RoomId == roomId && x.UserId == userId);
            if (item != null)
            {
                mc.VisitStamps.Remove(item);
                mc.SaveChanges();
            }

            return Json(new { ok = true });
        }

        [HttpPost]
        public ActionResult Reset(string userId)
        {
            AshiatoContext mc = new();
            var list = mc.VisitStamps.Where(v => v.UserId == userId);
            mc.VisitStamps.RemoveRange(list);
            mc.SaveChanges();
            return Json(new { ok = true });
        }
    }
}
