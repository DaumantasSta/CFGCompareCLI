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
        public ParameterComparison()
        { }

        public ParameterComparison(List<Parameter> source, List<Parameter> target)
        {
            int max = source.Count + target.Count;
            int index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                index = target.FindIndex(x => x.Id == source[i].Id);
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
                //TODO
                //Need to fix here
                index = source.FindIndex(x => x.Id != target[i].Id);
                if (index == -1 && int.TryParse(source[i].Id, out _)) //Source doesn't have target id, and checks if it's numeric
                    _paramComparison.Add(new Tuple<string, string, char>(target[i].Id, target[i].Value, 'A'));
            }
        }

        public void printParameters()
        {
            printParameters(_paramComparison);
        }

        public void printSummary()
        {
            printSummary(_paramComparison);
        }

        private static void printParameters(List<Tuple<string, string, char>> output)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var item in output)
            {
                if (item.Item3 != 'U')
                    changeConsoleColour(item);
                Console.WriteLine("ID: " + item.Item1 + " Value: " + item.Item2 + " Comparasion: " + item.Item3);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (output.Count == 0)
                Console.WriteLine("Records not found");
            Console.ReadLine();
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

        private static void changeConsoleColour(Tuple<string, string, char> item)
        {
            if (item.Item3 == 'M')
                Console.ForegroundColor = ConsoleColor.Green;
            else if (item.Item3 == 'R')
                Console.ForegroundColor = ConsoleColor.Red;
            else if (item.Item3 == 'A')
                Console.ForegroundColor = ConsoleColor.Yellow;
        }

        private static void printSummary(List<Tuple<string, string, char>> paramComparison)
        {
            int unchangedValue = paramComparison.FindAll(x => x.Item3 == 'U').Count;
            int modifiedValue = paramComparison.FindAll(x => x.Item3 == 'M').Count;
            int removedValue = paramComparison.FindAll(x => x.Item3 == 'R').Count;
            int addedValue = paramComparison.FindAll(x => x.Item3 == 'A').Count;
            Console.WriteLine("Unchanged: " + unchangedValue + ", Modified: " + modifiedValue + ", Removed: " + removedValue + ", Added: " + addedValue);
        }
    }
}
