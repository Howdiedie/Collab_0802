using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {

    public class MissionEditController : Controller {
        private readonly TestBananaContext _TestBananaContext; // 你的 DbContext

        public MissionEditController(TestBananaContext context)
        {
            _TestBananaContext = context;
        }

        public IActionResult Index() {

            
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveMission(Mission mission)
        {
            if (ModelState.IsValid)
            {
                _TestBananaContext.Add(mission);
                await _TestBananaContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(mission);
        }

    }
}
