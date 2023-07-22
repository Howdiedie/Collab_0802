using Collab.Filters;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class PersonalOverviewController : Controller {

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index() {
            return View();
        }
    }
}
