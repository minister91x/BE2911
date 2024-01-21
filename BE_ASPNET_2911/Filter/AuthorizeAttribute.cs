using DataAccess.Demo.Entities;
using DataAccess.Demo.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

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
        private IConfiguration _configuration;
        private IMyShopUnitOfWork _myShopUnitOfWork;
        public DemoAuthorizeActionFilter(IConfiguration configuration, IMyShopUnitOfWork myShopUnitOfWork)
        {
            _configuration = configuration;
            _myShopUnitOfWork = myShopUnitOfWork;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            var headerAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            var accessToken = headerAuthorization.FirstOrDefault()?.Split(' ')[1] ?? "";

          


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

                //                var principal = GetPrincipalFromExpiredToken(accessToken);
                //                if (principal == null)
                //                {
                //                    //  return BadRequest("Invalid access token or refresh token");
                //                }

                //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                //#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //                string username = principal.Identity.Name;
                //#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                //                var user_db =  _myShopUnitOfWork.accountRepository.GetUsers().Result.Where(s=>s.UserName== user.UserName).FirstOrDefault();

                //                if (user_db == null || user_db.RefeshToken != refreshToken || user.RefeshTokenExpired  <= DateTime.Now)
                //                {
                //                    return BadRequest("Invalid access token or refresh token");
                //                }

                //var newAccessToken = CreateToken(principal.Claims.ToList());
                //var newRefreshToken = GenerateRefreshToken();

                //user.RefreshToken = newRefreshToken;
                //await _userManager.UpdateAsync(user);

                //return new ObjectResult(new
                //{
                //    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                //    refreshToken = newRefreshToken
                //});

            }
        }

      
    }

}
