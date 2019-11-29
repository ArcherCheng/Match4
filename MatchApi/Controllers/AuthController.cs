using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatchApi.Helper;
using MatchApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MatchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public AuthController(IAuthService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this._service = service;
            this._config = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            if (await _service.UserIsExists(model.Email, model.Phone))
                return BadRequest("此電子郵件或電話已經是會員了");

            var member = await _service.Register(model, model.password);
            return Ok(member);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var userLoginData = await _service.Login(model.Username, model.Password);
            if (userLoginData == null)
                return Unauthorized();

            //var saltKey = _config.GetSection("AppSettings:Token");
            var tokenLoginData = Security.UserLoginToken(userLoginData);

            //返回簡單使用者資料
            return Ok(new
            {
                //tokenToReturn = tokenHandler.WriteToken(token),
                tokenLoginData,
                userLoginData
            });
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var newPassword = await _service.NewPassword(model);
            if (newPassword.Length == 0)
                return BadRequest("取回密碼失敗");

            return Ok(newPassword);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            var isSuccess = await _service.ChangePassword(model);

            if (!isSuccess)
                BadRequest("原密碼輸入錯誤,請重新輸入");

            return Ok();
        }
    }
}