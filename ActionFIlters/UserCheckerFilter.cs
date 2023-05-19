using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShoppingAPI.Constants;
using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Service.Abstractions;
using System;
using System.Linq;

namespace OnlineShoppingAPI.ActionFIlters
{
    public class UserCheckerFilter : IActionFilter
    {
        public UserCheckerFilter(IGenericRepository<Log> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public IGenericRepository<Log> _genericRepository { get; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.HttpContext.Request.Path.ToString().Contains("Login") || 
                !context.HttpContext.Request.Path.ToString().Contains("Logout"))
            {
                var userToken = context.HttpContext.Request.Cookies.TryGetValue(".AspNetCore.Identity.Application", out string token);
                if (!string.IsNullOrEmpty(token))
                {
                    var userGuid = context.HttpContext.Request.Cookies.TryGetValue(ConstantValue.UserGuid,out string userGuidCookie);

                    if (!userGuid)
                    {
                        context.Result = new BadRequestResult();
                        return;
                    }
                    var guidInfo = _genericRepository.GetValuesByExpression(m => m.UserGuid == userGuidCookie).FirstOrDefault();
                    if (guidInfo is null)
                    {
                        context.Result = new BadRequestResult();
                        return;
                    }
                    if (guidInfo.ExpireDate <= DateTime.Now)
                    {
                        context.Result = new BadRequestResult();
                        return;
                    }
                }
            }
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
