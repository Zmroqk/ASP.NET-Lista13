using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lista13.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [MaxLength(100)]
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
