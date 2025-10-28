namespace Algebra.WebShop2025.App.Areas.Admin.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? FileName { get; set; }

    public string? FileContentBase64 { get; set; }
}
