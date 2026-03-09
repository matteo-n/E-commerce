using E_commerce.Models;

namespace E_commerce.Providers
{
    // Plăți
    public interface IPaymentProvider { string Name { get; } string Pay(decimal amount); }

    public class DummyPaymentProvider : IPaymentProvider
    {
        public string Name => "Plata demo";
        public string Pay(decimal amount) => $"Platit {amount:C}";
    }

    public class CreditCardPaymentProvider : IPaymentProvider
    {
        public string Name => "Card (Visa/Mastercard)";
        public string Pay(decimal amount) => $"Card debitat cu {amount:C}";
    }

    public class PayPalPaymentProvider : IPaymentProvider
    {
        public string Name => "PayPal";
        public string Pay(decimal amount) => $"Plata prin PayPal: {amount:C} procesata";
    }

    public class BankTransferPaymentProvider : IPaymentProvider
    {
        public string Name => "Transfer bancar";
        public string Pay(decimal amount) => $"Transfer bancar initiat pentru {amount:C}";
    }

    // Livrare / curierat
    public interface IShippingProvider { string Name { get; } string Ship(Order order); }

    public class DummyShippingProvider : IShippingProvider
    {
        public string Name => "Curier demo";
        public string Ship(Order order) => $"Expediat {order.Items.Count} produse";
    }

    public class FastCourierProvider : IShippingProvider
    {
        public string Name => "Curier Rapid";
        public string Ship(Order order) => $"Expediat rapid ({order.Items.Count} produse)";
    }

    public class EconomyCourierProvider : IShippingProvider
    {
        public string Name => "Curier Economic";
        public string Ship(Order order) => $"Expediere economica pentru {order.Items.Count} produse";
    }

    public class PickupPointProvider : IShippingProvider
    {
        public string Name => "Ridicare din magazin";
        public string Ship(Order order) => $"Comanda pregatita pentru ridicare ({order.Items.Count} produse)";
    }
}
