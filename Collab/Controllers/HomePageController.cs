using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class HomePageController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
