using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using QueryArchive.Api.Models;

namespace QueryArchive.Web.Pages.Answers
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Answer Answer { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public void OnGet(int questionId)
        {
            Answer = new Answer { QuestionID = questionId };
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
                    Answer.Attachment = Convert.ToBase64String(array);
                }
            }

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(Answer), Encoding.UTF8, "application/json");
                await httpClient.PostAsync("https://localhost:5001/api/answers", content);
            }

            return RedirectToPage("./Index", new { questionId = Answer.QuestionID });
        }
    }
}
