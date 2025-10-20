namespace GroceryStoreData.Models;

public partial class State
{
    public int StateId { get; set; }

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
