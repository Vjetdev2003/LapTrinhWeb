using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.Web;

namespace SV21T1020171.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.UserName = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(username, password);
            
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại!");
                return View();
            }

            //Đăng nhập thành công, tạo dữ liệu để lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(',').ToList()
            };
            //Thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            //Redirec về trang chủ sau khi đăng nhập
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword(string oldPassword = "", string newPassword = "", string reNewPassword = "")
        {
            ViewBag.Title = "Đổi Mật Khẩu";
            if (Request.Method == "POST") {
                //Kiểm tra xem có nhập đầy đủ thông tin không ?
                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(reNewPassword))
                {
                    ModelState.AddModelError("Error", "Vui lòng nhập lại thông tin");
                    return View();
                }
                //Kiểm tra xem mật khẩu có trùng với mật khẩu mới không ?
                if (!newPassword.Equals(reNewPassword)) {
                    ModelState.AddModelError("Error", "Mật khẩu mới không trùng khớp");
                    return View();
                }
                var userData = User.GetUserData();
                if (userData != null) {
                    var result = UserAccountService.ChangePassword(userData.UserName, oldPassword, newPassword);
                    if (!result)
                    {
                        ModelState.AddModelError("Error", "Mật khẩu cũ không đúng");
                        return View();
                    }
                    return RedirectToAction("Logout");
                }
            }
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
