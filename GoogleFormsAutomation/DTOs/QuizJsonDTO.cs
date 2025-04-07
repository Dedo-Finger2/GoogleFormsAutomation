using System.Text.Json.Serialization;

namespace GoogleFormsAutomation.App.DTOs
{
    public class QuizJsonDTO
    {
        [JsonPropertyName("quizTitle")]
        public string Title { get; set; }
        [JsonPropertyName("quizDescription")]
        public string? Description { get; set; }
    }
}
