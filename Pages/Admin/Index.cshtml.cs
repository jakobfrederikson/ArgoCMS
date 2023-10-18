using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArgoCMS.Pages.Admin
{
    [Authorize(Roles = "Administrators")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
