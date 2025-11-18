using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.ShoppingListItem
{
    public class ManageShoppingListItemModel : PageModel
    {
        private readonly IShoppingListItemService _shoppingListItemService;
        private readonly IShoppingListService _shoppingListService;

        public ManageShoppingListItemModel(
            IShoppingListItemService shoppingListItemService,
            IShoppingListService shoppingListService
        )
        {
            _shoppingListItemService = shoppingListItemService;
            _shoppingListService = shoppingListService;
        }

        public List<ShoppingListItemModel> ShoppingListItems { get; set; } = new List<ShoppingListItemModel>();
        public SelectList ShoppingLists { get; private set; }

        [BindProperty]
        public int ShoppingListId { get; set; }

        [BindProperty]
        public string NewShoppingListQuantity { get; set; }

        public void OnGet()
        {
            ShoppingListItems = _shoppingListItemService.GetAll().ToList();
            GetShoppingLists();
        }

        public IActionResult OnPostEditButton(int id)
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteButton(int id)
        {
            return RedirectToPage();

        }
        public IActionResult OnPostCreate(int id)
        {

            return RedirectToPage();
        }

        private void GetShoppingLists()
        {
            var shoppingLists = _shoppingListService.GetAll()
                .ToDictionary(sl => sl.ShoppingListId, sl => sl.Title);
            ShoppingLists = new SelectList(shoppingLists, "Key", "Value");
        }
    }
}
