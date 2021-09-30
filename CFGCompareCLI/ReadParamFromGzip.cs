using CFGCompareCLI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFGCompareCLI
{
    public class ReadParamFromGzip
    {
        public List<Parameter> LoadData(string fileDestination)
        {
            string temp = "";
            using (FileStream reader = File.OpenRead(fileDestination))
            using (GZipStream zip = new GZipStream(reader, CompressionMode.Decompress, false))
            using (StreamReader unzip = new StreamReader(zip))
                while (!unzip.EndOfStream)
                    temp = unzip.ReadLine();

            //Removing last character of the input file to make it work well with split functions
            temp = temp.Remove(temp.Length - 1);

            return temp.Split(';').Select(x => x.Split(':')).Select(x => new Parameter { Id = x[0], Value = x[1] }).ToList();
        }
    }
}
