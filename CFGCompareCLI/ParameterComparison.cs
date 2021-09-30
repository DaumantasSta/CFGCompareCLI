using CFGCompareCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFGCompareCLI
{
    public class ParameterComparison
    {
        public ParameterComparison()
        { }

        public ParameterComparison(List<Parameter> source, List<Parameter> target)
        {
            var paramComparison = new List<Tuple<string, string, char>>(); //ID, Value, UMRA

            int max = source.Count + target.Count;
            int index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                index = target.FindIndex(x => x.Id == source[i].Id);
                if (index > 0 && int.TryParse(source[i].Id, out _)) //If same id exist and it is numeric one
                {
                    if (source[i].Value == target[index].Value)
                        paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'U'));
                    else
                        paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'M'));
                }
                else if (int.TryParse(source[i].Id, out _)) //Target doesn't have source id
                    paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'R'));
            }

            for (int i = 0; i < target.Count; i++)
            {
                index = source.FindIndex(x => x.Id == target[i].Id);
                if (index == -1 && int.TryParse(source[i].Id, out _)) //Source doesn't have target id, and checks if it's numeric
                    paramComparison.Add(new Tuple<string, string, char>(target[i].Id, target[i].Value, 'U'));
            }

            printSummary(paramComparison);
        }

        private static void printSummary(List<Tuple<string, string, char>> paramComparison)
        {
            int unchangedValue = paramComparison.FindAll(x => x.Item3 == 'U').Count;
            int modifiedValue = paramComparison.FindAll(x => x.Item3 == 'M').Count;
            int removedValue = paramComparison.FindAll(x => x.Item3 == 'R').Count;
            int addedValue = paramComparison.FindAll(x => x.Item3 == 'A').Count;
            Console.WriteLine(unchangedValue + " " + modifiedValue + " " + removedValue + " " + addedValue);
        }
    }
}
