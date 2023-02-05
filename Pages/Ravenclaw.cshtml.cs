using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    [Authorize(Roles = "Ravenclaw")]
    public class Ravenclaw : PageModel
    {
        public void OnGet()
        {
        }
    }
}
