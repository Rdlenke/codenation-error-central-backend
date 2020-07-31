using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            foreach (LogError logError in logErrors)
            {
                ListLogErrorsViewModel model = new ListLogErrorsViewModel(
                userId: logError.UserId,
                details: logError.Details,
                level: logError.Level,
                createdAt: logError.CreatedAt,
                source: logError.Source,
                events: logError.Events
                );

                logErrorsViewModel.Add(model);
            }

            return new Response<List<ListLogErrorsViewModel>>(
                data: logErrorsViewModel, success: true, errors: null);
        }

        public Response<List<EnvironmentLogErrorsViewModel>> GetByEnvironment(EEnvironment environment)
        {
            var environmentLogErrors = _logErrorRepository.GetByEnvironment(environment);

            if (environmentLogErrors == null)
            {
                return new Response<List<EnvironmentLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var environmentErrorsViewModel = new List<EnvironmentLogErrorsViewModel>();
            foreach (LogError logError in environmentLogErrors)
            {
                EnvironmentLogErrorsViewModel model = new EnvironmentLogErrorsViewModel(
                userId: logError.UserId,
                details: logError.Details,
                level: logError.Level,
                createdAt: logError.CreatedAt,
                source: logError.Source,
                events: logError.Events
                );

                environmentErrorsViewModel.Add(model);
            }

            return new Response<List<EnvironmentLogErrorsViewModel>>(
                data: environmentErrorsViewModel, success: true, errors: null);
        }
    }
}
