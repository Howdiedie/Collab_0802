﻿using Collab.Models;
using Microsoft.AspNetCore.Mvc;
using Collab.Filters;

namespace collab_00.Controllers {
    public class NotebookController : Controller {

        private readonly TestBananaContext _bananaContext;

        public NotebookController(TestBananaContext bananaContext)
        {
            _bananaContext = bananaContext;
        }
        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        public IActionResult Index(int? SortNum) {

            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            

            var NB_Bag = from Howhow in _bananaContext.Notebooks
                         join Diedie in _bananaContext.Members on Howhow.MemberId equals Diedie.MemberId
                         where Howhow.ProgramId == programId
                         select new TestBananaContext
                         {
                             NBT = Howhow.NotebookTitle,
                             NBCT = Howhow.NotebooAddDate.HasValue ? Howhow.NotebooAddDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                             NBImg = Diedie.MemberPhoto
                         };
            switch (SortNum)
            {
                case 1:
                    NB_Bag = NB_Bag.OrderByDescending(x => x.NBCT);
                    break;

                case 2:
                    NB_Bag = NB_Bag.OrderBy(x => x.NBCT);
                    break;

                default:
                    break;
            }
            //if (SerchWd != null)
            //{
            //    NB_Bag = NB_Bag.Where(item => item.NBT.Contains(SerchWd));
            //}
            return View(NB_Bag.ToList());
        }

        [ServiceFilter(typeof(ProfilePicturePathFilter))]
        [HttpPost]
        public IActionResult Index(string SerchWd)
        {

            string programIdStr = Request.Cookies["ProgramId"];
            int.TryParse(programIdStr, out int programId);
            
            if (SerchWd == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var NB_Bag = from Howhow in _bananaContext.Notebooks
                             join Diedie in _bananaContext.Members on Howhow.MemberId equals Diedie.MemberId
                             where Howhow.ProgramId == programId && Howhow.NotebookTitle.Contains(SerchWd)
                             select new TestBananaContext
                             {
                                 NBT = Howhow.NotebookTitle,
                                 NBCT = Howhow.NotebooAddDate.HasValue ? Howhow.NotebooAddDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                                 NBImg = Diedie.MemberPhoto
                             };

                ViewBag.SrWd = SerchWd;
                return View(NB_Bag.ToList());
            }

        }
    }
}
