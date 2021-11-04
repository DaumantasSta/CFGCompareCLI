using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFGCompareCLI.Models;
using Microsoft.AspNetCore.Http;

namespace CFGCompareAPI.Services
{
    public interface IParameterComparisonService
    {
        void Post(IFormFile sourceFile, IFormFile targetFile);
        string Get();
        string GetResultsById(string id);
        string GetResultsByState(ParameterState state);
    }
}
