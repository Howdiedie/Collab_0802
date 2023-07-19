using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers
{
    public class SignController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] 
        public IActionResult Register(string userName, string account, string password, string confirmPassword)
        {
            

            // 假設註冊成功，重新導向到會員登入的頁面
            return RedirectToAction("Index", "Login");
        }
    }
}
