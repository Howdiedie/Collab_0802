using Collab.Models;
using Collab.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace collab_00.Controllers {
    public class IntentController : Controller {
		private readonly TestBananaContext _bananaContext;

		public IntentController(TestBananaContext bananaContext)
		{
			_bananaContext = bananaContext;
		}

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(int id)
		{
			//全部的mission
			var missions = from mission in _bananaContext.Missions
						   join intent in _bananaContext.Intents on mission.IntentId equals intent.IntentId
						   join member in _bananaContext.Members on mission.MemberId equals member.MemberId
						   where intent.ProgramId == id
						   orderby mission.MisFinishTime ascending // 将 MisFinishTime 字段降序排序
						   select new
						   {
							   Mission = mission,
							   IntentId = intent.IntentId,
							   MemberPhoto = member.MemberPhoto,
							   MemberAccount = member.MemberAccount,
							   MemberId = member.MemberId
						   };
			//全部的Intent
			var query = from intent in _bananaContext.Intents
						join program in _bananaContext.Programs on intent.ProgramId equals program.ProgramId
						where intent.ProgramId == id
						select new
						{
							IntentName = intent.IntentName,
							IntentId = intent.IntentId
						};
			//這個專案裡面全部的人員
			var membersInProgram = from member in _bananaContext.Members
								   join programMember in _bananaContext.ProgramMembers
								   on member.MemberId equals programMember.MemberId
								   where programMember.ProgramId == id && programMember.MemberState == "還在"
								   select new
								   {
									   MemberAccount = member.MemberAccount,
									   MemberId = member.MemberId
								   };


			ViewBag.membersInProgram = membersInProgram.ToList();
			var result = missions.ToList();
			ViewBag.option = query.ToList();

			if (result != null && result.Count > 0) {
				ViewBag.MissionWithIntent = result;
			}
			else {
				ViewBag.MissionWithIntent = null;
			}

			var obj = from targetItem in _bananaContext.Intents
					  where targetItem.ProgramId == id
					  orderby targetItem.IntentId descending
					  select new TestBananaContext
					  {
						  targetID = targetItem.IntentId,
						  targetName = targetItem.IntentName,
						  missionCount = targetItem.MissionCountTotal,
						  missionFinish = targetItem.MissionCountFinish
					  };

			return View(obj.ToList());
		}


		[HttpPost]
		public IActionResult AddTarget(string target)
		{
			string programIdStr = Request.Cookies["ProgramId"];
			int.TryParse(programIdStr, out int programId);
			string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
			int.TryParse(userIdStr, out int userId);

			if (string.IsNullOrWhiteSpace(target)) {
				return RedirectToAction("Index",new { id = programId });
			}

			var newTarget = new Intent
			{
				IntentName = target,
				ProgramId = programId,
				MissionCountTotal = 0,
				MissionCountFinish = 0
			};

			//新增通知
			var NotifyAdd = new Notify {
				NotifyDate = DateTime.Now,
				NotifyAction = "新增",
				NotifyType = "目標",
				ActionName = newTarget.IntentName,
				ProgramId = newTarget.ProgramId,
				MemberId = userId
			};
			_bananaContext.Notifies.Add(NotifyAdd);
			_bananaContext.Intents.Add(newTarget);
			_bananaContext.SaveChanges();

			return RedirectToAction("Index", new { id = programId });
		}

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Sort(int? sortOrder)
		{
			string programIdStr = Request.Cookies["ProgramId"];
			int.TryParse(programIdStr, out int programId);

			//全部的Intent
			var query = from intent in _bananaContext.Intents
						join program in _bananaContext.Programs on intent.ProgramId equals program.ProgramId
						where intent.ProgramId == programId
						select new
						{
							IntentName = intent.IntentName,
							IntentId = intent.IntentId
						};
			//這個專案裡面全部的人員
			var membersInProgram = from member in _bananaContext.Members
								   join programMember in _bananaContext.ProgramMembers
								   on member.MemberId equals programMember.MemberId
								   where programMember.ProgramId == programId && programMember.MemberState == "還在"
								   select new
								   {
									   MemberAccount = member.MemberAccount,
									   MemberId = member.MemberId
								   };


			ViewBag.membersInProgram = membersInProgram.ToList();
			ViewBag.option = query.ToList();


			var sortList = from targetItem in _bananaContext.Intents
						   join program in _bananaContext.Programs on targetItem.ProgramId equals program.ProgramId
						   where targetItem.ProgramId == programId
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
					sortList = sortList.OrderByDescending(item => item.targetID);
					break;
				case 2:
                    // 按進度排序
                    sortList = sortList.OrderByDescending(item => item.missionCount != 0 && item.missionFinish != 0 ? (double)(item.missionFinish) / (double)(item.missionCount) * 100 : 0);
					break;
				case 3:
					// 按任務數量排序
					sortList = sortList.OrderByDescending(item => item.missionCount);
					break;
				default:
					break;
			}

			return View("Index", sortList.ToList());
		}


	}
}

