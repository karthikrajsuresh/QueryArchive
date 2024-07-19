using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QueryArchive.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QueryArchive.Web.Pages.Questions
{
    public class IndexModel : PageModel
    {
        public List<Question> Questions { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public int TopicId { get; set; }

        public async Task OnGetAsync(int topicId, string searchString, int pageNumber = 1)
        {
            TopicId = topicId;
            CurrentPage = pageNumber;
            SearchString = searchString;
            int pageSize = 10;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/questions/bytopic/{topicId}?searchString={searchString}&pageNumber={pageNumber}&pageSize={pageSize}");
                var result = JsonConvert.DeserializeObject<PagedResult<Question>>(response);
                Questions = result.Items;
                TotalPages = result.TotalPages;
            }
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
