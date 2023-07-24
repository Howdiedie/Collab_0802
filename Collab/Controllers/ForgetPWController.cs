using Azure;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;

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
				var VerificationCode=GenerateRandomVerificationCode();
				TempData["VerificationCode"] = VerificationCode;
				TempData["MemberAccount"] = memberaccount;
                // 使用 Google Mail Server 發信
                string GoogleID = "collabserviceteam@gmail.com"; //Google 發信帳號
				string TempPwd = "hjngahfbaqicnxtl"; //應用程式密碼
				string? ReceiveMail = memberaccount; //接收信箱

				string SmtpServer = "smtp.gmail.com";
				int SmtpPort = 587;
				MailMessage mms = new MailMessage();
				mms.From = new MailAddress(GoogleID);
				mms.Subject = "信件主題";
				string mailContent = $@"
					<!DOCTYPE html>
					<html>
					<head>
						<meta charset=""UTF-8"">
						<title>驗證碼郵件</title>
						<style>
							body {{
							font-family: Arial, sans-serif;
							line-height: 1.6;
							}}
							.container {{
							max-width: 600px;
							margin: 0 auto;
							padding: 20px;
							border: 1px solid #ccc;
							border-radius: 10px;
							}}
							.header {{
							text-align: center;
							}}
							.content {{
							padding-top: 20px;
							}}
							.verification-code {{
							font-size: 24px;
							font-weight: bold;
							color: #007BFF;
							}}
							.note {{
							font-size: 14px;
							color: #666;
							}}
						 </style>
						</head>
							<body>
								<div class=""container"">
									<div class=""header"">
										<h2>Collab-驗證碼郵件</h2>
									</div>
									<div class=""content"">
										<p>HIHI~{user.MemberName}，歡迎您使用Collab！</p>
										<p>您的驗證碼是：<span class=""verification-code"">{VerificationCode}</span></p>
										<p class=""note"">請注意此郵件可能包含敏感信息，請勿將驗證碼洩漏給他人。</p>
									</div>
								</div>
							</body>
						</html>
						";

				mms.Body = mailContent;
				mms.IsBodyHtml = true;
				mms.SubjectEncoding = Encoding.UTF8;
				mms.To.Add(new MailAddress(ReceiveMail));
				using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
				{
					client.EnableSsl = true;
					client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
					client.Send(mms); //寄出信件
				}
				ViewBag.memberaccount = memberaccount;
				return View("Index");
			}

			else
			{
				ViewBag.notfound = "未搜尋到帳戶，請確認後再輸入";
				return View("Index");
			}
		}

		public ActionResult CheckVerificationCode(string CheckVerificationCode )
		{
			string VerificationCode = (string)TempData["VerificationCode"];
			string MemberAccount = (string)TempData["MemberAccount"];
            if (CheckVerificationCode == VerificationCode)
			{
				var user = _TestBananaContext.Members.FirstOrDefault(m => m.MemberAccount == MemberAccount);
                Response.Cookies.Append("UserID", user.MemberId.ToString());
                return RedirectToAction("Index", "MemberArea");
			}
			else
			{
				ViewBag.VerificationCodeError = "驗證碼輸入錯誤";
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
