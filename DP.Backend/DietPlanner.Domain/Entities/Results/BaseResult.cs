namespace DietPlanner.Domain.Entities.Results
{
    public class BaseResult
    {
        public bool Succeeded { get; init; } = true;

        public string? ErrorCode { get; init; } = null;

        public static BaseResult Success => new();

        public static BaseResult Failed(string errorCode) => new () { Succeeded = false, ErrorCode = errorCode };
    }
}
