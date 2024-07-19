using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QueryArchive.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QueryArchive.Web.Pages.Answers
{
    public class IndexModel : PageModel
    {
        public List<Answer> Answers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public int QuestionId { get; set; }

        public async Task OnGetAsync(int questionId, string searchString, int pageNumber = 1)
        {
            QuestionId = questionId;
            CurrentPage = pageNumber;
            SearchString = searchString;
            int pageSize = 10;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/answers?questionId={questionId}&searchString={searchString}&pageNumber={pageNumber}&pageSize={pageSize}");
                var result = JsonConvert.DeserializeObject<PagedResult<Answer>>(response);
                Answers = result.Items;
                TotalPages = result.TotalPages;
            }
        }
    }
}
