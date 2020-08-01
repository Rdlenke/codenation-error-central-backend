using ErrorCentral.Application.ViewModels.LogError;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public interface ILogErrorService
    {
        Task<bool> CreateAsync(CreateLogErrorViewModel model, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(int id);
    }
}
