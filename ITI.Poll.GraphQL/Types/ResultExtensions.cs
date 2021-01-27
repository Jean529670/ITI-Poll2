using System;
using ITI.Poll.GraphQL.Types;

namespace ITI.Poll.Model
{
    public static class ResultExtensions
    {
        public static TResult ToGraphQL<T1, T2, TResult>(
            this Result<T1> @this,
            Func<TResult> newResult,
            Action<TResult, Error[]> setError,
            Func<T1, T2> map,
            Action<TResult, T2> setSuccess)
        {
            TResult result = newResult();
            if (!@this.IsSuccess)
            {
                setError(result, new Error[]
                {
                    new Error { Type = @this.Error, Message = @this.ErrorMessage }
                });
                return result;
            }

            setError(result, new Error[0]);
            T2 mapped = map(@this.Value);
            setSuccess(result, mapped);
            return result;
        }

        public static TResult ToGraphQL<TResult>(
            this Result @this,
            Func<TResult> newResult,
            Action<TResult, Error[]> setError)
        {
            TResult result = newResult();
            if (!@this.IsSuccess)
            {
                setError(result, new Error[]
                {
                    new Error { Type = @this.Error, Message = @this.ErrorMessage }
                });
                return result;
            }

            setError(result, new Error[0]);
            return result;
        }
    }
}