using Collab.Filters;
using Collab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Security.Claims;

namespace Collab.Controllers {
    public class MemberAreaController : Controller {

        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public MemberAreaController(TestBananaContext context) {  //                                                                                          ****************************（自己動手加上）
            _db = context;  
        }

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index() {
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);
            var user = _db.Members.Find(userId);  // 從資料庫中查詢該會員的資料

            //var member = _db.Members.FirstOrDefault();

            ViewBag.ProfilePicturePath = user.MemberPhoto;


            return View(user);
        }

        //-------------編輯會員名稱
        [HttpPost] 
        public IActionResult Edit(Member updatedMember) {
            if (ModelState.IsValid) {
                string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
                int.TryParse(userIdStr, out int userId);
                var existingMember = _db.Members.Find(userId);
                if (existingMember != null) {
                    existingMember.MemberName = updatedMember.MemberName;
                    _db.SaveChanges(); // 將更新後的資料寫回資料庫
                }
                return RedirectToAction("Index");
            }
            return View(updatedMember);
        }

        //-------------更改密碼
        [HttpPost]
        public IActionResult EditPWD(string oldPassword, string newPassword, string confirmPassword) {
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);
            
            var user = _db.Members.Find(userId);  // 從資料庫中查詢該會員的資料

            if (user == null) {
                // 找不到該會員，返回錯誤訊息
                TempData["Message"] = "會員不存在。";
                return RedirectToAction("Index");
            }
            if (user.MemberPassword != oldPassword) {
                // 輸入的舊密碼不正確，返回錯誤訊息
                TempData["Message"] = "舊密碼不正確。";
                return RedirectToAction("Index");
            }
            if (newPassword != confirmPassword) {
                // 新密碼和確認密碼不一致，返回錯誤訊息
                TempData["Message"] = "新密碼和確認密碼不一致。";
                return RedirectToAction("Index");
            }
            // 更新密碼
            user.MemberPassword = newPassword;
            _db.SaveChanges();  // 儲存變更
                                // 返回成功訊息
            TempData["Message"] = "密碼更改成功。";
            return RedirectToAction("Index");
        }


        //-------------更改頭貼
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture( string cropped_image) {
            /*var userId = 1;*/  
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);

            // 從身份驗證 Cookie 中獲取當前登錄會員的 ID
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // 從 Cookie 中獲取當前登錄會員的 ID
            //var userId = Request.Cookies["YourCookieName"];

            // 檢查是否有文件被上傳
            if (string.IsNullOrEmpty(cropped_image)) {
                TempData["Message"] = "請選擇一個文件上傳。";
                return RedirectToAction("Index");
            }

            // 將 base64 編碼的圖片數據轉換為 byte[]
            var base64Data = cropped_image.Substring(cropped_image.IndexOf(',') + 1);
            var imageBytes = Convert.FromBase64String(base64Data);

            // 將文件保存到某個路徑
            var fileName = userId + ".jpg";  // 裁剪後的圖片將被保存為 .jpg 文件
            var filePath = Path.Combine("wwwroot", "img", "MemberImg", fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            // 更新使用者的頭像路徑
            var user = _db.Members.Find(userId);
            user.MemberPhoto = "/img/MemberImg/" + fileName + "?t=" + DateTime.Now.Ticks;

            _db.SaveChanges();
            TempData["Message"] = "頭像上傳成功。";
            return RedirectToAction("Index");
        }






    }
}
