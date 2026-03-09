using System;
using System.Collections.Generic;
using System.Linq;
using E_commerce.Models;

namespace E_commerce.Services
{
    public class Catalog
    {
        public List<Product> Products { get; } = new();

        public void AddSampleProducts()
        {
            var items = new (string Name, string Category, decimal Price)[]
            {
                ("Tricou", "Imbracaminte", 29.99m),
                ("Pantaloni", "Imbracaminte", 59.50m),
                ("Adidasi", "Incaltaminte", 120.00m),
                ("Sapca", "Accesorii", 15.00m),
                ("Pantofi", "Imbracaminte", 45.50m),
                ("Compleu_treining", "Imbracaminte", 89.99m),
                ("Camasa", "Imbracaminte", 39.99m),
                ("Curea", "Accesorii", 35.00m),
                ("Rochie", "Imbracaminte", 79.99m),
                ("Fusta", "Imbracaminte", 49.99m),
                ("Sandale", "Incaltaminte", 60.00m),
                ("Geanta", "Accesorii", 150.00m),
                ("Bluza", "Imbracaminte", 34.99m),
                ("Hanorac", "Imbracaminte", 69.99m),
                ("Ghete", "Incaltaminte", 110.00m),
                ("Portofel", "Accesorii", 25.00m),
                ("Pulover", "Imbracaminte", 44.99m),
                ("Vesta", "Imbracaminte", 54.99m),
                ("Cizme", "Incaltaminte", 130.00m),
                ("Ochelari de soare", "Accesorii", 80.00m)
            };

            var id = 1;
            foreach (var it in items)
            {
                Products.Add(new Product(id++, it.Name, it.Category, it.Price));
            }

        }
    }

    public class Cart
    {
        public List<Product> Items { get; } = new();
        public void Add(Product p) => Items.Add(p);
        public decimal Total() => Items.Sum(i => i.Price);
        public void Clear() => Items.Clear();
    }

    public static class SortingService
    {
        public static IEnumerable<Product> Sort(IEnumerable<Product> products, string key, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(key))
                return products;
            switch (key.Trim().ToLower())
            {
                case "price":
                    return ascending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                case "name":
                    return ascending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                case "category":
                    return ascending ? products.OrderBy(p => p.Category) : products.OrderByDescending(p => p.Category);
                default:
                    return products;
            }
        }
    }

    // Discount strategies

    public class NoDiscount : IDiscountStrategy
    {
        public string Name => "Fara reducere";
        public decimal CalculateDiscount(Order order) => 0m;
    }

    public class PercentageDiscount : IDiscountStrategy
    {
        public string Name => $"Reducere procentuala ({Percent}%)";
        public decimal Percent { get; }
        public PercentageDiscount(decimal percent)
        {
            Percent = percent;
        }
        public decimal CalculateDiscount(Order order) => Math.Round(order.Total * (Percent / 100m), 2);
    }

    public class FixedAmountDiscount : IDiscountStrategy
    {
        public string Name => $"Reducere fixa ({Amount:C})";
        public decimal Amount { get; }
        public FixedAmountDiscount(decimal amount)
        {
            Amount = amount;
        }
        public decimal CalculateDiscount(Order order) => Amount <= order.Total ? Amount : order.Total;
    }
}
