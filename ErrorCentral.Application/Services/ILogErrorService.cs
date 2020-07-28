﻿using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ErrorCentral.Application.Services
{
    public interface ILogErrorService
    {
        Task<bool> CreateAsync(CreateLogErrorViewModel model, CancellationToken cancellationToken = default);
        Task<Response<LogErrorDetailsViewModel>> GetLogError(int id);
        Response<List<ListLogErrorsViewModel>> GetAll();
    }
}
