using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.StoreProduct
{
    public class ManageStoreProductModel : PageModel
    {
        private readonly IStoreProductService _storeProductService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;

        public List<StoreProductModel> StoreProducts { get; private set; } = new List<StoreProductModel>();
        public SelectList Stores { get; private set; }
        public SelectList Products { get; private set; }

        [BindProperty]
        public int StoreId { get; set; }
        [BindProperty]
        public int ProductId { get; set; }

        public ManageStoreProductModel(
            IStoreProductService storeProductService,
            IStoreService storeService,
            IProductService productService
        )
        {
            _storeProductService = storeProductService;
            _storeService = storeService;
            _productService = productService;
        }

        public void OnGet()
        {
            StoreProducts = _storeProductService.GetAll().ToList();
            GetStoreList();
            GetProductList();
        }
        public IActionResult OnPostCreate(int id)
        {

            return RedirectToPage();
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
            var stores = _storeService.GetAll()?
                .ToDictionary(store => store.StoreId, store => store.Name);
            Stores = new SelectList(stores, "Key", "Value");
        }

        private void GetProductList()
        {
            var products = _productService.GetAll()
                .ToDictionary(product => product.ProductId, product => product.Name);
            Products = new SelectList(products, "Key", "Value");
        }

        #endregion 
    }
}
