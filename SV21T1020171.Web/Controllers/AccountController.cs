using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }
    public IActionResult ChangePassword()
    {
        return View();
    }
}