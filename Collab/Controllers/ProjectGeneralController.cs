using Collab.Filters;
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

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(int id) {
            
            var program = _db.Programs.Find(id);  // 假設您想要查詢的 Program 的 ID 是 1

            if (program == null) {
                // 找不到該 Program，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                ViewBag.ProgramName = "該計劃不存在";
                return View();
            }
            else {
                // Program found, set the ProgramName in the ViewBag
                ViewBag.ProgramName = program.ProgramName;
                ViewBag.ProgramColor = program.ProgramColor;
            }

            ViewBag.ProgramOverview = program.ProgramOverview;

            // 這裡讀取 ProgramLinkList 的資料
            var links = _db.ProgramLinkLists.Where(link => link.ProgramId == program.ProgramId).ToList();
            ViewBag.Links = links;

            // 這裡讀取 ProgramMember 的資料
            var members = _db.ProgramMembers
                .Where(pm => pm.ProgramId == program.ProgramId)
                .Select(pm => new {
                    Name = pm.Member.MemberName,
                    PhotoUrl = pm.Member.MemberPhoto
                })
                .ToList();

            ViewBag.Members = members;

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


        [HttpPost]
        public IActionResult AddMember(int memberId) {
            int programId = 1;  // 這裡假設你想要添加成員的 Program 的 ID 是 1

            // 檢查是否存在該成員
            var member = _db.Members.Find(memberId);
            if (member == null) {
                TempData["Message"] = "該成員不存在。";
                return RedirectToAction("Index");
            }

            // 檢查是否該成員已經是計劃的成員
            var programMember = _db.ProgramMembers.FirstOrDefault(pm => pm.ProgramId == programId && pm.MemberId == memberId);
            if (programMember != null) {
                TempData["Message"] = "該成員已經是計劃的成員。";
                return RedirectToAction("Index");
            }

            // 創建一個新的 ProgramMember 實例並儲存
            var newProgramMember = new ProgramMember { ProgramId = programId, MemberId = memberId, MemberState = "Active" };
            _db.ProgramMembers.Add(newProgramMember);
            _db.SaveChanges();

            TempData["Message"] = "成功添加成員。";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteMember(int memberId) {
            // 檢查成員是否有未完成的任務
            var unfinishedMissions = _db.Missions
                .Where(m => m.MemberId == memberId && (m.MisState == "新任務" || m.MisState == "進行中"))
                .Any();

            if (unfinishedMissions) {
                // 如果成員有未完成的任務，返回錯誤訊息
                TempData["Message"] = "該成員還有未完成的任務。請先完成所有任務再刪除該成員。";
                return RedirectToAction("Index");
            }

            // 從計劃成員中刪除該成員
            var memberInProgram = _db.ProgramMembers.FirstOrDefault(pm => pm.MemberId == memberId && pm.ProgramId == 1);
            if (memberInProgram != null) {
                _db.ProgramMembers.Remove(memberInProgram);
                _db.SaveChanges();
                TempData["Message"] = "成員已成功從計劃中移除。";
            }
            else {
                TempData["Message"] = "找不到該成員在計劃中的記錄。";
            }

            return RedirectToAction("Index");
        }


    }
}
