using Microsoft.AspNetCore.Mvc;
using Collab.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Immutable;

namespace collab_00.Controllers
{
    public class PersonalOverviewController : Controller
    {
        private readonly TestBananaContext _TestBananaContext;
        public PersonalOverviewController(TestBananaContext context)
        {
            _TestBananaContext = context;
        }
        public IActionResult Index()
        {
            string? stringUserID = Request.Cookies["UserID"];
            int UserID;
            if (int.TryParse(stringUserID, out UserID))
            {
                //var user  = _TestBananaContext.Members.FirstOrDefault(m => m.MemberId == UserID);
                //if (user != null)
                //{
                //    ViewBag.MemberName = user.MemberName;
                //}
                var ingmissions = _TestBananaContext.Missions
        .Where(m => m.MemberId == UserID & m.MisState == "進行中" || m.MemberId == UserID & m.MisState == "新任務")
        .ToList();
                var donemissions = _TestBananaContext.Missions
        .Where(m => m.MemberId == UserID & m.MisState == "已完成")
        .ToList();

                ViewBag.donemissions = donemissions;
                ViewBag.ingmissions = ingmissions;
                return View();
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }

        }
    }
}
