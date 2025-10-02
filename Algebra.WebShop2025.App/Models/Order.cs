using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Algebra.WebShop2025.App.Models;

public class Order
{
    [Key]   
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    [ForeignKey("OrderId")]
    public virtual ICollection<OrderItem> Items { get; set; }
}
