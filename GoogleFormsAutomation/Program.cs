﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                await Program.Start();

            } catch (Exception e)
            {
                MyLogger.Error("something went wrong", e);
            }
        }

        public async static Task Start()
        {
            string? jsonFilePath;

            do
            {
                Console.Clear();

                Program.ShowMainMenu();

                Console.Write("File Path: ");

                jsonFilePath = Console.ReadLine();
            } while (jsonFilePath == null || jsonFilePath == string.Empty);

            var dto = MyJsonService.GetConvertedJsonToDTO(jsonFilePath);

            var googleApiFormService = new GoogleFormsAPIService();

            googleApiFormService.RegisterCredentials("credentials.json");

            googleApiFormService.SetupFormService();

            var formURL = await googleApiFormService.CreateFormFromJson(dto);

            Console.Clear();

            Console.WriteLine($"form created! URL: {formURL}");
            Console.WriteLine("Press ESC to leave or ENTER to create a new form.");

            ConsoleKey[] possiblekeys = { System.ConsoleKey.Escape, System.ConsoleKey.Enter };
            ConsoleKeyInfo userPressedKey = Console.ReadKey();

            while (!possiblekeys.Contains(userPressedKey.Key))
            {
                userPressedKey = Console.ReadKey();
            }

            if (userPressedKey.Key == System.ConsoleKey.Escape) ;

            if (userPressedKey.Key == System.ConsoleKey.Enter) await Program.Start();

        }

        public static void ShowMainMenu()
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat<string>("=", 30)));
            Console.WriteLine("Google Forms Automation");
            Console.WriteLine(string.Concat(Enumerable.Repeat<string>("=", 30)));
        }
    }
}