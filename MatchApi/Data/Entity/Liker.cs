using System;
using System.Collections.Generic;

namespace MatchApi.Data
{
    public partial class Liker
    {
        public int SendId { get; set; }
        public int LikerId { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? WriteTime { get; set; }
        public string WriteUser { get; set; }
        public string WriteIp { get; set; }

        public virtual Member LikerNavigation { get; set; }
        public virtual Member Send { get; set; }
    }
}
