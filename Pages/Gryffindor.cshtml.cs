using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    [Authorize(Roles = "Gryffindor")]
    public class Gryffindor : PageModel
    {
        public void OnGet()
        {
        }
    }
}
