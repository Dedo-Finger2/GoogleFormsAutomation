using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;
using GoogleFormsAutomation.App.DTOs;
using GoogleFormsAutomation.App.Utils;

namespace GoogleFormsAutomation.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                string? jsonFilePath;

                do
                {
                    Console.Write("Path: ");
                    jsonFilePath = Console.ReadLine();
                } while (jsonFilePath == null || jsonFilePath == string.Empty);

                var dto = MyJsonService.GetConvertedJsonToDTO(jsonFilePath);

                var googleApiFormService = new GoogleFormsAPIService();

                googleApiFormService.RegisterCredentials("credentials.json");

                googleApiFormService.SetupFormService();
                
                var formURL = await googleApiFormService.CreateFormFromJson(dto);

                Console.Clear();
                
                Console.WriteLine($"form created! URL: {formURL}");
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}