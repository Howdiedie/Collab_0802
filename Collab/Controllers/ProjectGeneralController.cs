using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Collab.Controllers {
    public class ProjectGeneralController : Controller {

        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public ProjectGeneralController(TestBananaContext context) {  //                                                                                          ****************************（自己動手加上）
            _db = context;
        }


        public IActionResult Index() {
            var program = _db.Programs.Find(1);  // 假設您想要查詢的 Program 的 ID 是 1

            if (program == null) {
                // 找不到該 Program，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                return View();
            }

            ViewBag.ProgramOverview = program.ProgramOverview;

            // 這裡讀取 ProgramLinkList 的資料
            var links = _db.ProgramLinkLists.Where(link => link.ProgramId == program.ProgramId).ToList();
            ViewBag.Links = links;

            return View();
        }



        [HttpPost]
        public IActionResult UpdateProgramOverview(string newOverview) {
            int programId = 1;  // 從 Session 或 Cookie 中獲取當前的 Program ID
            var program = _db.Programs.Find(programId);  // 從資料庫中查詢該計劃的資料

            if (program == null) {
                // 找不到該計劃，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                return RedirectToAction("Index");
            }

            // 更新計劃概述
            program.ProgramOverview = newOverview;
            _db.SaveChanges();  // 儲存變更

            // 返回成功訊息
            TempData["Message"] = "計劃概述更新成功。";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult AddLink(string linkTitle, string linkUrl) {
            if (string.IsNullOrEmpty(linkTitle) || string.IsNullOrEmpty(linkUrl)) {
                TempData["Message"] = "請輸入名稱和 URL。";
                return RedirectToAction("Index");
            }

            int programId = 1;  // 從 Session 或 Cookie 中獲取當前的 Program ID
            var program = _db.Programs.Find(programId);

            if (program == null) {
                // 找不到該 Program，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                return RedirectToAction("Index");
            }

            var newLink = new ProgramLinkList { LinkTitle = linkTitle, LinkUrl = linkUrl, ProgramId = programId };
            _db.ProgramLinkLists.Add(newLink);
            _db.SaveChanges();

            TempData["Message"] = "連結添加成功。";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteLink(int linkId) {
            var link = _db.ProgramLinkLists.Find(linkId);
            if (link == null) {
                // 找不到該連結，返回錯誤訊息
                TempData["Message"] = "該連結不存在。";
                return RedirectToAction("Index");
            }

            _db.ProgramLinkLists.Remove(link);
            _db.SaveChanges();

            TempData["Message"] = "連結刪除成功。";
            return RedirectToAction("Index");
        }




    }
}
