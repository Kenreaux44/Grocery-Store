namespace MyfirstLib.Models
{
    public class ShoppingListModel
    {
        public int ShoppingListId { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int StoreId { get; set; }
        public string Store { get; set; }

        public string Title { get; set; } = null!;
    }
}
