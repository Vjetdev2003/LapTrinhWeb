using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Details()
    {
        return View();
    }
    public IActionResult EditDetail(int id = 0, int idProductId = 0)
    {
        return View();
    }
    public IActionResult Shipping()
    {
        return View();
    }
    public IActionResult Create()
    {
        return View();
    }

}