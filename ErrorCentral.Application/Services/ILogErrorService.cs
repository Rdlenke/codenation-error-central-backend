using ErrorCentral.Application.ViewModels.LogError;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public interface ILogErrorService
    {
        Task<bool> Create(CreateLogErrorViewModel model, CancellationToken cancellationToken);
    }
}
