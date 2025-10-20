using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.Store
{
    public class ManageStoreModel : PageModel
    {
        private readonly IStoreService _storeService;

        public ManageStoreModel(
            IStoreService storeService
        )
        {
            _storeService = storeService;
        }

        public List<StoreModel> Stores { get; set; } = new List<StoreModel>();

        public void OnGet()
        {
            Stores = _storeService.GetAll().ToList();
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
