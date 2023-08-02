using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Collab.Filters;

namespace collab_00.Controllers {
    public class NoteEditController : Controller {

        private readonly TestBananaContext _bananaContext;

        public NoteEditController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(string? NBTitle)
        {
            var EditBag = from EB in _bananaContext.Notebooks
                          where EB.NotebookTitle == NBTitle
                          select new TestBananaContext
                          {
                              NBT = EB.NotebookTitle,
                              NBCT = EB.NotebooAddDate.ToString(),
                              NBOverview = EB.NotebookContent,
                              NBID = EB.NotebookId
                          };

            
            return View(EditBag.ToList());
        }

        [HttpPost]
        public IActionResult Index(string ChangeTitle, string ChangeOverView, int NBID)
        {
            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            string userIdStr = Request.Cookies["UserID"];  // 從 Session 或 Cookie 中獲取當前登錄會員的 ID
            int.TryParse(userIdStr, out int userId);

            var notebook = _bananaContext.Notebooks.FirstOrDefault(n => n.NotebookId == NBID );
            if (notebook != null)
            {
                notebook.NotebookTitle = ChangeTitle;
                notebook.NotebookContent = ChangeOverView;

                var NotifyAdd = new Notify
                {
                    NotifyDate = DateTime.Now,
                    NotifyAction = "修改",
                    NotifyType = "記事本",
                    ActionName = ChangeTitle,
                    ProgramId = programId,
                    MemberId = userId
                };
                
                _bananaContext.Notifies.Add(NotifyAdd);
                _bananaContext.SaveChanges();  // 儲存變更
            }

            return RedirectToAction("Index","Notebook");
        }
    }
}
