using AgileObjects.AgileMapper;
using MatchApi.Data;
using MatchApi.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public class AuthService : BaseService, IAuthService
    {
        public async Task<bool> ChangePassword(ChangePasswordDto model)
        {
            using (var db = base.NewDb())
            {
                var user = await db.Member.FirstOrDefaultAsync(x=>x.Email == model.Email && x.Phone == model.Phone);
                if (user.Email != model.Email || user.Phone != model.Phone)
                    return false;

                //先判定原密碼是否正確
                if (!PasswordHash.VerifyPasswordHash(model.OldPassword, user.PasswordHash, user.PasswordSalt))
                    return false;

                byte[] passwordHash, passwordSalt;
                PasswordHash.CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.WriteTime = System.DateTime.Now;
                db.Member.Update(user);
                var saveNumber = await db.SaveChangesAsync();
                return saveNumber > 0;
            }
        }

        //public async Task<Member> GetUser(string email, string phone)
        //{
        //    using (var db = base.NewDb())
        //    {
        //        var result = await db.Member
        //            .FirstOrDefaultAsync(x => x.Email == email && x.Phone == phone);
        //        return result;
        //    }
        //}

        public async Task<UserLoginData> Login(string userEmail, string password)
        {
            using (var db = base.NewDb())
            {
                var user = await db.Member
                    .FirstOrDefaultAsync(x => x.Email == userEmail || x.Phone == userEmail);
                if (user == null)
                    return null;

                if (!PasswordHash.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                user.LoginDate = System.DateTime.Now;
                db.Member.Update(user);
                var saveNumber = await db.SaveChangesAsync();
                var dto = Mapper.Map(user).ToANew<UserLoginData>(cfg=>cfg
                    .Map((source,target) => source.NickName)
                    .To(target => target.UserName));
                return dto;
            }
        }

        public async Task<string> NewPassword(ForgetPasswordDto model)
        {
            using (var db = base.NewDb())
            {
                var user = await db.Member.FirstOrDefaultAsync(x => x.Email == model.Email && x.Phone == model.Phone);
                if (user.Email != model.Email || user.Phone != model.Phone)
                    return "";

                var newPass = new System.Random();
                var newPassword = newPass.Next(111111, 999999).ToString();

                byte[] passwordHash, passwordSalt;
                PasswordHash.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.WriteTime = System.DateTime.Now;
                db.Member.Update(user);
                var saveNumber = await db.SaveChangesAsync();

                return newPassword;
            }
        }

        public async Task<UserLoginData> Register(RegisterDto model, string password)
        {
            var user = Mapper.Map(model).ToANew<Member>();

            byte[] passwordHash, passwordSalt;
            PasswordHash.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LoginDate = System.DateTime.Now;
            user.UserRoles = "users";
            using (var db = base.NewDb())
            {
                db.Member.Add(user);
                var saveNumber = await db.SaveChangesAsync();

                var userLoginData =  Mapper.Map(user).ToANew<UserLoginData>(cfg => cfg
                    .Map((source, target) => source.NickName)
                    .To(target => target.UserName));

                return userLoginData;
            }
        }

        public async Task<bool> UserIsExists(string userEmail, string userPhone)
        {
            using (var db = base.NewDb())
            {
                var user = await db.Member
                    .FirstOrDefaultAsync(p => p.Email == userEmail || p.Phone == userPhone);
                if (user == null)
                    return false;
                return true;
            }
        }
    }
}
