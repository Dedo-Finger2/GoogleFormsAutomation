using System.Text;

namespace GoogleFormsAutomation.App.Utils
{
    public static class MyLogger
    {        
        public static void Error(string message, Exception e)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "google-forms-automation.log");

                Console.Clear();

                Console.WriteLine($"unespected error. please check the log file at '{path}' for more information");

                string content = $"[{DateTime.Now}]\n[TYPE]: ERROR\n[MESSAGE]: {message}\n[ERROR MESSAGE]: {e.Message}\n[STACK TRACE]: {e.StackTrace}\n\n";

                File.AppendAllText(path, content);
            } 
            catch(Exception ex)
            {
                Console.WriteLine($"error trying to write log: {ex}");
            }
        }

        public static void Warning(string message, Exception e)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "google-forms-automation.log");

                string content = $"[{DateTime.Now}]\n[TYPE]: WARNING\n[MESSAGE]: {message}\n[ERROR MESSAGE]: {e.Message}\n[STACK TRACE]: {e.StackTrace}\n\n";

                File.AppendAllText(path, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error trying to write log: {ex}");
            }
        }
    }
}
