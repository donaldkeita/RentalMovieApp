using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Cover> objCoverList = _unitOfWork.Cover.GetAll();
            return View(objCoverList);
        }

        public IActionResult Create()
        {
            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Create(Cover obj)
        {
            //Server side validations
            if (ModelState.IsValid)
            {
                _unitOfWork.Cover.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "Cover type created successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //-----------------------------------------------------------------------------------//

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // we assume here that id is a primary key
            //var coverFromDb = _db.Covers.Find(id);
            // if id is not a primary key, the we can use this method
            var coverFromDbFirst = _unitOfWork.Cover.GetFirstOrDefault(c => c.Id == id);

            if (coverFromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Edit(Cover obj)
        {
            //Server side validations
            if (ModelState.IsValid)
            {
                // Based on the primary key, it will automatically update all the properties
                _unitOfWork.Cover.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Cover type updated successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //----------------------------------------------------------------------------------------//


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var coveryFromDb = _db.Covers.Find(id);
            var coverFromDbFirst = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);

            if (coverFromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverFromDbFirst);
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult DeletePOST(int? id)
        {
            //var obj = _db.Covers.Find(id);
            var obj = _unitOfWork.Cover.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Cover.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Cover type deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
