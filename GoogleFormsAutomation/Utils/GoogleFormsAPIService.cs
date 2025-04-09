using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using GoogleFormsAutomation.App.Exceptions;

namespace GoogleFormsAutomation.App.Utils
{
    public class GoogleFormsAPIService
    {
        public UserCredential userCredentials;

        public GoogleFormsAPIService() { }

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
    }
}
