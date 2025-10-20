using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.Product
{
    public class ManageProductModel : PageModel
    {
        private readonly IProductService _productService;

        public ManageProductModel(
            IProductService productService
        )
        {
            _productService = productService;
        }

        public List<ProductModel> Products { get; set; }
      
        [BindProperty]
        public string NewProductName { get; set; }

        [BindProperty]
        public string ProductDescription { get; set; }

        public void OnGet()
        {
            Products = _productService.GetAll().ToList();
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
    }
}
