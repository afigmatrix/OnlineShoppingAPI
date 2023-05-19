using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShoppingAPI.ActionFIlters
{
    public class CustomActionBaseActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
