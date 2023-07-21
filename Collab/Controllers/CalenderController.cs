using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Collab.Controllers
{
    public class CalenderController : Controller
    {

        private readonly TestBananaContext _bananaContext;

        public CalenderController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CalenderPage()
        {
            var CalIfo = from cal in _bananaContext.Missions
                         join intent in _bananaContext.Intents on cal.IntentId equals intent.IntentId
                         join program in _bananaContext.Programs on intent.ProgramId equals program.ProgramId
                         where program.ProgramId == 1
                         select new TestBananaContext
                         {
                             MisTit = cal.MissionName,
                             MisSta = cal.MisStartTime.ToString(),
                             MisEnd = cal.MisFinishTime.ToString()
                         };

            return View(CalIfo.ToList());
        }
    }
}
