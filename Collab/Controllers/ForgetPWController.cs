using Azure;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

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
                var VerificationCode = GenerateRandomVerificationCode();
                MailMessage mail = new MailMessage();
                //前面是發信email後面是顯示的名稱
                mail.From = new MailAddress("Collab@gmail.com", "Collab信件名稱");

                //收信者email
                mail.To.Add(memberaccount);

                //設定優先權
                mail.Priority = MailPriority.Normal;

                //標題
                mail.Subject = "Collab會員帳戶確認驗證碼";

                //內容
                mail.Body = $"<h1>HIHI,Wellcome</h1><p>您的驗證碼是：{VerificationCode}</p>";
                mail.IsBodyHtml = true;

                //內容使用html
                mail.IsBodyHtml = true;

                //設定gmail的smtp (這是google的)
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

                //您在gmail的帳號密碼
                MySmtp.Credentials = new System.Net.NetworkCredential("Collab1803@gmail.com", "collab5678");

                //開啟ssl
                MySmtp.EnableSsl = true;

                //發送郵件
                MySmtp.Send(mail);

                //放掉宣告出來的MySmtp
                MySmtp = null;

                //放掉宣告出來的mail
                mail.Dispose();
                ViewBag.memberaccount = memberaccount;
                ViewBag.VerificationCode = VerificationCode;
                return View("Index");
            }



            else
            {
                ViewBag.notfound = "未搜尋到帳戶，請確認後再輸入";
                return View("Index");
            }
        }
        private string GenerateRandomVerificationCode()
        {
            // 這裡生成 6 位數的驗證碼
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
    }

}
