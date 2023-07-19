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
            var missions = from intent in _TestBananaContext.Intents
                           join mission in _TestBananaContext.Missions on intent.IntentId equals  mission.IntentId
                           select new {
                               Mission = mission,
                               IntentName = intent.IntentName
                           };

            var query = from intent in _TestBananaContext.Intents
                        join program in _TestBananaContext.Programs on intent.ProgramId equals program.ProgramId
                        where intent.ProgramId == 1
                        select intent.IntentName;

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
