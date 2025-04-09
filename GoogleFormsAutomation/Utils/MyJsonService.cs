using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleFormsAutomation.App.DTOs;
using GoogleFormsAutomation.App.Exceptions;

namespace GoogleFormsAutomation.App.Utils
{
    public static class MyJsonService
    {
        public static QuizJsonDTO GetConvertedJsonToDTO(string filePath)
        {
            var jsonFileDoesNotExists = !File.Exists(filePath);
            var jsonFilePathIsEmpty = filePath.Trim() == string.Empty;

            if (jsonFilePathIsEmpty) throw new EmptyJsonFilePathException();
            if (jsonFileDoesNotExists) throw new ResourceNotFoundException($"json file not found at {filePath}");
            
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

            if (dto == null) throw new JsonFileMalformedException();

            return dto;
        }
    }
}
