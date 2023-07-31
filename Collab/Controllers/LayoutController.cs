using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using System.Diagnostics.Metrics;

namespace Collab.Controllers {
    public class LayoutController : Controller {
        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public LayoutController(TestBananaContext context) {  //                                                                                          ****************************（自己動手加上）
            _db = context;
        }


        [HttpPost]
        public IActionResult AddProgram(string programName, string programColor) {
            // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            string userIdStr = Request.Cookies["UserID"];
            int.TryParse(userIdStr, out int userId);

            // 創建一個新的 Project 實例並儲存
            var newProgram = new Collab.Models.Program {
                ProgramName = programName,
                ProgramColor = programColor,
            };

            _db.Programs.Add(newProgram);
            _db.SaveChanges();

            // 從數據庫中獲取新計劃的 ID
            int newProgramId = newProgram.ProgramId;

            // 創建一個新的 ProgramMember 實例並儲存
            var newProgramMember = new ProgramMember { ProgramId = newProgramId, MemberId = userId, MemberState = "還在" };
            _db.ProgramMembers.Add(newProgramMember);
            _db.SaveChanges();

            // 重定向到一個合適的頁面，例如：首頁或計劃列表頁面
            return RedirectToAction("Index", "PersonalOverview");
        }




    }
}
