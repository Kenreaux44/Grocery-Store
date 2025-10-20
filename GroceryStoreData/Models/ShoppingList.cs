namespace GroceryStoreData.Models;

public partial class ShoppingList
{
    public int ShoppingListId { get; set; }

    public int UserId { get; set; }

    public int StoreId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? LastUpdateDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}
