using System.ComponentModel.DataAnnotations;

namespace CrudWithDotnet.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = null!;
        
        [Range(0.01, 3000)]
        public decimal Price { get; set; }
    }
}