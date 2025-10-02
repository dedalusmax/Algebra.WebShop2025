using System.ComponentModel.DataAnnotations;

namespace Algebra.WebShop2025.App.Models;

public class ProductCategory
{
    [Key]
    public int Id { get; set; }

    [Required]  
    public int ProductId { get; set; }

    public virtual required Product Product { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public virtual required Category Category { get; set; }
}
