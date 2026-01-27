using KenketsuNoAshiato.EF;
using KenketsuNoAshiato.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KenketsuNoAshiato.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(string id)
        {
            AshiatoContext dbContext = new();
            User? u = dbContext.Users.Find(id);
            if (string.IsNullOrEmpty(id) || u == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                u.LastAccessAt = DateTime.Now;
                dbContext.SaveChanges();
            }

            UserModel usermodel = new()
            {
                User = u,
                Rooms = Master.KenketsuRooms
            };
            int[] roomsPref = usermodel.Rooms.Select(r => r.PrefId).Distinct().ToArray();
            usermodel.Prefectures = Master.Prefectures;
            int[] roomsCenterBlock = usermodel.Prefectures.Select(p => p.CenterBlockId).Distinct().ToArray();
            usermodel.CenterBlocks = Master.CenterBlocks;

            return View(usermodel);
        }
    }
}
