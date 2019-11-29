using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public class AppKeyValueDto
    {
        public string AppGroup { get; set; }
        public string AppKey { get; set; }
        public string AppValue { get; set; }
    }

    public abstract class CrudStatusDto
    {
        public CrudStatus CrudStatus { get; set; }
    }

    public enum CrudStatus
    {
        Read = 0,
        Create,
        Update,
        Delete
    }
}
