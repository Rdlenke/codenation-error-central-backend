using System;
using System.Net;
using System.Threading.Tasks;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;

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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<LogErrorDetailsViewModel>>> GetLogError(int id)
        {
            Response<LogErrorDetailsViewModel> model = await _logErrorService.GetLogError(id);

            if (model.Success == false)
            {
                return NotFound(model);
            }

            return Ok(model);
        }

        [HttpGet]
        public ActionResult<Response<List<ListLogErrorsViewModel>>> GetAll([FromQuery(Name = "environment")] EEnvironment environment)
        {
            if (environment == default)
            {
                Response<List<ListLogErrorsViewModel>> model = _logErrorService.GetAll();

                if (model.Success == false)
                {
                    return NotFound(model);
                }

                return Ok(model);

            }
            else
            {
                Response<List<ListLogErrorsViewModel>> model = _logErrorService.GetByEnvironment(environment);

                if (model.Success == false)
                {
                    return NotFound(model);
                }

                return Ok(model);
            }
        }

        //[HttpGet()]
        //public ActionResult<Response<List<ListLogErrorsViewModel>>> GetByEnvironment([FromQuery(Name = "environment")]EEnvironment environment)
        //{
        //    Response<List<ListLogErrorsViewModel>> model = _logErrorService.GetByEnvironment(environment);

        //    if (model.Success == false)
        //    {
        //        return NotFound(model);
        //    }

        //    return Ok(model);
        //}
    }
}