using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class MissionController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
