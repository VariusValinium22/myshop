namespace MyShop.MyHelpers
{
    public static class OrderCalculator
    {
        // Pure business logic: no DB, no HTTP, no UI.
        public static decimal CalculateTotal(decimal price, int quantity)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

            return price * quantity;
        }
    }
}

// NOTE: This class is a 'helper' file that contains pure business logic.
// It was introduced as an initial, minimal example to validate that the xUnit
// unit-testing is initially setup correctly.
// It also serves as a reference for extracting and testing business logic
// independently of Razor Pages, HTTP, or database concerns.
