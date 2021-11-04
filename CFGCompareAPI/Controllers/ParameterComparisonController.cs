using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var sessionId = HttpContext.Session.Id;
            if (_parameterComparisonService.CheckFile(sessionId))
                return _parameterComparisonService.Get(sessionId);
            else
                return BadRequest("No files uploaded");
        }

        [HttpGet("byId/{id}")]
        //Get values by id
        public ActionResult<string> GetById(string id)
        {
            var sessionId = HttpContext.Session.Id;
            if (_parameterComparisonService.CheckFile(sessionId))
                return _parameterComparisonService.GetResultsById(id, sessionId);
            else
                return BadRequest("No files uploaded");
        }

        [HttpGet("byState/{state}")]
        //Get values by state
        public ActionResult<string> GetByState(ParameterState state)
        {
            var sessionId = HttpContext.Session.Id;
            if (_parameterComparisonService.CheckFile(sessionId))
                return _parameterComparisonService.GetResultsByState(state, sessionId);
            else
                return BadRequest("No files uploaded");
        }
        
        [HttpPost] // POST api/<ParameterComparisonController>
        public ActionResult Post(IFormFile source, IFormFile target)
        {
            //Initializing session
            HttpContext.Session.SetString("Session", "Session Value");
            var sessionId = HttpContext.Session.Id;

            //Checking if files are provided
            if (source == null || target == null)
                BadRequest("Upload both files");

            //Validate files
            var sourceExt = System.IO.Path.GetExtension(source.FileName);
            var targetExt = System.IO.Path.GetExtension(source.FileName);

            if (sourceExt == ".cfg" && targetExt == ".cfg")
            {
                _parameterComparisonService.Post(source, target, sessionId);
                return Ok("*.cfg files were uploaded successfully");
            }
            else
            {
                return BadRequest("Both or either of files aren't configuration files (*.cfg)");
            }
        }
    }
}
