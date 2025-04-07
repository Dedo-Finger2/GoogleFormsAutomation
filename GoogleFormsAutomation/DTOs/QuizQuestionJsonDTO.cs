using System.Text.Json.Serialization;
using GoogleFormsAutomation.App.Enums;

namespace GoogleFormsAutomation.App.DTOs
{
    public class QuizQuestionJsonDTO
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("pointValue")]
        public int Points { get; set; }
        [JsonPropertyName("correctAnswersValue")]
        public string[] CorrectAnswers { get; set; }
        [JsonPropertyName("whenRight")]
        public string TextWhenRight { get; set; }
        [JsonPropertyName("whenWrong")]
        public string TextWhenWrong { get; set; }
        [JsonPropertyName("questionType")]
        public QuestionType QuestionType { get; set; }
        [JsonPropertyName("options")]
        public string[] Options { get; set; }
    }
}
