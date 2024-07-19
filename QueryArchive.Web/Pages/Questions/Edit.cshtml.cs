using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using QueryArchive.Api.Models;

namespace QueryArchive.Web.Pages.Questions
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Question Question { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"https://localhost:5001/api/questions/{id}");
                Question = JsonConvert.DeserializeObject<Question>(response);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (File != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", File.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] array = new byte[stream.Length];
                    await stream.ReadAsync(array, 0, array.Length);
                    Question.Attachment = Convert.ToBase64String(array);
                }
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(Question), Encoding.UTF8, "application/json");
                await httpClient.PutAsync($"https://localhost:5001/api/questions/{Question.QuestionID}", content);
            }

            return RedirectToPage("./Index", new { topicId = Question.TopicID });
        }
    }
}
