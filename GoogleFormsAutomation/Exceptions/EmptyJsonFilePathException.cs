namespace GoogleFormsAutomation.App.Exceptions
{
    public class EmptyJsonFilePathException : Exception
    {
        public EmptyJsonFilePathException() : base("json file path cannot be empty") { }

        public EmptyJsonFilePathException(string message) : base(message) { }

        public EmptyJsonFilePathException(string message, Exception inner) : base(message, inner) { }
    }
}
