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
            string sourceFile = "";
            string targetFile = "";
            string configPath = Environment.CurrentDirectory;
            bool showMenu = true;

            //Reading existing .cfg files from project default I/O path (CFGCompareCLI\bin\Debug\net5.0)
            string[] fileEntries = Directory.GetFiles(configPath, "*.cfg");
            
            for (int i = 0; i < fileEntries.Length; i++)
            {
                string fileName = fileEntries[i].Substring(configPath.Length + 1);
                Console.WriteLine(i + " --- " + fileName);
            }

            Console.WriteLine("Choose source file");
            sourceFile = chooseFile(sourceFile, fileEntries);
            Console.WriteLine("Choose target file");
            targetFile = chooseFile(targetFile, fileEntries);

            source = readFromGzip.LoadData(sourceFile);
            target = readFromGzip.LoadData(targetFile);
            
            while (showMenu == true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) Perziureti parametru sarasa");
                Console.WriteLine("2) Rezultatu suvestine");
                Console.WriteLine("3) Filtruoti pagal id");
                Console.WriteLine("4) Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                       
                        break;
                    case "2":
                        
                        break;
                    case "3":
                        
                        break;
                    case "4":
                        showMenu = false;
                        break;
                }
            }

        }

        private static string chooseFile(string file, string[] fileEntries)
        {
            while (file == "")
            {
                int.TryParse(Console.ReadLine(), out int choice);
                if (choice < fileEntries.Length && choice > 0)
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
