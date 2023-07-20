using Microsoft.AspNetCore.Mvc;
using Collab.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace collab_00.Controllers {
    public class LoginController : Controller
    {
        private readonly TestBananaContext _TestBananaContext;
        public LoginController(TestBananaContext context)
        {
            _TestBananaContext = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult LoginTest(string memberaccount, string password)
        {
            // 帳號密碼驗證
            var user = _TestBananaContext.Members.FirstOrDefault(m => m.MemberAccount == memberaccount && m.MemberPassword == password);
            // 假設這裡使用一個簡單的驗證邏輯
            if (user!=null)
            {
                // 登入成功
                // 可以根據需要執行其他登入相關的操作，例如設置認證Cookie等

                // 將使用者帳號存入ViewBag以供後續使用
                Response.Cookies.Append("UserID", user.MemberId.ToString());

                // 重定向到登入成功後的頁面
                return RedirectToAction("Index", "PersonalOverview");
            }
            else
            {
                // 登入失敗
                // 可以執行相應的失敗處理邏輯，例如顯示錯誤訊息

                ViewBag.ErrorMessage = "帳號或密碼錯誤";
                ModelState.Clear();
                return View("Index");

            }
        }

        //public IActionResult TestDatabaseConnection()
        //{
        //    
        //    var members = _TestBananaContext.Members.ToList();

        //    
        //    if (members != null)
        //    {
        //       
        //        return Content("Database connection successful");
        //    }
        //    else
        //    {
        //       
        //        return Content("Unable to connect to the database");
        //    }
        //}

    }

}
