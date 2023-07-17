using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class NotebookController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
