using System;
using System.Collections.Generic;
using System.IO;
using CFGCompareCLI.Models;

namespace CFGCompareCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadParamFromGzip readFromGzip = new ReadParamFromGzip();
            List<Parameter> source = new List<Parameter>();
            List<Parameter> target = new List<Parameter>();
            bool showMenu = true;

            //Reading existing .cfg files from project default I/O path (CFGCompareCLI\bin\Debug\net5.0)
            string configPath = Environment.CurrentDirectory;
            string[] fileEntries = Directory.GetFiles(configPath, "*.cfg");

            for (int i = 0; i < fileEntries.Length; i++)
            {
                string fileName = fileEntries[i].Substring(configPath.Length + 1);
                Console.WriteLine(i + " --- " + fileName);
            }

            Console.WriteLine("Choose source file");
            string sourceFile = chooseFile(fileEntries);
            Console.WriteLine("Choose target file");
            string targetFile = chooseFile(fileEntries);
            //Loading data from chosen Gzip files
            source = readFromGzip.LoadData(sourceFile);
            target = readFromGzip.LoadData(targetFile);

            ParameterComparison parameterComparison = new ParameterComparison(source, target);

            while (showMenu == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                PrintConfigurationCredentials(source);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Comparison summary:");
                parameterComparison.printSummary();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) Perziureti parametru sarasa");
                Console.WriteLine("2) Filtruoti pagal id");
                Console.WriteLine("3) Filtruoti pagal palyginima");
                Console.WriteLine("4) Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        parameterComparison.printParameters();
                        break;
                    case "2":
                        parameterComparison.printParametersWithIdFilter();
                        break;
                    case "3":
                        parameterComparison.printParametersWithComparisonFilter();
                        break;
                    case "4":
                        showMenu = false;
                        break;
                }
            }
        }

        private static void PrintConfigurationCredentials(List<Parameter> source)
        {
            int index = source.FindIndex(x => int.TryParse(x.Id, out _) == true);
            for (int i = 0; i < index; i++)
                Console.WriteLine(source[i].Id + " " + source[i].Value);
        }

        private static string chooseFile(string[] fileEntries)
        {
            string file = "";
            while (file == "")
            {
                int.TryParse(Console.ReadLine(), out int choice);
                if (choice < fileEntries.Length && choice >= 0)
                    file = fileEntries[choice];
                else
                {
                    Console.WriteLine("Wrong choice");
                }
            }

            return file;
        }
    }
}
