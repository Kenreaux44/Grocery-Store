using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages.User
{
    public class ListUserModel : PageModel
    {
        private readonly IUserService _userService;
        public ListUserModel(
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
    }
}
