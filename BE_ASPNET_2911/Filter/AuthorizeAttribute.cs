using DataAccess.Demo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;

namespace BE_ASPNET_2911.Filter
{
    public class AuthorizeAttribute : TypeFilterAttribute

    {
        public AuthorizeAttribute() : base(typeof(DemoAuthorizeActionFilter))
        {
        }
    }

    public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var user = new User
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    UserID = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value)
                };

                if (user.UserID <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Vui lòng đăng nhập để thực hiện chức năng này "
                    });

                    return;
                }

            }
        }
    }

}
