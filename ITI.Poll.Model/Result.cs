using System;

namespace ITI.Poll.Model
{
    public class Result
    {
        internal Result(string error, string errorMessage)
        {
            Error = error;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccess => Error == string.Empty;

        public string Error { get; }

        public string ErrorMessage { get; }

        public static Result CreateSuccess() => new Result(string.Empty, string.Empty);

        public static Result<T> CreateSuccess<T>(T value) => new Result<T>(value);

        public static Result CreateError(string error, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("The error must be not null nor white space.", nameof(error));
            if (string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("The error message must be not null nor white space.", nameof(errorMessage));

            return new Result(error, errorMessage);
        }

        public static Result<T> CreateError<T>(string error, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("The error must be not null nor white space.", nameof(error));
            if (string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("The error message must be not null nor white space.", nameof(errorMessage));

            return new Result<T>(error, errorMessage);
        }

        public static Result<T> Map<T>(Result result)
        {
            return Result.CreateError<T>(result.Error, result.ErrorMessage);
        }
    }

    public sealed class Result<T> : Result
    {
        readonly T _value;
        
        internal Result(string error, string errorMessage)
            : base(error, errorMessage)
        {
        }

        internal Result(T value)
            : base(string.Empty, string.Empty)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (!IsSuccess) throw new InvalidOperationException("This result has error.");
                return _value;
            }
        }
    }
}