using System;
using System.Net;
using System.Threading.Tasks;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ErrorCentral.Application.ViewModels.Misc;
using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Authorization;

namespace ErrorCentral.API.v1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LogErrorsController : ControllerBase
    {
        private readonly ILogErrorService _logErrorService;
        private readonly ILogger<LogErrorsController> _logger;

        public LogErrorsController(ILogErrorService logErrorService, ILogger<LogErrorsController> logger)
        {
            _logErrorService = logErrorService ?? throw new ArgumentNullException(nameof(logErrorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] CreateLogErrorViewModel logError)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation(
                "----- Sending request: {ServiceName} - {ServiceMethod}: ({@ViewModel})",
                nameof(ILogErrorService),
                "CreateAsync",
                logError);

            var result = await _logErrorService.CreateAsync(logError);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<LogErrorDetailsViewModel>>> GetAsync(int id)
        {
            Response<LogErrorDetailsViewModel> model = await _logErrorService.GetLogError(id);

            if (model.Success == false)
            {
                return NotFound(model);
            }

            return Ok(model);
        }

        [HttpGet]
        public ActionResult<Response<List<ListLogErrorsViewModel>>> GetAll([FromQuery, Optional] GetLogErrorsQueryViewModel query)
        {
            Response<List<ListLogErrorsViewModel>> model = _logErrorService.Get(query); 

            if (model.Success == false)
            {
                return NotFound(model);
            }

            return Ok(model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            _logger.LogInformation(
                "----- Sending request: {ServiceName} - {ServiceMethod}: ({@Id})",
                nameof(ILogErrorService),
                "RemoveAsync",
                id);

            var result = await _logErrorService.RemoveAsync(id);
            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}