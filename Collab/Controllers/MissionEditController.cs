using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {
    
    public class MissionEditController : Controller {
        private readonly ILogger<MissionEditController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        public MissionEditController(ILogger<MissionEditController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }

        public IActionResult Index() {

            
            return View();
        }
        [HttpPost]
        public IActionResult UpsertMission(int MissionId, string MissionName, string? MisStartTime, string? MisFinishTime, string MisState, string? MisDescribe, int? IntentId, int? MemberId) {
            Console.WriteLine(MissionId);
            Console.WriteLine(MissionName);
            if (MissionId > 0) {
                // MissionId大於0，表示要進行更新
                return UpdateMission(MissionId,MissionName,MisStartTime,MisFinishTime,MisState,MisDescribe,IntentId,MemberId);
            } else {
                // MissionId為0或null，表示要進行新增
                return CreateMission(MissionName, MisStartTime, MisFinishTime, MisState, MisDescribe, IntentId, MemberId);
            }
        }

        // 更新Mission
        private IActionResult UpdateMission(int MissionId, string MissionName, string? MisStartTime, string? MisFinishTime, string MisState, string? MisDescribe, int? IntentId, int? MemberId) {
            try {
                // 使用MissionId查詢資料庫中對應的Mission
                var existingMission = _TestBananaContext.Missions.Find(MissionId);
                if (existingMission == null) {
                    return NotFound("找不到對應的Mission");
                }

                // 更新Mission的其他屬性
                existingMission.MissionName = MissionName;
                existingMission.MisStartTime = MisStartTime;
                existingMission.MisFinishTime = MisFinishTime;
                existingMission.MisState = MisState;
                existingMission.MisDescribe = MisDescribe;
                existingMission.IntentId = IntentId;
                existingMission.MemberId = MemberId;

                // 儲存變更到資料庫
                _TestBananaContext.SaveChanges();

                return Ok("Mission更新成功");
            } catch (Exception ex) {
                return StatusCode(500, $"更新Mission時發生錯誤: {ex.Message}");
            }
        }

        // 新增Mission
        private IActionResult CreateMission(string MissionName, string? MisStartTime, string? MisFinishTime, string MisState, string? MisDescribe, int? IntentId, int? MemberId) {
            try {

                var mission = new Mission {
                    MissionName = MissionName,
                    MisStartTime = MisStartTime,
                    MisFinishTime = MisFinishTime,
                    MisState = MisState,
                    MisDescribe = MisDescribe,
                    IntentId = IntentId,
                    MemberId = MemberId
                };
                // 新增Mission到資料庫
                _TestBananaContext.Missions.Add(mission);
                _TestBananaContext.SaveChanges();

                return Ok("Mission新增成功");
            } catch (Exception ex) {
                return StatusCode(500, $"新增Mission時發生錯誤: {ex.Message}");
            }
        }
    }
}
