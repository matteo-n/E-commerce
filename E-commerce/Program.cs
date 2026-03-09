using System;
using System.Collections.Generic;
using System.Linq;
using E_commerce.Models;
using E_commerce.Services;
using E_commerce.Providers;

namespace E_commerce_Order
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new Catalog();
            catalog.AddSampleProducts();

            var cart = new Cart();
            var payments = new List<IPaymentProvider>
            {
                new DummyPaymentProvider(),
                new CreditCardPaymentProvider(),
                new PayPalPaymentProvider(),
                new BankTransferPaymentProvider()
            };
            var shippers = new List<IShippingProvider>
            {
                new DummyShippingProvider(),
                new FastCourierProvider(),
                new EconomyCourierProvider(),
                new PickupPointProvider()
            };
            var discounts = new List<IDiscountStrategy>
            {
                new NoDiscount(),
                new PercentageDiscount(10),
                new PercentageDiscount(20),
                new FixedAmountDiscount(15)
            };

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Lista produse");
                Console.WriteLine("2. Adauga în cos");
                Console.WriteLine("3. Vezi cos");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Sortează produse");
                Console.WriteLine("0. Iesire");
                Console.Write("Alege: ");
                var key = Console.ReadLine();
                if (key == "0") break;
                switch (key)
                {
                    case "1":
                        for (int i = 0; i < catalog.Products.Count; i++)
                        {
                            var p = catalog.Products[i];
                            Console.WriteLine($"{i + 1}. {p.Name} [{p.Category}] - {p.Price:C}");
                        }
                        break;
                    case "2":
                        Console.Write("Id produs: ");
                        if (int.TryParse(Console.ReadLine(), out var id))
                        {
                            var prod = catalog.Products.FirstOrDefault(p => p.Id == id);
                            if (prod != null)
                            {
                                cart.Add(prod);
                                Console.WriteLine("Adaugat.");
                            }
                            else Console.WriteLine("Produs nu exista.");
                        }
                        break;
                    case "3":
                        Console.WriteLine($"Cos: {cart.Items.Count} produse, total {cart.Total():C}");
                        foreach (var it in cart.Items)
                            Console.WriteLine($" - {it.Name} [{it.Category}] {it.Price:C}");
                        break;
                    case "4":
                        if (!cart.Items.Any())
                        {
                            Console.WriteLine("Cosul este gol.");
                            break;
                        }
                        Console.WriteLine("Selectati metoda de plata:");
                        for (int i = 0; i < payments.Count; i++)
                            Console.WriteLine($"{i + 1}. {payments[i].Name}");
                        if (!int.TryParse(Console.ReadLine(), out var psel) || psel < 1 || psel > payments.Count)
                        {
                            Console.WriteLine("Invalid");
                            break;
                        }
                        Console.WriteLine("Selectati curier:");
                        for (int i = 0; i < shippers.Count; i++)
                            Console.WriteLine($"{i + 1}. {shippers[i].Name}");
                        if (!int.TryParse(Console.ReadLine(), out var ssel) || ssel < 1 || ssel > shippers.Count)
                        {
                            Console.WriteLine("Invalid");
                            break;
                        }

                        var order = new Order { Items = cart.Items.ToList(), Total = cart.Total() };

                        Console.WriteLine("Selectati o reducere:");
                        for (int i = 0; i < discounts.Count; i++)
                            Console.WriteLine($"{i + 1}. {discounts[i].Name}");
                        if (!int.TryParse(Console.ReadLine(), out var dsel) || dsel < 1 || dsel > discounts.Count)
                        {
                            Console.WriteLine("Invalid discount");
                            break;
                        }
                        var discount = discounts[dsel - 1];
                        var discountAmount = discount.CalculateDiscount(order);
                        var payable = order.Total - discountAmount;
                        Console.WriteLine($"Reducere aplicata: {discountAmount:C}. Total de plata: {payable:C}");

                        var paymentResult = payments[psel - 1].Pay(payable);
                        var shipResult = shippers[ssel - 1].Ship(order);
                        Console.WriteLine($"Plata: {paymentResult}, Livrare: {shipResult}");
                        cart.Clear();
                        break;
                    case "5":
                        Console.Write("Criteriu (price/name/category): ");
                        var crit = Console.ReadLine();
                        Console.Write("Ascending? (y/n): ");
                        var ascInput = Console.ReadLine();
                        var asc = ascInput?.ToLower() == "y";
                        var sorted = SortingService.Sort(catalog.Products, crit, asc);
                        Console.WriteLine("Produse sortate:");
                        var sortedList = sorted.ToList();
                        for (int i = 0; i < sortedList.Count; i++)
                        {
                            var p = sortedList[i];
                            Console.WriteLine($"{i + 1}. {p.Name} [{p.Category}] - {p.Price:C}");
                        }
                        break;
                }
            }
        }
    }
}
