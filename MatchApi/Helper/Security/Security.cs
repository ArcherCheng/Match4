using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MatchApi.Helper
{
    public class Security
    {
        private static IHttpContextAccessor HttpContextAccessor { get; set; }

        //static Security()
        //{
        //    this.HttpContextAccessor = new Microsoft.AspNetCore.Http.HttpContextAccessor();
        //}

        public static bool CheckCurrentUser(int userId, HttpContext httpContext)
        {
            var claimUserId = int.Parse(httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            if (claimUserId == userId)
                return true;

            return false;

            return true;
        }


        public static string UserLoginToken(UserLoginData user)
        {
            var tokenSaltKey = AppSettingsHelper.Configuration["AppSettings:Token"];
            //var tokenSaltKey = AppSettingsHelper.Configuration.GetSection("AppTokens").GetSection("Token").Value;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSaltKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.UserRoles ?? "users" )
            };

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(30),
                SigningCredentials = credentials
            };

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripter);
            var tokenResult = tokenHandler.WriteToken(token);
            return tokenResult;
        }
    }
}
