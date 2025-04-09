namespace GoogleFormsAutomation.App.Exceptions
{
    public class JsonFileMalformedException : Exception
    {
        public JsonFileMalformedException() { }
        public JsonFileMalformedException(string message): base(message) { }
        public JsonFileMalformedException(string message, Exception inner) : base(message, inner) { }
    }
}
