using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.StoreProduct
{
    public class ManageStoreProductModel : PageModel
    {
        private readonly IStoreProductService _storeProductService;

        public ManageStoreProductModel(
            IStoreProductService storeProductService
        )
        {
            _storeProductService = storeProductService;
        }

        public List<StoreProductModel> StoreProducts { get; set; } = new List<StoreProductModel>();

        public void OnGet()
        {
            StoreProducts = _storeProductService.GetAll().ToList();
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
