using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;

        public string? SortedBy { get; set; } = null;

        public bool IsDecsending { get; set; } = false;
    }
}