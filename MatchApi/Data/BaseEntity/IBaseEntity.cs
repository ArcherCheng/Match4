using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Data
{
    public interface IBaseEntity
    {
        DateTime? WriteTime { get; set; }

        string WriteUser { get; set; }

        string WriteIp { get; set; }
    }

}
