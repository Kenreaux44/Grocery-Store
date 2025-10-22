using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.Store
{
    public class ManageStoreModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly IStateService _stateService;

        public ManageStoreModel(
            IStoreService storeService, 
            IStateService stateService
        )
        {
            _storeService = storeService;
            _stateService = stateService;
        }

        public List<StoreModel> Stores { get; set; } = new List<StoreModel>();

        public SelectList States { get; set; }

        [BindProperty]
        public string NewStoreName { get; set; }

        [BindProperty]
        public string NewStoreAddress1 { get; set; }

        [BindProperty]
        public string NewStoreAddress2 { get; set; }

        [BindProperty]
        public string NewStoreCity { get; set; }

        [BindProperty]
        public string NewStoreStateId { get; set; }

        [BindProperty]
        public string NewStoreZipCode { get; set; }


        public void OnGet()
        {
            Stores = _storeService.GetAll().ToList();
            var states = _stateService.GetAll()
                .ToDictionary(x => x.StateId, x => x.Name);
            States = new SelectList(states, "Key", "Value");
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
