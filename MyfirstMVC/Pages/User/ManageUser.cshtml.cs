using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;


namespace MyfirstMVC.Pages.User
{
    public class ManageUserModel : PageModel
    {
        private readonly IUserService _userService;
        public ManageUserModel(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public List<UserModel> Users { get; set; } = new List<UserModel>();

        public void OnGet()
        {            
            Users = _userService.GetAll().ToList();
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
