using Collab.Models;
using Microsoft.AspNetCore.Mvc;

namespace collab_00.Controllers {
    public class NoteEditController : Controller {

        private readonly TestBananaContext _bananaContext;

        public NoteEditController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        public IActionResult Index(string? NBTitle)
        {

            var EditBag = from EB in _bananaContext.Notebooks
                          where EB.NotebookTitle == NBTitle
                          select new TestBananaContext
                          {
                              NBT = EB.NotebookTitle,
                              NBCT = EB.NotebooAddDate.ToString(),
                              NBOverview = EB.NotebookContent
                          };

            return View(EditBag.ToList());
        }

        [HttpPost]
        public IActionResult Index(string ChangeTitle, string ChangeOverView, string NBTime)
        {
            var notebook = _bananaContext.Notebooks.FirstOrDefault(n => n.NotebooAddDate.ToString() == NBTime);
            if (notebook != null)
            {
                notebook.NotebookTitle = ChangeTitle;
                notebook.NotebookContent = ChangeOverView;
                _bananaContext.SaveChanges();  // 儲存變更
            }

            return RedirectToAction("Index","Notebook");
        }
    }
}
