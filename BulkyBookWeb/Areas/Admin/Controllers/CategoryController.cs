using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            // OR, var objCategoryList = _db.Categories.ToList();

            return View(objCategoryList);
        }


        //
        public IActionResult Create()
        {
            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Create(Category obj)
        {
            //Custom validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            //Server side validations
            if (ModelState.IsValid)
            {
                // add categgory object to categories list
                _unitOfWork.Category.Add(obj);
                // save categgory object to the database
                _unitOfWork.Save();
                // display temp data message to user interface
                TempData["success"] = "Category created successfully";
                // return to page index
                return RedirectToAction("Index");
            }
            // return model object tom the view
            return View(obj);         
        }


        //--------------------------------- Update --------------------------------------------//

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // we assume here that id is a primary key
            //var categoryFromDb = _db.Categories.Find(id);
            // if id is not a primary key, the we can use this method
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id==id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Edit(Category obj)
        {
            //Custom validation
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            //Server side validations
            if (ModelState.IsValid)
            {
                // Based on the primary key, it will automatically update all the properties
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Category updated successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //------------------------------------ Delete --------------------------------------------//


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult DeletePOST(int? id)
        {
            //var obj = _db.Categories.Find(id);
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
