using System;
using System.Net;
using System.Threading.Tasks;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace ErrorCentral.API.v1.Controllers
{
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
        public async Task<IActionResult> CreateLogErrorAsync([FromBody] CreateLogErrorViewModel logError)
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveLogErrorAsync([FromRoute] int id)
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
