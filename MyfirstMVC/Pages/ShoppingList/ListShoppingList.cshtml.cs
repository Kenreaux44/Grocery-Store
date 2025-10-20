using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.ShoppingList
{
    public class ListShoppingListModel : PageModel
    {
        private readonly IShoppingListService _shoppingListService;

        public ListShoppingListModel(
            IShoppingListService shoppingListService
        )
        {
            _shoppingListService = shoppingListService;
        }

        public List<ShoppingListModel> ShoppingLists { get; set; } = new List<ShoppingListModel>();

        public void OnGet()
        {
            ShoppingLists = _shoppingListService.GetAll().ToList();
        }
    }
}
