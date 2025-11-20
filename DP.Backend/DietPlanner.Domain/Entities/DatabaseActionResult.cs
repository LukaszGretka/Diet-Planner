using System;

namespace DietPlanner.Domain.Entities
{
    public class DatabaseActionResult<T> where T : class
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public T Obj { get; private set; }

        public Exception Exception { get; private set; }

        public DatabaseActionResult(bool success, string message = "", T obj = null, Exception exception = null)
        {
            Success = success;
            Message = message;
            Obj = obj;
            Exception = exception;
        }
    }

    public class DatabaseActionResult
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public DatabaseActionResult(bool success, string message = "", Exception exception = null)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }
    }
}
