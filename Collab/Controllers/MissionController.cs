using Collab.Controllers;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {
    public class MissionController : Controller {
        private readonly ILogger<MissionController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        public MissionController(ILogger<MissionController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }


        public IActionResult Index() {
            var missions = _TestBananaContext.Missions
                            .Select(m => new MissionViewModel {
                                MissionName = m.MissionName,
                                MisStartTime = m.MisStartTime,
                                MisFinishTime = m.MisFinishTime,
                                MisState = m.MisState
                            })
                            .ToList();

            return View(missions);
        }
    }
}
