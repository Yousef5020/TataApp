using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TataApp.Models;

namespace TataApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("404")]
    public IActionResult PageNotFound()
    {
        string originalPath = "unknown";

        if (HttpContext.Items.TryGetValue("originalPath", out object? value))
        {
            originalPath = value as string ?? "";
        }

        ViewData["originalPath"] = originalPath;

        return View();
    }
}
