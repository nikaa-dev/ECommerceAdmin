namespace src.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }
        public ValidationException(IEnumerable<string> errors) : base("One or more validation failures have occurred.")
        {
            Errors = errors.ToList();
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }
    }
}