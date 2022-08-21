using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
//using Microsoft.Build.Tasks.Deployment.Bootstrapper;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            //return View(objProductList);
            return View();
        }

        //-----------------------------------------------------------------------------------//

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                CoverList = _unitOfWork.Cover.GetAll().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
            };
            
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverList"] = CoverList;
                //return View(product);
                return View(productVM);
            }
            else
            {
                //update
            }
            return View(productVM);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //Help prevent Cross Site Request Forgery Attack
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            //Server side validations
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products" + filename + extension;
                }
                // Based on the primary key, it will automatically update all the properties
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();

                TempData["success"] = "Product created successfully";

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

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //var productList = _unitOfWork.Product.GetAll();
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Cover");
            return Json(new {data = productList});
        }
        #endregion
    }
}
