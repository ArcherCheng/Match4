﻿using System;
using System.Collections.Generic;

namespace MatchApi.Data
{
    public partial class MemberDetail
    {
        public int UserId { get; set; }
        public string Introduction { get; set; }
        public string LikeCondition { get; set; }
        public DateTime? WriteTime { get; set; }
        public string WriteUser { get; set; }
        public string WriteIp { get; set; }

        public virtual Member User { get; set; }
    }
}
