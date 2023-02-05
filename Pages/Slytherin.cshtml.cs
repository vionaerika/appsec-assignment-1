using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    [Authorize(Roles = "Slytherin")]
    public class Slytherin : PageModel
    {
        public void OnGet()
        {
        }
    }
}
