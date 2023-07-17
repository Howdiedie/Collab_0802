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
            var notifies = _TestBananaContext.Notifies.ToList(); // 檢索Notifies資料表中的所有資料
            // 若要取得Notifies資料表的所有欄位，您可以使用反射：
            var properties = typeof(Notify).GetProperties().Select(p => p.Name);
            ViewBag.Properties = properties;

            return View(notifies);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}