using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QueryArchive.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QueryArchive.Web.Pages.Topics
{
    public class IndexModel : PageModel
    {
        public List<Topic> Topics { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }

        public async Task OnGetAsync(string searchString, int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            SearchString = searchString;
            int pageSize = 10;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/topics?searchString={searchString}&pageNumber={pageNumber}&pageSize={pageSize}");
                var result = JsonConvert.DeserializeObject<PagedResult<Topic>>(response);
                Topics = result.Items;
                TotalPages = result.TotalPages;
            }
        }
    }
}
