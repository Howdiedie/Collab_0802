using Microsoft.AspNetCore.Mvc;
using Collab.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return View();
        }
    }
}
