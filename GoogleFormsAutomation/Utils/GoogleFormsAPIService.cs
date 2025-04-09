using System.Formats.Asn1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;
using GoogleFormsAutomation.App.DTOs;
using GoogleFormsAutomation.App.Exceptions;

namespace GoogleFormsAutomation.App.Utils
{
    public class GoogleFormsAPIService
    {
        private UserCredential userCredentials;
        private FormsService formService;

        public async void RegisterCredentials(string credentialsJsonFilePath)
        {
            var emptyCredentials = credentialsJsonFilePath == string.Empty;
            var credentialsFileDoesNotExists = !File.Exists(credentialsJsonFilePath);

            if (emptyCredentials) throw new Exception("user credentials cannot be empty");
            if (credentialsFileDoesNotExists) throw new ResourceNotFoundException($"{credentialsJsonFilePath} was not found");

            using (var stream = new FileStream(credentialsJsonFilePath, FileMode.Open, FileAccess.Read))
            {
                this.userCredentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(stream).Secrets, new[] { FormsService.Scope.FormsBody }, "user", CancellationToken.None);
            }
        }

        public void SetupFormService(string appName = "FormCreationApp")
        {
            this.formService = new FormsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = this.userCredentials,
                ApplicationName = appName
            });
        }

        private async Task<Form?> CreateForm(QuizJsonDTO dto)
        {
            Form form = new Form
            {
                Info = new Info
                {
                    Title = dto.Title,
                    DocumentTitle = dto.Title
                }
            };

            return await this.formService.Forms.Create(form).ExecuteAsync();
        }

        private BatchUpdateFormRequest CreateInitialRequest()
        {
            var request = new BatchUpdateFormRequest
            {
                Requests = [],
            };

            return request;
        }

        private void AddFormDescription(BatchUpdateFormRequest requestCollection, QuizJsonDTO dto)
        {
            requestCollection.Requests.Add(new Request
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
        }

        private void SetFormAsQuiz(BatchUpdateFormRequest requestCollection)
        {
            requestCollection.Requests.Add(new Request
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
        }

        private void AddFormQuestions(QuizJsonDTO dto, BatchUpdateFormRequest requestCollection)
        {
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

                requestCollection.Requests.Add(requestitem);
            }
        }

        public async Task<string> CreateFormFromJson(QuizJsonDTO dto)
        {
            var form = await this.CreateForm(dto);

            var requestCollection = this.CreateInitialRequest();

            this.AddFormDescription(requestCollection, dto);

            this.SetFormAsQuiz(requestCollection);

            this.AddFormQuestions(dto, requestCollection);

            await this.formService.Forms.BatchUpdate(requestCollection, form.FormId).ExecuteAsync();

            return $"https://docs.google.com/forms/d/{form.FormId}/edit";
        }
    }
}
