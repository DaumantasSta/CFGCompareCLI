using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFGCompareAPI.Services;
using CFGCompareCLI;
using CFGCompareCLI.Models;
using Microsoft.AspNetCore.Http;

namespace CFGCompareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterComparisonController : ControllerBase
    {
        private readonly IParameterComparisonService _parameterComparisonService;
        public ParameterComparisonController(IParameterComparisonService parameterComparisonService)
        {
            _parameterComparisonService = parameterComparisonService;
        }

        [HttpGet] // GET: api/<ParameterComparisonController>
        //Get all values
        public ActionResult <string> Get()
        {
            return _parameterComparisonService.Get();
        }

        [HttpGet("byId/{id}")]
        //Get values by id
        public ActionResult<string> GetById(string id)
        {
            return _parameterComparisonService.GetResultsById(id);
        }

        [HttpGet("byState/{state}")]
        //Get values by state
        public ActionResult<string> GetByState(ParameterState state)
        {
            return _parameterComparisonService.GetResultsByState(state);
        }
        
        [HttpPost] // POST api/<ParameterComparisonController>
        public ActionResult Post(IFormFile source, IFormFile target)
        {
            //Checking if files are present
            if (source == null || target == null)
                BadRequest("Upload both files");

            //Validate files
            var sourceExt = System.IO.Path.GetExtension(source.FileName);
            var targetExt = System.IO.Path.GetExtension(source.FileName);

            if (sourceExt == ".cfg" && targetExt == ".cfg")
            {
                _parameterComparisonService.Post(source, target);
                return Ok("*.cfg files were uploaded successfully");
            }
            else
            {
                return BadRequest("Both or either of files aren't configuration files (*.cfg)");
            }
        }
    }
}
