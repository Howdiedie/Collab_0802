using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class IntentController : Controller {
		private readonly TestBananaContext _bananaContext;

		public IntentController(TestBananaContext bananaContext)
		{
			_bananaContext = bananaContext;
		}
		public IActionResult Index()
		{

			var obj = from targetItem in _bananaContext.Intents
					  select new TestBananaContext
					  {
						  targetName = targetItem.IntentName,
						  missionCount = targetItem.MissionCountTotal,
						  missionFinish = targetItem.MissionCountFinish
					  };

			return View(obj.ToList());
		}


		[HttpPost]
		public IActionResult AddTarget(string target)
		{
			var newTarget = new Intent
			{
				IntentName = target,
				ProgramId = 1
			};

			_bananaContext.Intents.Add(newTarget);
			_bananaContext.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
