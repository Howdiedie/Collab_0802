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

		public IActionResult Sort(int? sortOrder)
		{
			var sortList = from targetItem in _bananaContext.Intents
						   select new TestBananaContext
						   {
							   targetName = targetItem.IntentName,
							   missionCount = targetItem.MissionCountTotal,
							   missionFinish = targetItem.MissionCountFinish,
							   targetID = targetItem.IntentId,
						   };

			switch (sortOrder) {
				case 1:
					// 按新增日期排序
					sortList = sortList.OrderBy(item => item.targetID);
					break;
				case 2:
					// 按進度排序
					sortList = sortList.OrderBy(item => item.missionCount != 0 ? item.missionFinish / item.missionCount : 0);
					break;
				case 3:
					// 按任務數量排序
					sortList = sortList.OrderBy(item => item.missionCount);
					break;
				default:
					break;
			}

			return View("Index", sortList.ToList());
		}

	}
}
