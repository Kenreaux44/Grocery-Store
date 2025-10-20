using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.Store
{
    public class ListStoreModel : PageModel
    {
        private readonly IStoreService _storeService;

        public ListStoreModel(
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
    }
}
