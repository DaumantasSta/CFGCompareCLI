using System.Collections.Generic;
using CFGCompareCLI.Models;

namespace CFGCompareAPI.Models
{
    public class ParameterComparisonJson
    {
        public string SourceName { get; set; }
        public string TargetName { get; set; }
        public List<ParameterComparisonEntry> Parameters { get; set; }
    }
}
