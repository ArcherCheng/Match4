using MatchApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public interface IAuthService : IBaseService
    {
        Task<UserLoginData> Register(RegisterDto model, string password);
        Task<UserLoginData> Login(string userEmail, string password);
        Task<bool> UserIsExists(string userEmail, string userPhone);
        //Task<Member> GetMember(string email, string phone);
        Task<string> NewPassword(ForgetPasswordDto model);
        Task<bool> ChangePassword(ChangePasswordDto model);
        //string UserLoginToken(AppUser user, string tokenSecretKey);
    }
}
