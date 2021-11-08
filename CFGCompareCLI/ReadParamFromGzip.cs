using CFGCompareCLI.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CFGCompareCLI
{
    public static class ReadParamFromGzip
    {
        public static List<Parameter> LoadData(string fileDestination)
        {
            using FileStream reader = File.OpenRead(fileDestination);
            return LoadData(reader);
        }

        public static List<Parameter> LoadData(Stream reader)
        {
            string input = "";
            using (GZipStream zip = new GZipStream(reader, CompressionMode.Decompress, false))
            using (StreamReader unzip = new StreamReader(zip))
                while (!unzip.EndOfStream)
                    input = unzip.ReadLine();

            //Removing last character of the input file to make it work well with split functions
            input = input.Remove(input.Length - 1);

            return input.Split(';').Select(x => x.Split(':')).Select(x => new Parameter { Id = x[0], Value = x[1] }).ToList();
        }
    }
}
