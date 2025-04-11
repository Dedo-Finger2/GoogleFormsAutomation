# 📝 Google Forms AutoJson

<img src="https://github.com/Dedo-Finger2/GoogleFormsAutomation/blob/master/GoogleFormsAutomation/Images/cover.png?raw=true" />

## 📌 Overview

This project automates the creation of Google Forms quizzes using a structured JSON file. It's designed to help users quickly generate quizzes to enhance their studies and save time.

## 🛠️ Tech Stack

- **Language:** C#
- **Framework:** .NET Core

## 📂 JSON Structure

To create a quiz, prepare a JSON file with the following structure:

```json
{
  "quizTitle": "Your Quiz Title",
  "quizDescription": "Description of the quiz.",
  "questions": [
    {
      "title": "Question text?",
      "pointValue": 1,
      "correctAnswersValue": ["Correct Answer"],
      "whenRight": "Feedback for correct answer.",
      "whenWrong": "Feedback for incorrect answer.",
      "questionType": "RADIO",
      "options": [
        "Option 1",
        "Option 2",
        "Option 3",
        "Option 4",
        "Option 5"
      ]
    }
    // Add more questions as needed
  ]
}
```

*Note:* It's recommended to use an AI tool to generate this JSON structure for consistency and accuracy.

## 🤖 AI prompt example

```plaintext
Generate [NUMBER OF QUESTIONS] multiple choice questions with 5 options each and only ONE correct answer. The questions must:

Cover all topics contained in the sources you receive.
Be of an advanced level, with tricks, two or more very similar answers, requiring real knowledge and attention. Be formatted in the following JSON format, with all fields complete:

{
  "quizTitle": "Your Quiz Title",
  "quizDescription": "Description of the quiz.",
  "questions": [
    {
      "title": "Question text?",
      "pointValue": 1,
      "correctAnswersValue": ["Correct Answer"],
      "whenRight": "Feedback for correct answer.",
      "whenWrong": "Feedback for incorrect answer.",
      "questionType": "RADIO",
      "options": [
        "Option 1",
        "Option 2",
        "Option 3",
        "Option 4",
        "Option 5"
      ]
    }
    // Add more questions as needed
  ]
}

🔴 ATTENTION:

- Keep the exact format of the JSON presented above.
- Use appropriate technical language.
- Do not repeat questions or alternatives.
- Make sure that the correct answer is among the options, and that only one is true.
- Add logical or syntax tricks to make it more difficult.
- Make sure that the correct answer is formatted the same as the alternative. The text format must be identical.
```

## 🔐 Setting Up Google Cloud Credentials

To interact with the Google Forms API, you'll need to set up a Google Cloud project and obtain a `credentials.json` file.

### 1. Create a Google Cloud Project

- Navigate to the [Google Cloud Console](https://console.cloud.google.com/).
- Click on the project dropdown and select "New Project".
- Enter a project name and click "Create".

### 2. Enable the Google Forms API

- In the [APIs & Services Dashboard](https://console.cloud.google.com/apis/dashboard), click "Enable APIs and Services".
- Search for "Google Forms API" and enable it.

### 3. Create a Service Account

- Go to [Service Accounts](https://console.cloud.google.com/iam-admin/serviceaccounts).
- Click "Create Service Account".
- Provide a name and description, then click "Create".
- Assign the "Editor" role or a role with sufficient permissions.
- Click "Done".

### 4. Generate the `credentials.json` File

- In the Service Accounts page, click on the newly created service account.
- Navigate to the "Keys" tab.
- Click "Add Key" > "Create New Key".
- Choose "JSON" as the key type and click "Create".
- A `credentials.json` file will be downloaded to your computer.

## 🔨 Build

```sh
git clone a

cd GoogleFormsAutomation

dotnet build

cd GoogleFormsAutomation/bin/Debug/net9.0
./GoogleFormsAutomation.App.exe
```

## 🐞 Report problems

The system generates a `.log` file in your Desktop when an error happens. Make sure to read it or let me know if you find out any bugs.

## 🚀 Usage

Once you have your `credentials.json` and your quiz JSON file:

1. Place both files in your project directory (same as the binary)
2. Run the application, providing the path to your quiz JSON file.
3. The application will create a Google Form based on your JSON structure.

## 📜 License

This project is licensed under the MIT License.
