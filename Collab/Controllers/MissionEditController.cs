using Collab.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace collab_00.Controllers {
    
    public class MissionEditController : Controller {
        private readonly TestBananaContext _bananaContext;

        public MissionEditController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        public IActionResult Index() {

            
            return View();
        }


    }
}
