using Collab.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class MissionEditController : Controller {
        private readonly TestBananaContext _bananaContext;

        public MissionEditController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        public IActionResult Index() {
            return View();
        }


        [HttpPost]
        public IActionResult Save(MissionEdit model)
        {
            if (ModelState.IsValid) // 確認模型驗證通過
            {
                var newMission = new Mission
                {
                    MissionName = model.TaskName,
                    MisStartTime = model.startDate,
                    MisFinishTime = model.endDate,
                    MisState = model.status,
                    IntentID = model.targetSelect,
                    MisDescribe = model.description,
                    MemberID = 1
                };

                _bananaContext.Missions.Add(newMission);
                _bananaContext.SaveChanges();

                return RedirectToAction("Index");
            }

            // 如果模型驗證失敗，返回原頁面以顯示錯誤訊息
            return View(model);
        }


    }
}
