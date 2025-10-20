using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.ShoppingListItem
{
    public class ListShoppingListItemModel : PageModel
    {
        private readonly IShoppingListItemService _shoppingListItemService;

        public ListShoppingListItemModel(
            IShoppingListItemService shoppingListItemService
        )
        {
            _shoppingListItemService = shoppingListItemService;
        }

        public List<ShoppingListItemModel> ShoppingListItems { get; set; } = new List<ShoppingListItemModel>();

        public void OnGet()
        {
            ShoppingListItems = _shoppingListItemService.GetAll().ToList();
        }
    }
}
