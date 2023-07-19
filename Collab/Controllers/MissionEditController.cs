using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {
    public class MissionEditController : Controller {

        public IActionResult Index() {

            //// 從資料庫獲取數據
            //List<string> IntentName = new List<string>();
            //var intents = _dbContext.Intents;
            //foreach (var intent in intents)
            //{
            //    IntentName.Add(intent.Name);
            //}

            //// 將目標傳遞給視窗
            //ViewBag.IntentName = IntentName;
            return View();
        }
    }
}
