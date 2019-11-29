using System;
using System.Collections.Generic;

namespace MatchApi.Data
{
    public partial class AppDataLog
    {
        public long Id { get; set; }
        public string TableName { get; set; }
        public string InsertData { get; set; }
        public string DeleteData { get; set; }
        public byte WriteType { get; set; }
        public DateTime? WriteTime { get; set; }
        public string WriteUser { get; set; }
        public string WriteIp { get; set; }
    }
}
