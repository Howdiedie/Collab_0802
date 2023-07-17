using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class SignController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
