using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public class UserLoginData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRoles { get; set; }
        public string MainPhotoUrl { get; set; }
    }
}
