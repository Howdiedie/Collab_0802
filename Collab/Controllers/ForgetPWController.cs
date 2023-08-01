using Azure;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Drawing;

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
							h2{{color: #254e6b;
								}}
						</style>
						</head>
						<body>
							<div class=""container"">
								<div class=""header"">
									<h2>Collab 密碼重置驗證碼</h2>
								</div>
								<div class=""content"">
									<h4>親愛的 {user.MemberName}，</h4>
									<p>您好！我們收到您在 Collab 專案管理工具申請密碼重置的請求。<br>為了保障您的帳號安全，請使用以下驗證碼進行身份確認：<br><br>
									驗證碼：<span class=""verification-code"">{VerificationCode}</span><br><br>
									請在 Collab 網站中輸入上述驗證碼完成密碼重置程序。請注意，此驗證碼將在接收後的 30 分鐘內有效，逾時將需要重新申請密碼重置。<br><br>
									若您並未進行密碼重置請求，可能是其他使用者誤用了您的電子郵件地址，請忽略此郵件。<br>
									如有任何疑問或需要協助，請隨時與我們聯繫，我們的客服團隊將竭誠為您提供幫助。
									謝謝使用 Collab！<br><br><br><br>
									此致，<br>
									Collab 專案管理工具團隊
									</p>
									<p class=""note"">請注意此郵件可能包含敏感信息，請勿將驗證碼洩漏給他人。</p>
								</div>
							</div>
						</body>
					</html>";
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
				ViewBag.MemberName = user.MemberName;
                TempData["MemberId"] = user.MemberId;
				ViewBag.DialogToShow=1;
                return View("Index");
			}
			else
			{
				ViewBag.VerificationCodeError = "驗證碼輸入錯誤";
                ViewBag.DialogToShow = 0;
                return View("Index");
			}
		}
		public ActionResult changePasswordForm(string newPassword)
		{
			var UserID = (int)TempData["MemberId"]; // 假設UserID是整數類型
			var user = _TestBananaContext.Members.FirstOrDefault(m => m.MemberId == UserID);

			// 確認找到了用戶記錄
			if (user != null)
			{

				user.MemberPassword = "#############";
				_TestBananaContext.SaveChanges();
				// 將新密碼更新到用戶資料中
				user.MemberPassword = newPassword;

				// 儲存變更到資料庫
				_TestBananaContext.SaveChanges();
				//Response.Cookies.Append("UserID", user.MemberId.ToString());
			}

			return RedirectToAction("Index", "Login");
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
