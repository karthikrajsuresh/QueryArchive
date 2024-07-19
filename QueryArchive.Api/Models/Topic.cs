using System.ComponentModel.DataAnnotations;

namespace QueryArchive.Api.Models
{
    // Models/Topic.cs
    public class Topic
    {
        public int TopicID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int QuestionsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation property for related questions
        public List<Question> Questions { get; set; } = new List<Question>();
    }    
}
