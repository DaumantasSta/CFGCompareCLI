using CFGCompareCLI.Models;
using Microsoft.AspNetCore.Http;

namespace CFGCompareAPI.Services
{
    public interface IParameterComparisonService
    {
        void Post(IFormFile sourceFile, IFormFile targetFile, string sessionId);
        string Get(string sessionId);
        string GetResultsById(string id, string sessionId);
        string GetResultsByState(ParameterState state, string sessionId);
        bool CheckFile(string sessionId);
    }
}
