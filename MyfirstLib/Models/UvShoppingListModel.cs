namespace MyfirstLib.Models
{
    public class UvShoppingListModel
    {
        public string Store { get; set; } = null!;

        public string StoreAddress1 { get; set; } = null!;

        public string? StoreAddress2 { get; set; }

        public string StoreCity { get; set; } = null!;

        public string StoreState { get; set; } = null!;

        public string ListTitle { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Product { get; set; } = null!;

        public decimal? Quantity { get; set; }

        public string UnitOfMeasure { get; set; } = null!;
    }
}
