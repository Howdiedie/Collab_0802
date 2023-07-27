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
            var existingMember = await _TestBananaContext.Members
           .FirstOrDefaultAsync(m => m.MemberAccount == member.MemberAccount);

            if (existingMember != null)
            {
                ModelState.AddModelError(string.Empty, "該帳號已被註冊");
                return View(member);
            }

            member.MemberPhoto = "/img/MemberImg/memberphotoex.png";
            
            _TestBananaContext.Members.Add(member);
            await _TestBananaContext.SaveChangesAsync();

            // 註冊成功，重新導向到會員登入的頁面

            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public async Task<JsonResult> CheckAccount(string account)
        {
            var isUsed = await _TestBananaContext.Members
                .AnyAsync(m => m.MemberAccount == account);

            return Json(new { isUsed = isUsed });
        }
    }
}
