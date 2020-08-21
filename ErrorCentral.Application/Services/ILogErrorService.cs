using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ErrorCentral.Application.ViewModels.Misc;
using ErrorCentral.Application.ViewModels.User;

namespace ErrorCentral.Application.Services
{
    public interface ILogErrorService
    {
        Task<Response<CreateLogErrorViewModel>> CreateAsync(CreateLogErrorViewModel model, CancellationToken cancellationToken = default);
        Task<Response<LogErrorDetailsViewModel>> GetLogError(int id);
        Task<Response<List<ListLogErrorsViewModel>>> Get(GetLogErrorsQueryViewModel query);
        Task<Response<int>> RemoveAsync(int id);
        Task<Response<int>> ArchiveAsync(int id);
        Task<Response<int>> UnarchiveAsync(int id);
        Task<Response<List<ListLogErrorsViewModel>>> GetArchived();
    }
}
