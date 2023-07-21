using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class MissionEditController : Controller {
        private readonly ILogger<MissionEditController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        public MissionEditController(ILogger<MissionEditController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }

        public IActionResult Index() {
            var query = from intent in _TestBananaContext.Intents
                        join program in _TestBananaContext.Programs on intent.ProgramId equals program.ProgramId
                        where intent.ProgramId == 1
                        select intent.IntentName;

            ViewBag.option = query.ToList();
            return View();
        }
    }
}
