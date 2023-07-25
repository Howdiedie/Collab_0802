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
			int NowProgramId = 1;
			int EditMemberId = 2; //操作這個頁面的member的memberID傳進來，這裡預設為2
			var newTarget = new Intent
			{
				IntentName = target,
				ProgramId = NowProgramId,
				MissionCountTotal = 0,
				MissionCountFinish = 0
			};

			//新增通知
			var NotifyAdd = new Notify
			{
				NotifyDate = DateTime.Now,
				NotifyAction = "新增",
				NotifyType = "目標",
				ActionName = newTarget.IntentName,
				ProgramId = newTarget.ProgramId,
				MemberId = EditMemberId
			};
			_bananaContext.Notifies.Add(NotifyAdd);
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
                    sortList = sortList.OrderByDescending(item => item.missionCount != 0 && item.missionFinish != 0 ? (double)(item.missionFinish) / (double)(item.missionCount) * 100 : 0);
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
