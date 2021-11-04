using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFGCompareCLI.Models
{
    public class ParameterComparisonEntry
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public ParameterState State { get; set; }
    }
}
