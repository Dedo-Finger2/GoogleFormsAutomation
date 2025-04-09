using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;
using GoogleFormsAutomation.App.DTOs;

namespace GoogleFormsAutomation.App
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            UserCredential credential;
            using (FileStream stream = new("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(stream).Secrets, new[] { FormsService.Scope.FormsBody }, "user", CancellationToken.None);
            }

            FormsService service = new FormsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FormCreationAPp"
            });

            Form form = new Form
            {
                Info = new Info
                {
                    Title = dto.Title,
                    DocumentTitle = dto.Title
                }
            };

            Form? createdForm = await service.Forms.Create(form).ExecuteAsync();
            Console.WriteLine($"Formulário criado! Link de edição: https://docs.google.com/forms/d/{createdForm.FormId}/edit");

            BatchUpdateFormRequest request = new BatchUpdateFormRequest
            {
                Requests = [],
            };

            request.Requests.Add(new Request
            {
                UpdateFormInfo = new UpdateFormInfoRequest
                {
                    Info = new Info
                    {
                        Description = dto.Description
                    },
                    UpdateMask = "description"
                }
            });

            request.Requests.Add(new Request
            {
                UpdateSettings = new UpdateSettingsRequest
                {
                    Settings = new FormSettings
                    {
                        QuizSettings = new QuizSettings
                        {
                            IsQuiz = true
                        }
                    },
                    UpdateMask = "quizSettings.isQuiz"
                }
            });

            for (int i = 0; i < dto.Questions.Length; i++)
            {
                List<Option> options = new();
                foreach (string option in dto.Questions[i].Options)
                {
                    options.Add(new Option { Value = option });
                }

                List<CorrectAnswer> correctAnswers = new();
                foreach (string correctAnswer in dto.Questions[i].CorrectAnswers)
                {
                    correctAnswers.Add(new CorrectAnswer { Value = correctAnswer });
                }

                Request requestitem = new Request
                {
                    CreateItem = new CreateItemRequest
                    {
                        Item = new Item
                        {
                            Title = dto.Questions[i].Title,
                            QuestionItem = new QuestionItem
                            {
                                Question = new Question
                                {
                                    Required = true,
                                    ChoiceQuestion = new ChoiceQuestion
                                    {
                                        Type = dto.Questions[i].QuestionType.ToString(),
                                        Options = options,
                                        Shuffle = true
                                    },
                                    Grading = new Grading
                                    {
                                        PointValue = dto.Questions[i].Points,
                                        CorrectAnswers = new CorrectAnswers
                                        {
                                            Answers = correctAnswers,
                                        },
                                        WhenRight = new Feedback
                                        {
                                            Text = dto.Questions[i].TextWhenWrong
                                        },
                                        WhenWrong = new Feedback
                                        {
                                            Text = dto.Questions[i].TextWhenWrong
                                        }
                                    }
                                }
                            }
                        },
                        Location = new Location
                        {
                            Index = i
                        }
                    }
                };

                request.Requests.Add(requestitem);
            }

            await service.Forms.BatchUpdate(request, createdForm.FormId).ExecuteAsync();
            Console.WriteLine("Perguntas adicionadas!");
        }
    }
}