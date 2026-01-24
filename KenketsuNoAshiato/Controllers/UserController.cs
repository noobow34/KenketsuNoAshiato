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
            if (string.IsNullOrEmpty(id) || dbContext.Users.Find(id) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            UserModel usermodel = new() { UserId = id };
            usermodel.Rooms = dbContext.KenketsuRooms.Include(r => r.Pref).ThenInclude(p => p!.CenterBlock)
                .OrderBy(r => r.Pref!.CenterBlock!.DisplayOrder).ThenBy(r => r.Pref!.DisplayOrder).ThenBy(r => r.DisplayOrder).ToArray();

            return View(usermodel);
        }
    }
}
