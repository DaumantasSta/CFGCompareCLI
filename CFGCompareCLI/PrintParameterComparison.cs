using CFGCompareCLI.Models;
using System;
using System.Collections.Generic;

namespace CFGCompareCLI
{
    public static class PrintParameterComparison
    {
        public static void PrintParameters(List<ParameterComparisonEntry> output)
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
                    Console.WriteLine("ID: " + item.Id + " Value: " + item.Value + " Comparison: " + item.State);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        public static void PrintParametersWithIdFilter(List<ParameterComparisonEntry> parameterComparisonEntry)
        {
            Console.WriteLine("Please type ID filter");
            string input = Console.ReadLine()?.Trim().ToLower();
            var output = parameterComparisonEntry.FindAll(x => x.Id.StartsWith(input));

            PrintParameters(output);
        }

        public static void PrintParametersWithComparisonFilter(List<ParameterComparisonEntry> parameterComparisonEntry)
        {
            int intState = 4;
            Console.WriteLine("Please type one number of an comparison filter (0 - unchanged, 1 - modified, 2 - removed, 3 - added)");
            string inputState = Console.ReadLine()?.Trim().ToLower();
            if (int.TryParse(inputState, out _))
                intState = Convert.ToInt32(inputState);

            var output = parameterComparisonEntry.FindAll(x => x.State == (ParameterState)intState);

            PrintParameters(output);
        }

        public static void PrintSummary(List<ParameterComparisonEntry> parameterComparisonEntry)
        {
            int unchangedValue = parameterComparisonEntry.FindAll(x => x.State == ParameterState.Unchanged).Count;
            int modifiedValue = parameterComparisonEntry.FindAll(x => x.State == ParameterState.Modified).Count;
            int removedValue = parameterComparisonEntry.FindAll(x => x.State == ParameterState.Removed).Count;
            int addedValue = parameterComparisonEntry.FindAll(x => x.State == ParameterState.Added).Count;
            Console.WriteLine("Unchanged: " + unchangedValue + ", Modified: " + modifiedValue + ", Removed: " + removedValue + ", Added: " + addedValue);
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