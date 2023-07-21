using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers
{
    public class SignController : Controller
    {
        private readonly TestBananaContext _TestBananaContext;
        public SignController(TestBananaContext context)
        {
            _TestBananaContext = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Member member)
        {
            
            _TestBananaContext.Members.Add(member);
            await _TestBananaContext.SaveChangesAsync();

            // 註冊成功，重新導向到會員登入的頁面

            return RedirectToAction("Index", "Login");
        }
    }
}
