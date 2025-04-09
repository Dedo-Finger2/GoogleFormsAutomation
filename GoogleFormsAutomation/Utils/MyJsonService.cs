using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleFormsAutomation.App.DTOs;

namespace GoogleFormsAutomation.App.Utils
{
    public static class MyJsonService
    {
        public static QuizJsonDTO? GetConvertedJsonToDTO(string filePath)
        {
            var jsonFileExists = File.Exists(filePath);
            var jsonFilePathIsEmpty = filePath.Trim() == string.Empty;

            if (!jsonFileExists || jsonFilePathIsEmpty) throw new Exception("json file was not found");

            QuizJsonDTO? dto = new();

            using (var r = new StreamReader(filePath))
            {
                var content = r.ReadToEnd();

                var options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                };

                dto = JsonSerializer.Deserialize<QuizJsonDTO>(content, options);
            }

            if (dto == null) throw new Exception("error trying to parse json content into dto");

            return dto;
        }
    }
}
