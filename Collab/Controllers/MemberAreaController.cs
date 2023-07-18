using Collab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Collab.Controllers {
    public class MemberAreaController : Controller {

        //*************************************   連結 TestBanana 資料庫  ********************************* (start)
        #region
        private readonly TestBananaContext _db = new TestBananaContext();
        #endregion
        //*************************************   連結 TestBanana 資料庫  ********************************* (end)

        public MemberAreaController(TestBananaContext context) {  //                                                                                          ****************************（自己動手加上）
            _db = context;  
        }

        public IActionResult Index() {
            
            var member = _db.Members.FirstOrDefault();

            return View(member);
        }

        [HttpPost] 
        public IActionResult Edit(Member updatedMember) {
            if (ModelState.IsValid) {
                var existingMember = _db.Members.FirstOrDefault();
                if (existingMember != null) {
                    existingMember.MemberName = updatedMember.MemberName;
                    _db.SaveChanges(); // 將更新後的資料寫回資料庫
                }

                return RedirectToAction("Index");
            }

            return View(updatedMember);
        }


        


    }
}
