using System;
using System.Collections.Generic;

namespace MatchApi.Data
{
    public partial class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public string MainPhotoUrl { get; set; }
        public bool? IsInWork { get; set; }
        public DateTime? LoginDate { get; set; }
        public string UserRole { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime? WriteTime { get; set; }
        public string WriteUser { get; set; }
        public string WriteIp { get; set; }
    }
}
