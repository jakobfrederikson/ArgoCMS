using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArgoCMS.Pages.api.Dashboard
{
    public class GetBackgroundColoursModel : PageModel
    {
        private const string BackgroundColourOpacity = "0.9";
        private readonly string[] Colours = new string[]
        {
            $"rgba(199, 255, 69, {BackgroundColourOpacity})",
            $"rgba(134, 255, 69, {BackgroundColourOpacity})",
            $"rgba(69, 255, 122, {BackgroundColourOpacity})",
            $"rgba(69, 255, 227, {BackgroundColourOpacity})",
            $"rgba(69, 171, 255, {BackgroundColourOpacity})",
            $"rgba(69, 75, 255, {BackgroundColourOpacity})",
            $"rgba(150, 69, 255, {BackgroundColourOpacity})",
            $"rgba(230, 69, 255, {BackgroundColourOpacity})"
        };

        public List<string> ListOfColours { get; set; } = new List<string>();

        public IActionResult OnGet(int count)
        {
            ListOfColours = InitializeBackgroundColours(count);

            return new JsonResult(ListOfColours);
        }

        private List<string> InitializeBackgroundColours(int count)
        {
            List<string> listOfColours = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string colour = Colours[i % Colours.Length];
                listOfColours.Add(colour);
            }

            return listOfColours;
        }
    }
}
