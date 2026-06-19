using System.Diagnostics;
using KenketsuNoAshiato.Models;
using Microsoft.AspNetCore.Mvc;

namespace KenketsuNoAshiato.Controllers;

public class HomeController : Controller
{
    private const string NewSite = "https://kenketsu.noobow.me";

    // トップページ → 転送案内
    [Route("")]
    [Route("Home")]
    [Route("Home/Index")]
    public IActionResult Index()
    {
        ViewBag.Destination = NewSite + "/";
        return View();
    }

    // /u/{userId} → 全国スタンプページへ転送
    [Route("u/{userId}")]
    public IActionResult UserPage(string userId)
    {
        ViewBag.Destination = $"{NewSite}/u/{userId}/stamp";
        return View("Redirect");
    }

    // /u/{userId}/stamp など配下のパスもまとめてカバー
    [Route("u/{userId}/{**rest}")]
    public IActionResult UserSub(string userId, string? rest)
    {
        ViewBag.Destination = $"{NewSite}/u/{userId}/stamp";
        return View("Redirect");
    }

    // /s/{shareId} → 転送案内（閲覧共有ページ）
    [Route("s/{shareId}")]
    public IActionResult Share(string shareId)
    {
        ViewBag.Destination = $"{NewSite}/s/{shareId}";
        return View("ShareRedirect");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
