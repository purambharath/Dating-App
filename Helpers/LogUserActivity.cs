using System;
using System.Threading.Tasks;
using Extentions;
using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // throw new System.NotImplementedException();

            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return ;

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            var user = await repo.GetUserByIdAsync(userId);

            user.LastActive = DateTime.Now;


            await repo.SaveAllAsync();



        }
    }
}