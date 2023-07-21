using Azure;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers
{
    public class ForgetPWController : Controller
    {
        private readonly TestBananaContext _TestBananaContext;
        public ForgetPWController(TestBananaContext context)
        {
            _TestBananaContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult MemberEmail(string memberaccount)
        {
            var user = _TestBananaContext.Members.FirstOrDefault(m => m.MemberAccount == memberaccount);
            if (user != null)
            {
                //SendMailTokenOut outModel = new SendMailTokenOut();




                ViewBag.memberaccount=memberaccount;
                return View("Index"); 
            }
            else {
                ViewBag.notfound = "未搜尋到帳戶，請確認後再輸入";
                return View("Index");
            }
        }
    }

}
