using Collab.Controllers;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {
    public class MissionController : Controller {
        private readonly ILogger<MissionController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        public MissionController(ILogger<MissionController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }

        public IActionResult Index() {
            //全部的mission
            var missions = from mission in _TestBananaContext.Missions
                           join intent in _TestBananaContext.Intents on mission.IntentId equals intent.IntentId
                           join member in _TestBananaContext.Members on mission.MemberId equals member.MemberId
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
                        where intent.ProgramId == 1
                        select new {
                            IntentName = intent.IntentName,
                            IntentId = intent.IntentId
                        };
            //這個專案裡面全部的人員
            var membersInProgram = from member in _TestBananaContext.Members
                                   join programMember in _TestBananaContext.ProgramMembers
                                   on member.MemberId equals programMember.MemberId
                                   where programMember.ProgramId == 1 && programMember.MemberState == "還在"
                                   select new {
                                       MemberAccount = member.MemberAccount,
                                       MemberId = member.MemberId
                                   };


            ViewBag.membersInProgram = membersInProgram.ToList();
            ViewBag.option = query.ToList();
            var result = missions.ToList();

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

            if (missionToUpdate != null) {
                // 更新MisState的值
                missionToUpdate.MisState = misState;

                // 保存更改到数据库
                _TestBananaContext.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Mission not found." });

        }
    }
}
