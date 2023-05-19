using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShoppingAPI.ActionFIlters
{
    public class CustomGlobalActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //context.HttpContext.Response.Cookies.Delete("IsLoginned");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var cookieValue = context.HttpContext.Request.Cookies.TryGetValue("IsLoginned",out string isLogined);
            //if (!cookieValue)
            //{
            //    context.HttpContext.Response.Cookies.Append("IsLoginned", "Afiq");
            //}
        }
    }
}
