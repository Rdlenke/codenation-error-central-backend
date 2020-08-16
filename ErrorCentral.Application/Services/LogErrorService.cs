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
using ErrorCentral.Application.ViewModels.Validators;
using ErrorCentral.Application.ViewModels.User;

namespace ErrorCentral.Application.Services
{
    public class LogErrorService : ILogErrorService
    {
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IUserRepository _userRepository;

        public LogErrorService(ILogErrorRepository logErrorRepository, IUserRepository userRepository)
        {
            _logErrorRepository = logErrorRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<CreateLogErrorViewModel>> CreateAsync(CreateLogErrorViewModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await new CreateLogErrorViewModelValidator().ValidateAsync(model);

            if (!validationResult.IsValid)
                return new Response<CreateLogErrorViewModel>(
                    data: model,
                    success: false,
                    errors: validationResult.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            var user = await _userRepository.GetAsync(model.UserId);
            if (user == null)
                return new Response<CreateLogErrorViewModel>(
                    data: model,
                    success: false,
                    errors: new [] { $"User with id {model.UserId} not found" }
                );

            var logError = new LogError(
                userId: model.UserId,
                title: model.Title,
                details: model.Details,
                source: model.Source,
                level: model.Level,
                environment: model.Environment);

            await _logErrorRepository.AddAsync(logError);

            var result = await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);

            return result ? new Response<CreateLogErrorViewModel>(model, result) : new Response<CreateLogErrorViewModel>(model, false, new[] { $"Error persisting database changes" });
        }

        public async Task<Response<LogErrorDetailsViewModel>> GetLogError(int id)
        {
            var logError = await _logErrorRepository.GetByIdAsync(id);

            if (logError == null)
                return new Response<LogErrorDetailsViewModel>(
                    success: false,
                    errors: new[] { $"There isn't a log error with {id}" });

            var model = new LogErrorDetailsViewModel()
            {
                UserId = logError.UserId,
                Title = logError.Title,
                Details = logError.Details,
                Level = logError.Level,
                Environment = logError.Environment,
                CreatedAt = logError.CreatedAt,
                Source = logError.Source,
            };

            return new Response<LogErrorDetailsViewModel>(data: model, success: true);
        }

        public async Task<Response<List<ListLogErrorsViewModel>>> Get(GetLogErrorsQueryViewModel query)
        {
            var logErrors = await _logErrorRepository.GetAllUnarchivedAsync();

            if (logErrors == null)
            {
                return new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var listLogErrors = logErrors
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        filed: x.Filed,
                                                        events: CountEvents(x, logErrors)));
             

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
                        listLogErrors = listLogErrors.Where(x => rx.IsMatch(x.Details));
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


        public async Task<Response<int>> RemoveAsync(int id)
        {
            var logError = await _logErrorRepository.GetByIdAsync(id);
            if (logError == null)
                return new Response<int>(id, false, new[] { $"object with id {id} not found" });

            logError.Remove();
            _logErrorRepository.Update(logError);

            var result = await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync();
            return result ? new Response<int>(id, result) : new Response<int>(id, false, new[] { $"Error persisting database changes" });
        }

        public async Task<Response<int>> ArchiveAsync(int id)
        {
            var logError = await _logErrorRepository.GetByIdAsync(id);
            if (logError == null)
                return new Response<int>(id, false, new[] { $"object with id {id} not found" });

            logError.Archive();
            _logErrorRepository.Update(logError);

            var result = await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync();
            return result ? new Response<int>(id, result) : new Response<int>(id, false, new[] { $"Error persisting database changes" });
        }

        public async Task<Response<List<ListLogErrorsViewModel>>> GetArchived()
        {
            var logErrors = await _logErrorRepository.GetArchivedAsync();

            if (logErrors == null)
            {
                return new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });
            }

            var listLogErrors = logErrors
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        filed: x.Filed,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, logErrors))).ToList();

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: listLogErrors, success: true, errors: null);

            return response;
        }

        public async Task<Response<int>> UnarchiveAsync(int id)
        {
            var logError = await _logErrorRepository.GetByIdAsync(id);

            if (logError == null)
                return new Response<int>(id, false, new[] { $"object with id {id} not found" });

            logError.Unarchive();
            _logErrorRepository.Update(logError);

            var result = await _logErrorRepository.UnitOfWork
                .SaveEntitiesAsync();

            return result ? new Response<int>(id, result) : new Response<int>(id, false, new[] { $"Error persisting database changes" });
        }


        private int CountEvents(LogError logError, IList<LogError> listAllLogErrors)
        {
            var list = listAllLogErrors
                .Where(x => x.Details == logError.Details &&
                       x.Environment == logError.Environment &&
                       x.Title == logError.Title &&
                       x.Source == logError.Source &&
                       x.Level == logError.Level).ToList();

            return list.Count;
        }
    }
}
