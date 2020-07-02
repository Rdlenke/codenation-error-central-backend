using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogError;
using ErrorCentral.Domain.AggregatesModel.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public class LogErrorService : ILogErrorService
    {
        private ILogErrorRepository _logErrorRepository;
        private IUserRepository _userRepository;

        public LogErrorService(ILogErrorRepository logErrorRepository, IUserRepository userRepository)
        {
            this._logErrorRepository = logErrorRepository;
            this._userRepository = userRepository;
        }

        public async Task<bool> Create(CreateLogErrorViewModel model, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(model.UserId);
            if (user == null)
                throw new ArgumentException("Invalid number of userId");

            var logError = new LogError(
                userId: model.UserId,
                title: model.Title,
                details: model.Details,
                source: model.Source,
                level: model.Level,
                environment: model.Environment);

            _logErrorRepository.Add(logError);

            return await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}
