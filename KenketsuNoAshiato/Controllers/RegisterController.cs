using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace KenketsuNoAshiato.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            string generatedId = GenerateId();
            return View();
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
