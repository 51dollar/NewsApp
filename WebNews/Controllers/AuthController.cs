using Microsoft.AspNetCore.Mvc;
using WebNews.Models.ViewModels.Auth;
using WebNews.Services.Interfaces;

namespace WebNews.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _service;

    public AuthController(IServiceManager service)
    {
        _service = service.UserService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Date in not valid";
            return View(model);
        }

        var existEmail = await _service.IsUserExistsByEmailAsync(model.Email);
        if (existEmail)
        {
            ViewBag.Error = "Email already exists";
            return View(model);
        }

        await _service.RegisterAsync(model);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
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

    public async Task<IActionResult> Logout()
    {
        await _service.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}