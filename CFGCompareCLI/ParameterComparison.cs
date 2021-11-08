using CFGCompareCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFGCompareCLI
{
    /// <summary>
    /// Compares both cfg file data
    /// </summary>

    public class ParameterComparison
    {
        private readonly List<ParameterComparisonEntry> _paramComparison = new List<ParameterComparisonEntry>();
        
        public ParameterComparison(List<Parameter> source, List<Parameter> target)
        {
            _paramComparison = InitializeParamComparison(source, target);
        }

        private static List<ParameterComparisonEntry> InitializeParamComparison(List<Parameter> source, List<Parameter> target)
        {
            var parameterComparisonDict = new Dictionary<string, ParameterComparisonEntry>();

            foreach (var parameter in source)
            {
                parameterComparisonDict[parameter.Id] =
                    ParameterComparisonEntry.CreateRemovedParameterComparisonEntry(parameter.Id, parameter.Value);
            }

            foreach (var parameter in target)
            {
                //If Id's are same
                if (parameterComparisonDict.TryGetValue(parameter.Id, out var entry))
                {
                    if (entry.Value == parameter.Value)
                    {
                        entry.State = ParameterState.Unchanged;
                    }
                    else
                    {
                        entry.State = ParameterState.Modified;
                    }
                }
                else
                {
                    parameterComparisonDict[parameter.Id] =
                        ParameterComparisonEntry.CreateAddedParameterComparisonEntry(parameter.Id, parameter.Value);
                }
            }

            return parameterComparisonDict.Values.ToList();
        }

        public List<ParameterComparisonEntry> ReturnParameters()
        {
            return _paramComparison;
        }
    }
}
