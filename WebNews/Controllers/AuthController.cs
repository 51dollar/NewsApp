using Microsoft.AspNetCore.Mvc;
using WebNews.Models.Entities;
using WebNews.Models.ViewModels.Auth;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _service;

    public AuthController(IServiceManager service)
    {
        _service = service.AuthService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]  
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)    
        {
            ViewBag.Error = "Date in not valid";
            return View(model);
        }

        try
        {
            await _service.RegisterAsync(model, RoleType.User);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid.";
            return View(model);
        }

        try
        {
            await _service.LoginAsync(model);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _service.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}