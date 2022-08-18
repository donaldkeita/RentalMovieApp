using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Create(Product obj)
        {
            //Server side validations
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product type created successfully";

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
            //var ProductFromDb = _db.Products.Find(id);
            // if id is not a primary key, the we can use this method
            var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

            if (productFromDbFirst == null)
            {
                return NotFound();
            }
            return View(productFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Edit(Product obj)
        {
            //Server side validations
            if (ModelState.IsValid)
            {
                // Based on the primary key, it will automatically update all the properties
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product type updated successfully";

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
            //var productFromDb = _db.Products.Find(id);
            var productFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (productFromDbFirst == null)
            {
                return NotFound();
            }
            return View(productFromDbFirst);
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult DeletePOST(int? id)
        {
            //var obj = _db.Products.Find(id);
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Product type deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
