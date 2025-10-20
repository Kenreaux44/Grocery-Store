namespace MyfirstLib.Models
{
    public class ShoppingListItemModel
    {
        public int ShoppingListItemId { get; set; }

        public int ShoppingListId { get; set; }
        public string ShoppingList { get; set; }

        public int StoreProductId { get; set; }

        public string Product { get; set; }

        public decimal? Quantity { get; set; }

        public string UnitOfMeasure { get; set; }
    }
}
