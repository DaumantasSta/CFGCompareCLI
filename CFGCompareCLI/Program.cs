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

            string[] fileEntries = ReadingCfgFilesInDir();

            Console.WriteLine("Choose source file");
            string sourceFile = ChooseFile(fileEntries);
            Console.WriteLine("\n Choose target file");
            string targetFile = ChooseFile(fileEntries);
            
            ReadParamFromGzip readFromGzip = new ReadParamFromGzip();
            //Loading data from chosen Gzip files
            List<Parameter> source = readFromGzip.LoadData(sourceFile);
            List<Parameter> target = readFromGzip.LoadData(targetFile);

            //Extracting credentials
            List<string> configurationCredentials = new List<string>();

            int indexOfFirstNumericParameter = source.FindIndex(x => int.TryParse(x.Id, out _) == true);
            for (int i = 0; i < indexOfFirstNumericParameter; i++)
                configurationCredentials.Add(source[i].Id + " " + source[i].Value);

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
                //string configPath = Console.ReadLine();
                string configPath = Environment.CurrentDirectory; //default I/O path (CFGCompareCLI\bin\Debug\net5.0)
                if (Directory.Exists(configPath))
                    fileList = Directory.GetFiles(configPath, "*.cfg");
                else
                    Console.WriteLine("Such directory doesn't exist");

                if(fileList.Length==0)
                    Console.WriteLine("No *.cfg files found in selected dir \n");
            }

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
                int.TryParse(Console.ReadLine(), out int choice);
                if (choice < fileEntries.Length && choice >= 0)
                {
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
