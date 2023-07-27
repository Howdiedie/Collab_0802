using Azure;
using Azure.Core;
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
            if (filterContext.HttpContext.Request.Cookies.TryGetValue("UserID", out string userIdStr)) {
                if (int.TryParse(userIdStr, out int userId)) {
                    var user = _db.Members.Find(userId);
                    if (user != null) {
                        ((Controller)filterContext.Controller).ViewBag.ProfilePicturePath = user.MemberPhoto;
                        ((Controller)filterContext.Controller).ViewBag.MemberName = user.MemberName;

                        // 從 ProgramMembers 表中獲取與當前用戶相關的所有 Program ID
                        var programIds = _db.ProgramMembers
                            .Where(pm => pm.MemberId == userId && pm.MemberState == "還在")
                            .Select(pm => pm.ProgramId)
                            .ToList();

                        // 從 Programs 表中獲取這些 Programs
                        var programs = _db.Programs
                            .Where(p => programIds.Contains(p.ProgramId))
                            .ToList();

                        ((Controller)filterContext.Controller).ViewBag.Programs = programs;
                    }
                    else {
                        ((Controller)filterContext.Controller).ViewBag.ProfilePicturePath = "/default/path/to/avatar.jpg";
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

       



    }

}
