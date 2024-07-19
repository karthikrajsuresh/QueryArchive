using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryArchive.Api.Models;

namespace QueryArchive.Web.Pages.Topics
{
    public class DetailsModel : PageModel
    {
        public Topic Topic { get; set; }

        public async Task OnGetAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/topics/{id}");
                Topic = JsonConvert.DeserializeObject<Topic>(response);
            }
        }
    }
}
