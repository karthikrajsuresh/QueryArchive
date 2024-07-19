using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryArchive.Api.Models;

namespace QueryArchive.Web.Pages.Topics
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Topic Topic { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/topics/{id}");
                Topic = JsonConvert.DeserializeObject<Topic>(response);
            }

            return Page();
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
                await httpClient.PutAsync($"https://localhost:5001/api/topics/{Topic.TopicID}", content);
            }

            return RedirectToPage("./Index");
        }
    }
}
