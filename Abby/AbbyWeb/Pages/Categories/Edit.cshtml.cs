using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AbbyWeb.Model;
using AbbyWeb.Data;
using System.Linq;

namespace AbbyWeb.Pages.Categories
{

    [BindProperties]

    public class EditModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public Category Category { get; set; }


        public EditModel(ApplicationDbContext db)
        {
            _db = db;
                
        }


        public void OnGet(int id)
        {
            // Find(...) works on primary key of the table
            Category = _db.Category.Find(id);

            //// First(u=>u.Id==id) find the first record that matches the id; otherwise throws an exception
            //Category = _db.Category.First(u=>u.Id==id);
            //// FirstOrDefault(u=>u.Id==id) returns the first record that matches id; otherwise returns null 
            //Category = _db.Category.FirstOrDefault(u=>u.Id==id);
            //// When you expect one entity to be return; throws an exception when more than one entity is returned
            //Category = _db.Category.Single(u => u.Id == id);
            //// When you expect one entity to be return; otherwise returns null
            //Category = _db.Category.SingleOrDefault(u => u.Id == id);
            //// Where returns all the record that matched the id, then FirstOrDefault() returns the first one or return null otherwise
            //Category = _db.Category.Where(u => u.Id == id).FirstOrDefault();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Category.Name", "The value entered in DisplayOrder cannot exactly match the value in Name");
            }

            if(ModelState.IsValid)
            {
                _db.Category.Update(Category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            // return back to the same page
            return Page();
        }
    }
}
