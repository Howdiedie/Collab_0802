using Collab.Filters;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace Collab.Controllers {
    public class ProjectGeneralController : LayoutController {

        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public ProjectGeneralController(TestBananaContext context) : base(context) {
            _db = context;
        }


        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(int id) {

            Console.WriteLine(id);
            var program = _db.Programs.Find(id);  // 查詢的 Program 的 ID 
            int programId = id;  
            Response.Cookies.Append("ProgramId", programId.ToString());// 將 ProgramId 儲存到 Cookie

            if (program == null) {
                // 找不到該 Program，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                //ViewBag.ProgramName = "該計劃不存在";
                return View();
            }
            else {
                // Program found, set the ProgramName in the ViewBag
                //ViewBag.ProgramName = program.ProgramName;
                //ViewBag.ProgramColor = program.ProgramColor;
            }

            ViewBag.ProgramOverview = program.ProgramOverview;

            // 這裡讀取 ProgramLinkList 的資料
            var links = _db.ProgramLinkLists.Where(link => link.ProgramId == program.ProgramId).ToList();
            ViewBag.Links = links;

            // 這裡讀取 ProgramMember 的資料
            var members = _db.ProgramMembers
                .Where(pm => pm.ProgramId == program.ProgramId && pm.MemberState == "還在")
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
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);

            int EditMemberId = userId; //誰編輯這個計畫概述的
            var program = _db.Programs.Find(programId);  // 從資料庫中查詢該計劃的資料

            if (program == null) {
                // 找不到該計劃，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                return RedirectToAction("Index");
            }

            // 更新計劃概述
            program.ProgramOverview = newOverview;

            //新增通知
            var NotifyAdd = new Notify {
                NotifyDate = DateTime.Now,
                NotifyAction = "修改",
                NotifyType = "計畫概述",
                ActionName = program.ProgramName.ToString(),
                ProgramId = program.ProgramId,
                MemberId = EditMemberId
            };
            _db.Notifies.Add(NotifyAdd);

            _db.SaveChanges();  // 儲存變更

            // 返回成功訊息
            TempData["Message"] = "計劃概述更新成功。";

            return RedirectToAction("Index", new { id = programId });

        }



        [HttpPost]
        public IActionResult AddLink(string linkTitle, string linkUrl) {
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);


            if (string.IsNullOrEmpty(linkTitle) || string.IsNullOrEmpty(linkUrl)) {
                TempData["Message"] = "請輸入名稱和 URL。";
                return RedirectToAction("Index", new { id = programId });
            }

            var program = _db.Programs.Find(programId);

            if (program == null) {
                // 找不到該 Program，返回錯誤訊息
                TempData["Message"] = "該計劃不存在。";
                return RedirectToAction("Index", new { id = programId });
            }

            var newLink = new ProgramLinkList { LinkTitle = linkTitle, LinkUrl = linkUrl, ProgramId = programId };
            _db.ProgramLinkLists.Add(newLink);


            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);
            int EditMemberId = userId; //誰編輯這個計畫概述的
            //新增通知
            var NotifyAdd = new Notify {
                NotifyDate = DateTime.Now,
                NotifyAction = "新增",
                NotifyType = "連結",
                ActionName = linkTitle,
                ProgramId = program.ProgramId,
                MemberId = EditMemberId
            };
            _db.Notifies.Add(NotifyAdd);
            _db.SaveChanges();

            TempData["Message"] = "連結添加成功。";
            return RedirectToAction("Index", new { id = programId });
        }

        [HttpPost]
        public IActionResult DeleteLink(int linkId) {
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);

            var link = _db.ProgramLinkLists.Find(linkId);
            if (link == null) {
                // 找不到該連結，返回錯誤訊息
                TempData["Message"] = "該連結不存在。";
                return RedirectToAction("Index", new { id = programId });
            }

            _db.ProgramLinkLists.Remove(link);
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);
            int EditMemberId = userId; //誰編輯這個計畫概述的
            //新增通知
            var NotifyAdd = new Notify {
                NotifyDate = DateTime.Now,
                NotifyAction = "刪除",
                NotifyType = "連結",
                ActionName = link.LinkTitle,
                ProgramId = programId,
                MemberId = EditMemberId
            };
            _db.Notifies.Add(NotifyAdd);
            _db.SaveChanges();

            TempData["Message"] = "連結刪除成功。";
            return RedirectToAction("Index", new { id = programId });
        }


        [HttpPost]
        public IActionResult AddMember(string memberAccount) {
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);

            // 檢查是否存在該成員
            var member = _db.Members.FirstOrDefault(m => m.MemberAccount == memberAccount);
            if (member == null) {
                TempData["Message"] = "該成員不存在。";
                return RedirectToAction("Index", new { id = programId });
            }

            // 檢查是否該成員已經是計劃的成員，且狀態為 "還在"
            var existingProgramMember = _db.ProgramMembers.FirstOrDefault(pm => pm.ProgramId == programId && pm.MemberId == member.MemberId && pm.MemberState == "還在");
            if (existingProgramMember != null) {
                TempData["Message"] = "該成員已經是計劃的成員。";
                return RedirectToAction("Index", new { id = programId });
            }

            // 檢查該成員是否已存在，但狀態為 "不在"
            var inactiveProgramMember = _db.ProgramMembers.FirstOrDefault(pm => pm.ProgramId == programId && pm.MemberId == member.MemberId && pm.MemberState == "不在");
            if (inactiveProgramMember != null) {
                // 若該計劃曾經有這個成員，只是狀態為"不在"，改成"還在"
                inactiveProgramMember.MemberState = "還在";
            }
            else {
                // 創建一個新的 ProgramMember 實例並儲存
                var newProgramMember = new ProgramMember { ProgramId = programId, MemberId = member.MemberId, MemberState = "還在" };
                _db.ProgramMembers.Add(newProgramMember);

                string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
                int.TryParse(userIdStr, out int userId);
                int EditMemberId = userId; //誰編輯這個計畫概述的
                                           //新增通知
                var NotifyAdd = new Notify {
                    NotifyDate = DateTime.Now,
                    NotifyAction = "新增",
                    NotifyType = "成員",
                    ActionName = member.MemberName,
                    ProgramId = programId,
                    MemberId = EditMemberId
                };
                _db.Notifies.Add(NotifyAdd);


            }
           
            _db.SaveChanges();

            TempData["Message"] = "成功添加成員。";
            return RedirectToAction("Index", new { id = programId });
        }




        [HttpPost]
        public IActionResult DeleteMember(string memberAccount) {
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);

            // 檢查是否存在該成員
            var member = _db.Members.FirstOrDefault(m => m.MemberAccount == memberAccount);
            if (member == null) {
                TempData["Message"] = "該成員不存在。";
                return RedirectToAction("Index", new { id = programId });
            }

            // 檢查成員是否有未完成的任務
            var unfinishedMissions = _db.Missions
                .Where(m => m.MemberId == member.MemberId && (m.MisState == "新任務" || m.MisState == "進行中"))
                .Any();

            if (unfinishedMissions) {
                // 如果成員有未完成的任務，返回錯誤訊息
                TempData["Message"] = "該成員還有未完成的任務。請先完成所有任務再刪除該成員。";
                return RedirectToAction("Index", new { id = programId });
            }

            // 從計劃成員中找到該成員
            var memberInProgram = _db.ProgramMembers.FirstOrDefault(pm => pm.MemberId == member.MemberId && pm.ProgramId == programId);
            if (memberInProgram != null) {
                // 將該成員的狀態改為 "不在"
                memberInProgram.MemberState = "不在";

                string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
                int.TryParse(userIdStr, out int userId);
                int EditMemberId = userId; //誰編輯這個計畫概述的
                                           //新增通知
                var NotifyAdd = new Notify {
                    NotifyDate = DateTime.Now,
                    NotifyAction = "刪除",
                    NotifyType = "成員",
                    ActionName = member.MemberName,
                    ProgramId = programId,
                    MemberId = EditMemberId
                };
                _db.Notifies.Add(NotifyAdd);


                _db.SaveChanges();
                TempData["Message"] = "成員已成功從計劃中移除。";
            }
            else {
                TempData["Message"] = "找不到該成員在計劃中的記錄。";
            }

            return RedirectToAction("Index", new { id = programId });
        }




    }
}
