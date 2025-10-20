using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.ShoppingList
{
    public class ManageShoppingListModel : PageModel
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;
        public List<ShoppingListModel> ShoppingLists { get; private set; } = new List<ShoppingListModel>();
        public SelectList Stores { get; private set; }
        public SelectList Users { get; private set; }


        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public int StoreId { get; set; }

        [BindProperty]
        public int UserId { get; set; }

        public ManageShoppingListModel(
            IShoppingListService shoppingListService,
            IStoreService storeService,
            IUserService userService
        )
        {
            _shoppingListService = shoppingListService;
            _storeService = storeService;
            _userService = userService;
        }
        public void OnGet()
        {
            ShoppingLists = _shoppingListService.GetAll().ToList();
            GetStoreList();
            GetUserList();
        }
        public IActionResult OnPostEditButton(int id)
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteButton(int id)
        {
            return RedirectToPage();
        }

        #region Support

        private void GetStoreList()
        {
            var stores = _storeService.GetAll()
                .ToDictionary(store => store.StoreId, store => store.Name);
            Stores = new SelectList(stores, "Key", "Value");
        }
        private void GetUserList()
        {
            var users = _userService.GetAll()
                .ToDictionary(user => user.UserId, user => user.Email);
            Users = new SelectList(users, "Key", "Value");
        }

        #endregion
    }
}
