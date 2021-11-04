using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CFGCompareCLI;
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
            var target = ReadParamFromGzip.LoadData(sourceFile.OpenReadStream());
            ParameterComparison parameterComparison = new ParameterComparison(source, target);
            SaveResults(parameterComparison);
        }

        public void SaveResults(ParameterComparison parameterComparison)
        {
            string saveFolder = "Json_Temp";
            string jsonSavePath = saveFolder + "/" + "save" + ".json";
            var resultJson = JsonConvert.SerializeObject(parameterComparison.ReturnParameters(), Formatting.Indented);
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            File.WriteAllText(jsonSavePath, resultJson);
        }

        public string Get()
        {
            string saveFolder = "Json_Temp";
            string jsonSavePath = saveFolder + "/" + "save" + ".json";
            string jsonRead = "";
            jsonRead = File.ReadAllText(jsonSavePath);
            return jsonRead;
        }

        public string GetById(string id)
        {
            return "";
        }

        public string GetByState(string state)
        {
            return "";
        }
    }
}
