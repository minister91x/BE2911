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
        public AuthorizeAttribute(string functionCode, string permission) : base(typeof(DemoAuthorizeActionFilter))
        {
            Arguments = new object[] { functionCode, permission };
        }
    }

    public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _functionCode;
        private readonly string _permission;

        private IConfiguration _configuration;
        private IMyShopUnitOfWork _myShopUnitOfWork;
        public DemoAuthorizeActionFilter(string functionCode, string permission, IConfiguration configuration, IMyShopUnitOfWork myShopUnitOfWork)
        {
            _configuration = configuration;
            _myShopUnitOfWork = myShopUnitOfWork;
            _functionCode = functionCode;
            _permission = permission;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            //var headerAuthorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            //var accessToken = headerAuthorization.FirstOrDefault()?.Split(' ')[1] ?? "";


            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                // Bước 1 : Lấy thông tin User từ Token
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
                //Lấy functionId dựa vào _functionCode
                var function = await _myShopUnitOfWork.accountRepository.GetFunction(_functionCode);
                if (function == null || function.FunctionID <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Bạn không"
                    });

                    return;
                }

                // Bước 3: Lấy userFunction dựa vào userid , functionId , Permisstion
                var userFunction = await _myShopUnitOfWork.accountRepository.GetUserFunction(user.UserID, function.FunctionID, _permission);

               
                if (userFunction == null || userFunction.FunctionID <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Bạn không có quyền"
                    });

                    return;
                }

                // Bước 4: Check kết quả
                switch (_permission)
                {
                    case "ISVIEW":
                        if (userFunction.IsView == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền xem danh sách "
                            });

                            return;
                        }
                        break;
                    case "ISUPDATE":
                        if (userFunction.IsUpdate == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền cập nhật "
                            });

                            return;
                        }
                        break;

                    
                }


            }
        }


    }

}
