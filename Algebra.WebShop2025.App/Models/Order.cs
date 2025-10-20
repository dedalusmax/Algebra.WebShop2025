using System.ComponentModel;
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

    [Required]
    public DateTime DateTimeCreated { get; set; }

    [Required(ErrorMessage = "Customer's first name is required.")]
    [StringLength(50)]
    [DisplayName("Customer's first name")]
    public string CustomerFirstName { get; set; }

    [Required(ErrorMessage = "Customer's last name is required.")]
    [StringLength(50)]
    [DisplayName("Customer's last name")]
    public string CustomerLastName { get; set; }

    [Required(ErrorMessage = "Customer's email is required.")]
    [StringLength(50), EmailAddress]
    [DisplayName("Customer's email address")]
    public string CustomerEmailAddress { get; set; }

    [Required(ErrorMessage = "Customer's phone number is required.")]
    [StringLength(50)] // Phone, Regex
    [DisplayName("Customer's phone number")]
    public string CustomerPhoneNumber { get; set; }

    [Required(ErrorMessage = "Customer's address is required.")]
    [StringLength(250)]
    [DisplayName("Customer's address")]
    public string CustomerAddress { get; set; }
}
