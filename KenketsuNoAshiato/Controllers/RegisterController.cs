using KenketsuNoAshiato.EF;
using Microsoft.AspNetCore.Mvc;

namespace KenketsuNoAshiato.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index(string? userName)
        {
            string generatedId = ID.GenerateId();
            AshiatoContext dbContext = new ();
            while(dbContext.Users.Find(generatedId) != null)
            {
                generatedId = ID.GenerateId();
            }
            User newUser = new ()
            {
                UserId = generatedId,
                UserName = userName,
                RegisteredAt = DateTime.Now,
                LastAccessAt = DateTime.Now
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            return RedirectToAction("Editable", "User", new { id = generatedId });
        }
    }
}
