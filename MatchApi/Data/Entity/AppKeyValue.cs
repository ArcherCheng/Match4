using System;
using System.Collections.Generic;

namespace MatchApi.Data
{
    public partial class AppKeyValue
    {
        public int Id { get; set; }
        public string AppGroup { get; set; }
        public string AppKey { get; set; }
        public string AppValue { get; set; }
        public int AppSort { get; set; }
        public DateTime? WriteTime { get; set; }
        public string WriteUser { get; set; }
        public string WriteIp { get; set; }
    }
}
