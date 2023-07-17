using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class LoginController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
