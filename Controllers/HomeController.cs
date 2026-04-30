using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginApp.Models;

namespace LoginApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    // Add this new test endpoint
    [HttpGet("config-test")]
public IActionResult ConfigTest()
{
    var testValues = new
    {
        Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Not Set",
        IsDevelopment = !string.IsNullOrEmpty(_configuration["AppConfiguration:ConnectionString"]),
        
        // Check for keys that ACTUALLY exist in your config
        DatabaseConnection = _configuration.GetConnectionString("DefaultConnection") ?? "Not Found",
        EmailHost = _configuration["EmailSettings:Host"] ?? "Not Found",
        KeyVaultUri = _configuration["KeyVault:Uri"] ?? "Not Found",
        
        // Better test - check if ANY App Configuration is loaded
        AppConfigLoaded = !string.IsNullOrEmpty(_configuration["EmailSettings:Host"]) ? "✅ YES" : "❌ NO",
        
        AllConfigKeys = _configuration.AsEnumerable()
            .Where(x => !string.IsNullOrEmpty(x.Key))
            .Take(20)
            .ToDictionary(x => x.Key, x => x.Value)
    };
    
    return Ok(testValues);
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
}
