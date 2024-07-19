using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryArchive.Api.Models;

namespace QueryArchive.Web.Pages.Topics
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Topic Topic { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(Topic), Encoding.UTF8, "application/json");
                await httpClient.PostAsync("https://localhost:5001/api/topics", content);
            }

            return RedirectToPage("./Index");
        }
    }
}
