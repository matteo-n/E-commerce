// Plan (pseudocode):
// 1. The compile error indicates two different `Order` types exist: `E_commerce.Models.Order` and another `Order` type.
// 2. Ensure the `IDiscountStrategy.CalculateDiscount` signature uses the same `Order` type that `Program.cs` constructs.
// 3. Import the models namespace (`using E_commerce.Models;`) so `Order` resolves to `E_commerce.Models.Order`.
// 4. Update the interface signature to accept that `Order` type.
// 5. This change keeps the parameter type explicit and resolves the CS1503 conversion error.
// 6. If implementations of `IDiscountStrategy` are in the project, they will compile against this signature (adjust them if required).

using E_commerce.Models;

namespace E_commerce.Services
{
    public interface IDiscountStrategy
    {
        string Name { get; }
        decimal CalculateDiscount(Order order);
    }
}