using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Service
{
    public class MatchParameter : BaseParameter
    {
        public MemberConditionDto Condition { get; set; }
        public int UserId { get; set; } //= 0;
        public string MessageContainer { get; set; }
    }
}
