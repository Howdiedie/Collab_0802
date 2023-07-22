using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Collab.Filters {
    public class ProfilePicturePathFilter : ActionFilterAttribute {
        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public ProfilePicturePathFilter(TestBananaContext context) {  //                                                                                          ****************************（自己動手加上）
            _db = context;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext) {



            var userId = 1;  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            var db = filterContext.HttpContext.RequestServices.GetService<TestBananaContext>();
            var user = db.Members.Find(userId);

            if (user != null) {
                ((Controller)filterContext.Controller).ViewBag.ProfilePicturePath = user.MemberPhoto;
            }
            else {
                // 如果用戶為 null，可以設置一個預設的頭像路徑
                ((Controller)filterContext.Controller).ViewBag.ProfilePicturePath = "/default/path/to/avatar.jpg";
            }
            ((Controller)filterContext.Controller).ViewBag.MemberName = user.MemberName;  // 設置會員名稱


            var programs = _db.Programs.ToList();
            ((Controller)filterContext.Controller).ViewBag.Programs = programs;

            base.OnActionExecuting(filterContext);
        }



    }

}
