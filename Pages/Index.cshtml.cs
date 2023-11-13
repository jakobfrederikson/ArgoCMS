using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArgoCMS.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Redirect("/Dashboard");
        }
    }
}
