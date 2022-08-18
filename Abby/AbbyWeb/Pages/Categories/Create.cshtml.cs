using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AbbyWeb.Model;
using AbbyWeb.Data;


namespace AbbyWeb.Pages.Categories
{

    [BindProperties]

    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public Category Category { get; set; }


        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
                
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Category.Name", "The value entered in DisplayOrder cannot exactly match the value in Name");
            }

            if(ModelState.IsValid)
            {
                await _db.Category.AddAsync(Category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            // return back to the same page
            return Page();
        }
    }
}
