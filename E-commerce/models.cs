using System.Collections.Generic;

namespace E_commerce.Models
{
    public record Product(int Id, string Name, string Category, decimal Price);

    public class Order
    {
        public List<Product> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
