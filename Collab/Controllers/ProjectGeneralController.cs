using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class ProjectGeneralController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
