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

            UserModel usermodel = new() { UserId = id };
            usermodel.Rooms = dbContext.KenketsuRooms.Include(r => r.Pref).ThenInclude(p => p!.CenterBlock)
                .OrderBy(r => r.Pref!.CenterBlock!.DisplayOrder).ThenBy(r => r.Pref!.DisplayOrder).ThenBy(r => r.DisplayOrder).ToArray();

            return View(usermodel);
        }
    }
}
