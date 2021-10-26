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
            bool showMenu = true;

            //Reading chosen directory and giving choice for user to choose files
            string[] fileEntries = ReadingCfgFilesInDir();
            Console.WriteLine("Choose source file");
            string sourceFile = ChooseFile(fileEntries);
            Console.WriteLine("\nChoose target file");
            string targetFile = ChooseFile(fileEntries);
            
            //Loading data from chosen Gzip files
            var source = ReadParamFromGzip.LoadData(sourceFile);
            var target = ReadParamFromGzip.LoadData(targetFile);

            //Extracting credentials
            var configurationCredentials = ConfigurationCredentials(source);

            ParameterComparison parameterComparison = new ParameterComparison(source, target);

            while (showMenu == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                PrintConfigurationCredentials(configurationCredentials);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Comparison summary:");
                parameterComparison.PrintSummary();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) View parameter list");
                Console.WriteLine("2) View parameter list by id");
                Console.WriteLine("3) View parameter list by comparison");
                Console.WriteLine("4) Exit");
                Console.Write("\r\nSelect an option: ");

                var selection = Console.ReadLine()?.Trim().ToLower();
                switch (selection)
                {
                    case "1":
                        parameterComparison.PrintParameters();
                        break;
                    case "2":
                        parameterComparison.PrintParametersWithIdFilter();
                        break;
                    case "3":
                        parameterComparison.PrintParametersWithComparisonFilter();
                        break;
                    case "4":
                        showMenu = false;
                        break;
                }
            }
        }

        private static List<string> ConfigurationCredentials(List<Parameter> source)
        {
            List<string> configurationCredentials = new List<string>();

            int indexOfFirstNumericParameter = source.FindIndex(x => int.TryParse(x.Id, out _) == true);
            for (int i = 0; i < indexOfFirstNumericParameter; i++)
                configurationCredentials.Add(source[i].Id + " " + source[i].Value);

            return configurationCredentials;
        }

        private static void PrintConfigurationCredentials(List<String> credentials)
        {
            foreach(string i in credentials)
                Console.WriteLine(i);
        }

        private static string[] ReadingCfgFilesInDir()
        {
            string[] fileList = new string[] {};
            while(fileList.Length<1)
            {
                Console.WriteLine("Enter directory of .cfg files");
                string configPath = Console.ReadLine();
                if (Directory.Exists(configPath))
                    fileList = Directory.GetFiles(configPath, "*.cfg");
                else
                    Console.WriteLine("Such directory doesn't exist");

                if(fileList.Length==0)
                    Console.WriteLine("No *.cfg files found in selected dir \n");
            }

            Console.WriteLine();
            return fileList;
        }

        private static string ChooseFile(string[] fileEntries)
        {
            for (int i = 0; i < fileEntries.Length; i++)
            {
                Console.WriteLine(i + " --- " + Path.GetFileName(fileEntries[i]));
            }

            string file = "";
            while (file == "")
            {
                var inputChoice = Console.ReadLine()?.Trim().ToLower();
                if (int.TryParse(inputChoice, out _))
                {
                    var choice = Convert.ToInt32(inputChoice);
                    if (choice < fileEntries.Length && choice >= 0)
                        file = fileEntries[choice];
                }
                else
                {
                    Console.WriteLine("Wrong choice");
                }
            }

            return file;
        }
    }
}
