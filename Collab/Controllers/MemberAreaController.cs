using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class MemberAreaController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
