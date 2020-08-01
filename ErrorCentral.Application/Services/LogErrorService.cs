using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ErrorCentral.Application.ViewModels.Misc;
using System.Text.RegularExpressions;

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

        public Response<List<ListLogErrorsViewModel>> Get(GetLogErrorsQueryViewModel query = null)
        {
            var logErrors = _logErrorRepository.GetList();

            if (logErrors == null)
            {
                return new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var listLogErrors = logErrors
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));
             

            if(query.Environment != 0)
            {
                listLogErrors = listLogErrors.Where(x => x.Environment == query.Environment);
            }

            if(query.Search != null)
            {
                Regex rx = new Regex(query.Search, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                switch(query.SearchBy)
                {
                    case 0:
                        break;
                    case ESearchBy.LEVEL:
                        listLogErrors = listLogErrors.Where(x => rx.IsMatch(x.Level.ToFriendlyString()));
                        break;
                    case ESearchBy.DETAILS:
                        listLogErrors = listLogErrors.Where(x => rx.IsMatch(x.Details));
                        break;
                    case ESearchBy.SOURCE:
                        listLogErrors = listLogErrors.Where(x => rx.IsMatch(x.Source));
                        break;
                    default:
                        listLogErrors = listLogErrors.Where(x => rx.IsMatch(x.Details));
                        break;
                }

            }

            switch (query.SortBy)
            {
                case 0:
                    break;
                case ESortBy.FREQUENCY:
                    listLogErrors = listLogErrors.OrderByDescending(x => x.Events);
                    break;
                case ESortBy.LEVEL:
                    listLogErrors = listLogErrors.OrderByDescending(x => x.Level);
                    break;
                default:
                    break;
            }

            return new Response<List<ListLogErrorsViewModel>>(
                    data: listLogErrors.ToList(), success: true, errors: null);
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
