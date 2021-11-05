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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CFGCompareAPI.Services
{
    using Newtonsoft.Json;
    public class ParameterComparisonService:IParameterComparisonService
    {
        private string _saveFolder = "Json_Temp";

        public void Post(IFormFile sourceFile, IFormFile targetFile, string sessionId)
        {

            var source = ReadParamFromGzip.LoadData(sourceFile.OpenReadStream());
            var target = ReadParamFromGzip.LoadData(targetFile.OpenReadStream());
            var sourceFileName = sourceFile.FileName;
            var targetFileName = targetFile.FileName;
            ParameterComparison parameterComparison = new ParameterComparison(source, target);
            SaveResults(parameterComparison, sourceFileName, targetFileName, sessionId);
        }

        public void SaveResults(ParameterComparison parameterComparison, string sourceName, string targetName, string sessionId)
        {
            var parameterComparisonResult = parameterComparison.ReturnParameters();
            //Directories and file names
            string jsonSavePath = _saveFolder + "/" + sessionId + ".json";
            if (!Directory.Exists(_saveFolder))
                Directory.CreateDirectory(_saveFolder);
            
            ParameterComparisonJson parameterComparisonJson = new ParameterComparisonJson()
            {
                SourceName = sourceName,
                TargetName = targetName,
                Parameters = parameterComparisonResult
            };

            var resultJson = JsonConvert.SerializeObject(parameterComparisonJson, Formatting.Indented);
            File.WriteAllText(jsonSavePath, resultJson);
        }

        public string Get(string sessionId)
        {
            return ReadResults(sessionId);
        }

        private static string ReadResults(string sessionId)
        {
            string saveFolder = "Json_Temp";
            string jsonSavePath = saveFolder + "/" + sessionId + ".json";
            var jsonRead = File.ReadAllText(jsonSavePath);
            return jsonRead;
        }

        public string GetResultsById(string id, string sessionId)
        {
            var jsonRead = JsonConvert.DeserializeObject<ParameterComparisonJson>(ReadResults(sessionId));
            var output = jsonRead.Parameters.FindAll(x => x.Id.StartsWith(id));
            return JsonConvert.SerializeObject(output, Formatting.Indented);
        }

        public string GetResultsByState(ParameterState state, string sessionId)
        {
            var jsonRead = JsonConvert.DeserializeObject<ParameterComparisonJson>(ReadResults(sessionId));
            var output = jsonRead.Parameters.FindAll(x => x.State == state);
            return JsonConvert.SerializeObject(output, Formatting.Indented); ;
        }

        public bool CheckFile(string sessionId)
        {
            string jsonSavePath = _saveFolder + "/" + sessionId + ".json";
            if (File.Exists(jsonSavePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
