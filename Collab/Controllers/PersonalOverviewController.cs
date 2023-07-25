using Collab.Filters;
using Microsoft.AspNetCore.Mvc;
using Collab.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Immutable;
using System.Diagnostics;

namespace collab.Controllers
{
	public class PersonalOverviewController : Controller
	{
		private readonly TestBananaContext _TestBananaContext;
		public PersonalOverviewController(TestBananaContext context)
		{
			_TestBananaContext = context;
		}
        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index()
		{
			string? stringUserID = Request.Cookies["UserID"];
			int UserID;
			if (int.TryParse(stringUserID, out UserID))
			{
				var ingmissions = _TestBananaContext.Missions
				.Where(m => m.MemberId == UserID && (m.MisState == "進行中" || m.MisState == "新任務"))
				.Join(
				_TestBananaContext.Intents,
				m => m.IntentId,
				i => i.IntentId,
				(m, i) => new
				{
					MissionId=m.MissionId,
					ProgramID = i.ProgramId,
					MissionName=m.MissionName,
					MisFinishTime = m.MisFinishTime,
					MisStartTime = m.MisStartTime

				}
				)
				.Join(
				_TestBananaContext.Programs,
				combined => combined.ProgramID,// 使用前一個 JOIN 中保留的 i.ProgramId 來關聯 Programs 資料表
				p => p.ProgramId,
				(combined, p) => new
				{
					MissionId = combined.MissionId,
					MissionName = combined.MissionName,
					MisFinishTime = combined.MisFinishTime,
					MisStartTime = combined.MisStartTime,
					ProgramColor = p.ProgramColor
				}
				)
				.OrderBy(item => item.MisStartTime)
				.ToList();

				//----------------------------------------------------------------------------------
				DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
				var donemissions = _TestBananaContext.Missions
				.Where(m => m.MemberId == UserID && m.MisState == "已完成" && DateTime.Compare((DateTime)m.MisFinishTime, sevenDaysAgo) >= 0)
				.Join(
				_TestBananaContext.Intents,
				m => m.IntentId,
				i => i.IntentId,
				(m, i) => new
				{
					MissionId = m.MissionId,
					ProgramID = i.ProgramId,
					MissionName = m.MissionName,
					MisFinishTime=m.MisFinishTime,
					MisStartTime=m.MisStartTime

				}
				)
				.Join(
				_TestBananaContext.Programs,
				combined => combined.ProgramID,// 使用前一個 JOIN 中保留的 i.ProgramId 來關聯 Programs 資料表
				p => p.ProgramId,
				(combined, p) => new {
					MissionId = combined.MissionId,
					MissionName = combined.MissionName,
					MisFinishTime = combined.MisFinishTime, 
					MisStartTime = combined.MisStartTime ,
					ProgramColor = p.ProgramColor
				}
				)
				.OrderBy(item => item.MisStartTime)
				.ToList();

				ViewBag.ingmissions = ingmissions;
				ViewBag.donemissions = donemissions;
				return View();
			}
			else
			{
				return RedirectToAction("Error", "Shared");
			}

		}
		
	}
}
