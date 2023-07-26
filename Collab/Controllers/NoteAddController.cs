using Collab.Filters;
using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Collab.Controllers
{
    public class NoteAddController : Controller
    {
        private readonly TestBananaContext _bananaContext;

        public NoteAddController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        [HttpPost]
        public IActionResult Index(string TitleAdd, string ContentAdd)
        {
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);

            var AddNB = new Notebook
            {
                NotebookTitle = TitleAdd,
                NotebookContent = ContentAdd,
                NotebooAddDate = DateTime.Now,
                ProgramId = programId,
                MemberId = userId
            };

            var NotifyAdd = new Notify
            {
                NotifyDate = DateTime.Now,
                NotifyAction = "新增",
                NotifyType = "記事本",
                ActionName = TitleAdd,
                ProgramId = programId,
                MemberId = userId                
            };
            _bananaContext.Notebooks.Add(AddNB);
            _bananaContext.Notifies.Add(NotifyAdd);
            _bananaContext.SaveChanges();
            return RedirectToAction("Index","Notebook");
        }
    }
}
