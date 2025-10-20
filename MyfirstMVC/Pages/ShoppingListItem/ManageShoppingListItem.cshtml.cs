using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.ShoppingListItem
{
    public class ManageShoppingListItemModel : PageModel
    {
        private readonly IShoppingListItemService _shoppingListItemService;

        public ManageShoppingListItemModel(
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

        public IActionResult OnPostEditButton(int id)
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteButton(int id)
        {
            return RedirectToPage();

        }
    }
}
