using Collab.Filters;
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
        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult CalenderPage()
        {
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);

            var CalIfo = from cal in _bananaContext.Missions
                         join intent in _bananaContext.Intents on cal.IntentId equals intent.IntentId
                         join program in _bananaContext.Programs on intent.ProgramId equals program.ProgramId
                         where program.ProgramId == programId
                         select new TestBananaContext
                         {
                             MisTit = cal.MissionName,
                             MisSta = cal.MisStartTime.ToString(),
                             MisEnd = cal.MisFinishTime.ToString()
                         };

            if (CalIfo.Any())
            {
                // 返回视图，并传递列表
                return View(CalIfo.ToList());
            }

            // 返回空视图
            return View();
        }
    }
}
