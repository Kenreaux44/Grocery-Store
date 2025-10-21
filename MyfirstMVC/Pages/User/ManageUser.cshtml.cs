using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;


namespace MyfirstMVC.Pages.User
{
    public class ManageUserModel : PageModel
    {
        private readonly IUserService _userService;

        public List<UserModel> Users { get; set; } = new List<UserModel>();

        [BindProperty]
        public string NewUserEmail { get; set; }

        [BindProperty]
        public string NewUserFirstName { get; set; }
       
        [BindProperty]
        public string NewUserLastName { get; set; }

        public ManageUserModel(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public void OnGet()
        {            
            Users = _userService.GetAll().ToList();
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
