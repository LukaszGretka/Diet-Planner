namespace DietPlanner.Shared.Models
{
    public class DatabaseActionResult<T> where T : class
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public T? Obj { get; private set; }

        public Exception? Exception { get; private set; }

        public DatabaseActionResult(bool success, string message = "", T? obj = null, Exception? exception = null)
        {
            Success = success;
            Message = message;
            Obj = obj;
            Exception = exception;
        }
    }
}
