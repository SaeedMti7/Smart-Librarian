using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Librarian.Config.Extensions
{
    public static class ResultHubExtensions
    {
        public static ObjectResult ToObjectResult<TSource>(this ResultHub<TSource> source,
            Action<ResultHub<TSource>> IsSuccessAction = null!)
        {
            if (IsSuccessAction != null && source.IsSuccess)
            {
                IsSuccessAction(source);
            }
            return new ObjectResult(source)
            {
                StatusCode = source.HttpStatusCode
            };
        }
    }
}
