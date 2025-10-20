using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.Product
{
    public class ListProductModel : PageModel
    {
        private readonly IProductService _productService;

        public ListProductModel(
            IProductService productService         
        )
        {
            _productService = productService;
        }

        public List<ProductModel> Products { get; set; }

        public void OnGet()
        {
            Products = _productService.GetAll().ToList();      
        }
    }
}
