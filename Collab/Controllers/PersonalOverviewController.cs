using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class PersonalOverviewController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
