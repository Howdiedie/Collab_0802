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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string TitleAdd, string ContentAdd)
        {

            var AddNB = new Notebook
            {
                NotebookTitle = TitleAdd,
                NotebookContent = ContentAdd,
                NotebooAddDate = DateTime.Now,
                ProgramId = 1,
                MemberId = 3
            };
            _bananaContext.Notebooks.Add(AddNB);
            _bananaContext.SaveChanges();
            return RedirectToAction("Index","Notebook");
        }
    }
}
