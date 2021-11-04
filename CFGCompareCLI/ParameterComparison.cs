using CFGCompareCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CFGCompareCLI
{
    public enum ParameterState
    {
        Unchanged,
        Modified,
        Removed,
        Added
    }

    public class ParameterComparisonEntry
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public ParameterState State { get; set; }
    }

    /// <summary>
    /// Compares both cfg file data
    /// </summary>

    public class ParameterComparison
    {
        private List<ParameterComparisonEntry> _paramComparison = new List<ParameterComparisonEntry>();
        
        public ParameterComparison(List<Parameter> source, List<Parameter> target)
        {
            //Remove all alphabetic id's
            source.RemoveAll(x => int.TryParse(x.Id, out _) == false);
            target.RemoveAll(x => int.TryParse(x.Id, out _) == false);

            int index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                index = target.FindIndex(x => x.Id == source[i].Id); //Find same id in target, if not returns -1

                if (index > -1) //If same Id is found
                {
                    if (source[i].Value == target[index].Value)
                        _paramComparison.Add(new ParameterComparisonEntry { Id = source[i].Id, Value = source[i].Value, State = ParameterState.Unchanged });
                    else
                        _paramComparison.Add(new ParameterComparisonEntry { Id = source[i].Id, Value = source[i].Value, State = ParameterState.Modified });
                }
                else //Target doesn't have source's id
                {
                    _paramComparison.Add(new ParameterComparisonEntry { Id = source[i].Id, Value = source[i].Value, State = ParameterState.Removed });
                }
            }

            for (int i = 0; i < target.Count; i++)
            {
                index = _paramComparison.FindIndex(x => x.Id == target[i].Id); //If param comparison list doesn't have target id it returns -1
                if (index == -1)
                    _paramComparison.Add(new ParameterComparisonEntry { Id = target[i].Id, Value = target[i].Value, State = ParameterState.Added });
            }
        }

        public void PrintParameters()
        {
            PrintParameters(_paramComparison);
        }

        public List<ParameterComparisonEntry> ReturnParameters()
        {
            return _paramComparison;
        }

        private static void PrintParameters(List<ParameterComparisonEntry> output)
        {
            if (output.Count == 0)
            {
                Console.WriteLine("Records not found");
            }
            else
            {
                foreach (var item in output)
                {
                    Console.ForegroundColor = GetConsoleColor(item.State);
                    Console.WriteLine("ID: " + item.Id + " Value: " + item.Value + " Comparasion: " + item.State);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        public void PrintSummary()
        {
            int unchangedValue = _paramComparison.FindAll(x => x.State == ParameterState.Unchanged).Count;
            int modifiedValue = _paramComparison.FindAll(x => x.State == ParameterState.Modified).Count;
            int removedValue = _paramComparison.FindAll(x => x.State == ParameterState.Removed).Count;
            int addedValue = _paramComparison.FindAll(x => x.State == ParameterState.Added).Count;
            Console.WriteLine("Unchanged: " + unchangedValue + ", Modified: " + modifiedValue + ", Removed: " + removedValue + ", Added: " + addedValue);
        }

        public void PrintParametersWithIdFilter()
        {
            Console.WriteLine("Please type ID filter");
            string input = Console.ReadLine()?.Trim().ToLower();
            var output = _paramComparison.FindAll(x => x.Id.StartsWith(input));

            PrintParameters(output);
        }

        public void PrintParametersWithComparisonFilter()
        {
            int intState = 4;
            Console.WriteLine("Please type one number of an comparison filter (0 - unchanged, 1 - modified, 2 - removed, 3 - added)");
            string inputState = Console.ReadLine()?.Trim().ToLower();
            if (int.TryParse(inputState, out _))
                intState = Convert.ToInt32(inputState);

            var output = _paramComparison.FindAll(x => x.State == (ParameterState)intState);

            PrintParameters(output);
        }

        private static ConsoleColor GetConsoleColor(ParameterState state) => state switch
        {
            ParameterState.Unchanged => ConsoleColor.Gray,
            ParameterState.Modified => ConsoleColor.Yellow,
            ParameterState.Removed => ConsoleColor.Green,
            ParameterState.Added => ConsoleColor.Red,
            _ => Console.ForegroundColor
        };
    }
}
