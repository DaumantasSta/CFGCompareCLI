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
        private List<Tuple<string, string, char>> _paramComparison = new List<Tuple<string, string, char>>(); //ID, Value, UMRA

        public ParameterComparison(List<Parameter> source, List<Parameter> target)
        {
            int index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                index = target.FindIndex(x => x.Id == source[i].Id); //Find same id in target, if not returns -1
                if (index > 0 && int.TryParse(source[i].Id, out _)) //If same id exist and it is numeric one
                {
                    if (source[i].Value == target[index].Value)
                        _paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'U'));
                    else
                        _paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'M'));
                }
                else if (int.TryParse(source[i].Id, out _)) //Target doesn't have source id
                    _paramComparison.Add(new Tuple<string, string, char>(source[i].Id, source[i].Value, 'R'));
            }

            for (int i = 0; i < target.Count; i++)
            {
                index = _paramComparison.FindIndex(x => x.Item1 == target[i].Id); //If param comparison dont have target id it returns -1
                if(index==-1 && int.TryParse(target[i].Id, out _)) //Check if target id numeric
                    _paramComparison.Add(new Tuple<string, string, char>(target[i].Id, target[i].Value, 'A'));
            }
        }

        public void printParameters()
        {
            printParameters(_paramComparison);
        }

        private static void printParameters(List<Tuple<string, string, char>> output)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var item in output)
            {
                Console.ForegroundColor = GetConsoleColor(item.Item3);
                Console.WriteLine("ID: " + item.Item1 + " Value: " + item.Item2 + " Comparasion: " + item.Item3);
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (output.Count == 0)
                Console.WriteLine("Records not found");
            Console.ReadLine();
        }
        public void printSummary()
        {
            int unchangedValue = _paramComparison.FindAll(x => x.Item3 == 'U').Count;
            int modifiedValue = _paramComparison.FindAll(x => x.Item3 == 'M').Count;
            int removedValue = _paramComparison.FindAll(x => x.Item3 == 'R').Count;
            int addedValue = _paramComparison.FindAll(x => x.Item3 == 'A').Count;
            Console.WriteLine("Unchanged: " + unchangedValue + ", Modified: " + modifiedValue + ", Removed: " + removedValue + ", Added: " + addedValue);
        }

        public void printParametersWithIdFilter()
        {
            Console.WriteLine("Please type ID filter");
            string input = Console.ReadLine();
            var output = _paramComparison.FindAll(x => x.Item1.StartsWith(input));

            printParameters(output);
        }

        public void printParametersWithComparisonFilter()
        {
            char charInput;
            Console.WriteLine("Please type one letter of an comparison filter (U - unchanged, M - modified, R - removed, A - added)");
            string input = Console.ReadLine();
            Char.TryParse(input, out charInput);
            var output = _paramComparison.FindAll(x => x.Item3 == charInput);

            printParameters(output);
        }

        private static ConsoleColor GetConsoleColor(char state) => state switch
        {
            'M' => ConsoleColor.Green,
            'R' => ConsoleColor.Red,
            'A' => ConsoleColor.Yellow,
            'U' => ConsoleColor.Gray,
            _ => Console.ForegroundColor
        };
    }
}
