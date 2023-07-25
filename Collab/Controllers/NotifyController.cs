using Collab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace Collab.Controllers {
    
    public class NotifyController : Controller {
        private readonly ILogger<NotifyController> _logger;
        private readonly TestBananaContext _TestBananaContext;
        // GET: api/<TodoController>
        
        public NotifyController(ILogger<NotifyController> logger, TestBananaContext testBananaContext) {
            _logger = logger;
            _TestBananaContext = testBananaContext;
        }

        public IActionResult Index() {
            var query = from notify in _TestBananaContext.Notifies
                        join member in _TestBananaContext.Members on notify.MemberId equals member.MemberId
                        orderby notify.NotifyDate descending
                        select new NotifyWithMember {
                            NotifyId = notify.NotifyId,
                            NotifyDate = notify.NotifyDate,
                            NotifyAction = notify.NotifyAction,
                            NotifyType = notify.NotifyType,
                            ActionName = notify.ActionName,
                            ProgramId = notify.ProgramId,
                            MemberId = notify.MemberId,
                            MemberName = member.MemberName
                        };

            return View(query.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}