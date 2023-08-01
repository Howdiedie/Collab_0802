using Collab.Controllers;
using Collab.Models;
using Collab.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace collab_00.Controllers {
    public class MissionController : Controller {
        private readonly ILogger<MissionController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        public MissionController(ILogger<MissionController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }
        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(int id) {
            Console.WriteLine(id);
            //全部的mission
            var missions = from mission in _TestBananaContext.Missions
                           join intent in _TestBananaContext.Intents on mission.IntentId equals intent.IntentId
                           join member in _TestBananaContext.Members on mission.MemberId equals member.MemberId
                           where intent.ProgramId == id
                           orderby mission.MisFinishTime ascending // 将 MisFinishTime 字段降序排序
                           select new {
                               Mission = mission,
                               IntentId = intent.IntentId,
                               MemberPhoto = member.MemberPhoto,
                               MemberAccount = member.MemberAccount,
                               MemberId = member.MemberId
                           };
            //全部的Intent
            var query = from intent in _TestBananaContext.Intents
                        join program in _TestBananaContext.Programs on intent.ProgramId equals program.ProgramId
                        where intent.ProgramId == id
                        select new {
                            IntentName = intent.IntentName,
                            IntentId = intent.IntentId
                        };
            //這個專案裡面全部的人員
            var membersInProgram = from member in _TestBananaContext.Members
                                   join programMember in _TestBananaContext.ProgramMembers
                                   on member.MemberId equals programMember.MemberId
                                   where programMember.ProgramId == id && programMember.MemberState == "還在"
                                   select new {
                                       MemberAccount = member.MemberAccount,
                                       MemberId = member.MemberId
                                   };


            ViewBag.membersInProgram = membersInProgram.ToList();
            var result = missions.ToList();
            ViewBag.option = query.ToList();

            if (result != null && result.Count > 0) {
                ViewBag.MissionWithIntent = result;
            } else {
                ViewBag.MissionWithIntent = null;
            }
            return View();
        }

        [HttpPost]
        public IActionResult ActionName(string missionName, string misState) {
            // 查询数据库中具有特定MissionName的Mission实例
            var missionToUpdate = _TestBananaContext.Missions.FirstOrDefault(m => m.MissionName == missionName);
            // 從 Session 或 Cookie 中獲取當前的 Program ID
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            string userIdStr = Request.Cookies["UserID"]; 
            int.TryParse(userIdStr, out int userId);
            if (missionToUpdate != null) {
                // 更新MisState的值
                missionToUpdate.MisState = misState;
                //新增通知
                //var NotifyAdd = new Notify {
                //    NotifyDate = DateTime.Now,
                //    NotifyAction = "修改",
                //    NotifyType = "任務",
                //    ActionName = missionName,
                //    ProgramId = programId,
                //    MemberId = userId
                //};
                //_TestBananaContext.Notifies.Add(NotifyAdd);
                // 保存更改到数据库
                _TestBananaContext.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Mission not found." });

        }
    }
}
