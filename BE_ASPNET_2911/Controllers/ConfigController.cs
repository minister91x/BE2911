using BE_2911.Model.Account;
using DataAccess.Demo.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BE_ASPNET_2911.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private IConfiguration _configuration;
        private IMyShopUnitOfWork _myShopUnitOfWork;
        public ConfigController(IConfiguration configuration, IMyShopUnitOfWork myShopUnitOfWork)
        {
            _configuration = configuration;
            _myShopUnitOfWork = myShopUnitOfWork;
        }

        [HttpPost("GetConfig")]
        public async Task<ActionResult> GetConfig()
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1 : Lấy token từ Request
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                if (accessToken == null)
                {
                    return BadRequest();
                }

                var jwtSecurityToken = new JwtSecurityToken(accessToken);
                var ValidTo = jwtSecurityToken.ValidTo.AddHours(7); // UTC+7 

                if (ValidTo <= DateTime.Now)
                {

                    // Bước 2 : giải mãi token dựa vào SecretKey đã config trước đó
                    var principal = GetPrincipalFromExpiredToken(accessToken);
                    if (principal == null)
                    {
                        return BadRequest(" access token or refresh token không hợp lệ");
                    }


                    // bước 3: Lấy userName từ token ra 
                    string username = principal?.Claims.ToList()[0]?.ToString()?.Split(' ')[1];

                    // Bước 4: kiểm tra xem RefeshToken hết hạn chưa 
                    var user_db = _myShopUnitOfWork.accountRepository.GetUsers().Result.Where(s => s.UserName == username).FirstOrDefault();

                    if (user_db == null || user_db.RefeshTokenExpired <= DateTime.Now)
                    {
                        return BadRequest("refresh token hết hạn.Vui lòng đăng nhập lại ");
                    }

                    // KHỞI TẠO TOKEN KHÁC


                    var authClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user_db.UserName),
                    new Claim(ClaimTypes.PrimarySid,user_db.UserID.ToString()),
                  new Claim(ClaimTypes.GivenName,user_db.FullName.ToString()),};

                    var newAccessToken = CreateToken(authClaims);

                    var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                    var refreshToken = GenerateRefreshToken();

                    // //Bước 3 update refreshToken vào db

                    var expired = _configuration["JWT:RefreshTokenValidityInDays"] ?? "";


                    var result_update = _myShopUnitOfWork.accountRepository.AccountUpdateRefeshToken(new AccountUpdateRefeshTokenRequestData
                    {
                        UserID = user_db.UserID,
                        RefeshToken = refreshToken,
                        RefeshTokenExpired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                    });

                    returnData.ResponseCode = 1;
                    returnData.ResponseMessage = "Đăng nhập thành công !";
                    returnData.token = token;
                    returnData.refeshToken = refreshToken;
                    return Ok(returnData);
                }

                return Ok(accessToken);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
