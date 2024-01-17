using BE_2911.Model.Account;
using DataAccess.Demo.UnitOfWork;
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
    public class AccountController : ControllerBase
    {
        private IMyShopUnitOfWork _myShopUnitOfWork;
        private IConfiguration _configuration;
        public AccountController(IMyShopUnitOfWork myShopUnitOfWork, IConfiguration configuration)
        {
            _myShopUnitOfWork = myShopUnitOfWork;
            _configuration = configuration;
        }

        [HttpPost("AccountLogin")]
        public async Task<ActionResult> AccountLogin(AccountLoginRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                // Bước 1 : Login 
                if (requestData == null ||
                    string.IsNullOrEmpty(requestData.UserName)
                    || string.IsNullOrEmpty(requestData.Password))
                {
                    return BadRequest();
                }


                var userLogin = await _myShopUnitOfWork.accountRepository.User_Login(requestData);

                if (userLogin == null || userLogin.UserID <= 0)
                {
                    returnData.ResponseCode = -1;
                    returnData.ResponseMessage = "Đăng nhập thất bại !";
                    return Ok(returnData);
                }



                // Bước 2 Tạo token 
                var authClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userLogin.UserName),
                    new Claim(ClaimTypes.PrimarySid,userLogin.UserID.ToString()),
                  new Claim(ClaimTypes.GivenName,userLogin.FullName.ToString()),};

                var newAccessToken = CreateToken(authClaims);

                var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                var refreshToken = GenerateRefreshToken();

                // //Bước 3 update refreshToken vào db

                var expired = _configuration["JWT:RefreshTokenValidityInDays"] ?? "";


                var result_update = _myShopUnitOfWork.accountRepository.AccountUpdateRefeshToken(new AccountUpdateRefeshTokenRequestData
                {
                    UserID = userLogin.UserID,
                    RefeshToken = refreshToken,
                    RefeshTokenExpired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                });

                returnData.ResponseCode = 1;
                returnData.ResponseMessage = "Đăng nhập thành công !";
                returnData.token = token;
                returnData.refeshToken = refreshToken;
                return Ok(returnData);
            }
            catch (Exception ex)
            {

                returnData.ResponseCode = -1;
                returnData.ResponseMessage = ex.Message;
                return Ok(returnData);
            }
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
