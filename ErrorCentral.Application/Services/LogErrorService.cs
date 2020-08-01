using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<bool> CreateAsync(CreateLogErrorViewModel model, CancellationToken cancellationToken = default)
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

        public async Task<Response<LogErrorDetailsViewModel>> GetLogError(int id)
        {
            var logError = await _logErrorRepository.GetById(id);

            if (logError == null)
            {
                return new Response<LogErrorDetailsViewModel>(
                    success: false,
                    errors: new[] { $"There isn't a log error with {id}" });
            }

            LogErrorDetailsViewModel model = new LogErrorDetailsViewModel(
                createdAt: logError.CreatedAt,
                environment: logError.Environment,
                level: logError.Level,
                source: logError.Source,
                details: logError.Details,
                title: logError.Title,
                userId: logError.UserId
            );

            return new Response<LogErrorDetailsViewModel>(
                data: model, success: true, errors: null);
        }

        public Response<List<ListLogErrorsViewModel>> GetAll()
        {
            var logErrors = _logErrorRepository.GetList();

            if (logErrors == null)
            {
                return new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var logErrorsViewModel = new List<ListLogErrorsViewModel>();

            var listLogErrors = logErrors
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level:x.Key.Level, 
                                                        source: x.Key.Source,
                                                        title: x.Key.Title, 
                                                        userId: x.Key.UserId, 
                                                        events: x.Count()))
                .ToList();

            return new Response<List<ListLogErrorsViewModel>>(
                data: listLogErrors, success: true, errors: null);
        }

        public Response<List<ListLogErrorsViewModel>> GetByEnvironment(EEnvironment environment)
        {
            var environmentLogErrors = _logErrorRepository.GetByEnvironment(environment);

            if (environmentLogErrors == null)
            {
                return new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var environmentErrorsViewModel = new List<ListLogErrorsViewModel>();

            var listLogErrors = environmentLogErrors
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        events: x.Count()))
                .ToList();

            return new Response<List<ListLogErrorsViewModel>>(
                data: environmentErrorsViewModel, success: true, errors: null);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            var logError = await _logErrorRepository.GetByIdAsync(id);
            if (logError == null)
                throw new ArgumentException("Invalid number of LogError");

            logError.Remove();
            _logErrorRepository.Update(logError);

            return await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync();
        }
    }
}
