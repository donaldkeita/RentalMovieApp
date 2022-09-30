using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        

        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
                return View(company);
            }
        }

        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created successfully";
                }

                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);           
        }


        // ------------------------------ API Calls -------------------------------------

        #region API CALLS

        //------------------------------------ GetAll --------------------------------------------//

        [HttpGet]
        public IActionResult GetAll()
        {
            //var companyList = _unitOfWork.Company.GetAll();
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        //------------------------------------ Delete --------------------------------------------//

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            //var obj = _db.Products.Find(id);
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            // if company object is not in the comoany table, then return a message in Json format
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            // remove the company object from the Companies list
            _unitOfWork.Company.Remove(obj);
            // saving/removing the company object from the database
            _unitOfWork.Save();
            // return the data in Json format
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
