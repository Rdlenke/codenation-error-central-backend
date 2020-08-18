﻿using System;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using ErrorCentral.Application.ViewModels.User;

namespace ErrorCentral.API.v1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response<CreateLogErrorViewModel>>> PostAsync([FromBody] CreateLogErrorViewModel logError)
        {
            _logger.LogInformation(
                "----- Sending request: {ServiceName} - {ServiceMethod}: ({@ViewModel})",
                nameof(ILogErrorService),
                "CreateAsync",
                logError);

            //var idClaim = HttpContext.User.Claims.Where(c => c.Type == "id").FirstOrDefault();
            //int.TryParse(idClaim.Value, out int userId);
            //logError.UserId = userId;
            var result = await _logErrorService.CreateAsync(logError);

            if (!result.Success)
                return BadRequest(result);

            return Created(nameof(PostAsync), result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<LogErrorDetailsViewModel>>> GetAsync(int id)
        {
            var model = await _logErrorService.GetLogError(id);

            if (!model.Success)
                return NotFound(model);

            return Ok(model);
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<ListLogErrorsViewModel>>>> GetAll([FromQuery, Optional] GetLogErrorsQueryViewModel query)
        {
            Response<List<ListLogErrorsViewModel>> model = await _logErrorService.Get(query);

            if (model.Success == false)
            {
                return NotFound(model);
            }

            return Ok(model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response<int>>> DeleteAsync([FromRoute] int id)
        {
            _logger.LogInformation(
                "----- Sending request: {ServiceName} - {ServiceMethod}: ({@Id})",
                nameof(ILogErrorService),
                "DeleteAsync",
                id);

            var result = await _logErrorService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response<int>>> ArchiveAsync([FromRoute] int id)
        {
            _logger.LogInformation(
                "----- Sending request: {ServiceName} - {ServiceMethod}: ({@Id})",
                nameof(ILogErrorService),
                "ArchiveAsync",
                id);

            var result = await _logErrorService.ArchiveAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("archived/")]
        public async Task<ActionResult<Response<List<ListLogErrorsViewModel>>>> GetArchived()
        {
            var result = await _logErrorService.GetArchived();

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Response<int>>> UnarchiveAsync([FromRoute] int id)
        {
            var result = await _logErrorService.UnarchiveAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}