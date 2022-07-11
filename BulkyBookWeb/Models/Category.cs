using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        [DisplayName("Display Order")]  // Display Name. from System.ComponentModel.DataAnnotations
        [Range(1,50,ErrorMessage = "Number of items displayed must be between 1 and 50")] // Range validation
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
