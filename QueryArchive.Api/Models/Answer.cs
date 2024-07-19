using System.ComponentModel.DataAnnotations;

namespace QueryArchive.Api.Models
{
    // Models/Answer.cs
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        [Required]
        public string Content { get; set; }
        public string Attachment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation property for related question
        public Question Question { get; set; }
    }
}
