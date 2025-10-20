using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.StoreProduct
{
    public class ListStoreProductModel : PageModel
    {
        private readonly IStoreProductService _storeProductService;

        public ListStoreProductModel(
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
    }
}
