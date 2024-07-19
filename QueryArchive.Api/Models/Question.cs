using System.ComponentModel.DataAnnotations;

namespace QueryArchive.Api.Models
{
    // Models/Question.cs
    public class Question
    {
        public int QuestionID { get; set; }
        public int TopicID { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Attachment { get; set; }
        public int AnswersCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation property for related topic
        public Topic Topic { get; set; }

        // Navigation property for related answers
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

}
