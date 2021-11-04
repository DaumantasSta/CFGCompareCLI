using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CFGCompareAPI.Models;
using CFGCompareCLI;
using CFGCompareCLI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CFGCompareAPI.Services
{
    using Newtonsoft.Json;
    public class ParameterComparisonService:IParameterComparisonService
    {
        public void Post(IFormFile sourceFile, IFormFile targetFile)
        {
            var source = ReadParamFromGzip.LoadData(sourceFile.OpenReadStream());
            var target = ReadParamFromGzip.LoadData(targetFile.OpenReadStream());
            var sourceFileName = sourceFile.FileName;
            var targetFileName = targetFile.FileName;
            ParameterComparison parameterComparison = new ParameterComparison(source, target);
            SaveResults(parameterComparison, sourceFileName, targetFileName);
        }

        public void SaveResults(ParameterComparison parameterComparison, string sourceName, string targetName)
        {
            var parameterComparisonResult = parameterComparison.ReturnParameters();
            //Directories and file names
            string saveFolder = "Json_Temp";
            string jsonSavePath = saveFolder + "/" + "jsontemp" + ".json";
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
            
            ParameterComparisonJson parameterComparisonJson = new ParameterComparisonJson()
            {
                SourceName = sourceName,
                TargetName = targetName,
                Parameters = parameterComparison.ReturnParameters()
            };

            var resultJson = JsonConvert.SerializeObject(parameterComparisonJson, Formatting.Indented);
            File.WriteAllText(jsonSavePath, resultJson);
        }

        public string Get()
        {
            return ReadResults();
        }

        private static string ReadResults()
        {
            string saveFolder = "Json_Temp";
            string jsonSavePath = saveFolder + "/" + "save" + ".json";
            string jsonRead = "";
            jsonRead = File.ReadAllText(jsonSavePath);
            return jsonRead;
        }

        public string GetResultsById(string id)
        {
            var jsonRead = JsonConvert.DeserializeObject<ParameterComparisonJson>(ReadResults());
            var output = jsonRead.Parameters.FindAll(x => x.Id.StartsWith(id));
            return JsonConvert.SerializeObject(output, Formatting.Indented);
        }

        public string GetResultsByState(ParameterState state)
        {
            var jsonRead = JsonConvert.DeserializeObject<ParameterComparisonJson>(ReadResults());
            var output = jsonRead.Parameters.FindAll(x => x.State == state);
            return JsonConvert.SerializeObject(output, Formatting.Indented); ;
        }
    }
}
