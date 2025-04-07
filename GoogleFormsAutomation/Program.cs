using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleFormsAutomation.App.DTOs;

namespace GoogleFormsAutomation.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? jsonFilePath;

            do
            {
                Console.Write("Path: ");
                jsonFilePath = Console.ReadLine();
            } while (jsonFilePath == null || jsonFilePath == string.Empty);

            QuizJsonDTO? dto = new();

            using (StreamReader r = new(jsonFilePath))
            {
                string jsonData = r.ReadToEnd();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                };

                dto = JsonSerializer.Deserialize<QuizJsonDTO>(jsonData, options);
            }

            if (dto == null) return;

            Console.WriteLine(dto.Questions[0].Title);
        }
    }
}