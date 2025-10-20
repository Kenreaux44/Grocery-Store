namespace GroceryStoreData.Models;

public partial class StoreProduct
{
    public int StoreProductId { get; set; }

    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}
