using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyfirstLib.Models;

namespace MyfirstMVC.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var model = new Tools()
            {
                Message = "Something else.",
                Date = DateTime.Now
            };
            model.Message = "Anything I want!";
        }
    }
}
