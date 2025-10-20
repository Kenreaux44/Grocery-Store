using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryStoreData.Models;

public partial class ShoppingListItem
{
    [Column("ShoppingListItems")]
    public int ShoppingListItemId { get; set; }

    public int ShoppingListId { get; set; }

    public int StoreProductId { get; set; }

    public decimal? Quantity { get; set; }
    public virtual ShoppingList ShoppingList { get; set; } = null!;
    public virtual StoreProduct StoreProduct { get; set; } = null!;

}
