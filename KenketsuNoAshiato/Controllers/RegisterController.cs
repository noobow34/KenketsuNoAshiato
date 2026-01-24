using KenketsuNoAshiato.EF;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace KenketsuNoAshiato.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index(string? userName)
        {
            string generatedId = GenerateId();
            AshiatoContext dbContext = new ();
            while(dbContext.Users.Find(generatedId) != null)
            {
                generatedId = GenerateId();
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
            return RedirectToAction("Index","User", new { id = generatedId });
        }

        private string GenerateId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}
