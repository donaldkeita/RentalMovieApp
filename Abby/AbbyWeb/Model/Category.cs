using System.ComponentModel.DataAnnotations;

namespace AbbyWeb.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Display Order")]
        [Range(1,50,ErrorMessage="Order quantity must be between 1 and 50.")]
        public int DisplayOrder { get; set; }
    }
}
