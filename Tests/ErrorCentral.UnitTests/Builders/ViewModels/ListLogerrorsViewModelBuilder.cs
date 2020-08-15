using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class ListLogerrorsViewModelBuilder : IBuilder<ListLogErrorsViewModel>
    {

        public string Title => "Run-time exception (line 8): Attempted to divide by zero.";
        public string Details => "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8";
        public string Source => "http://production.com/";
        public ELevel Level => ELevel.Error;
        public EEnvironment Environment => EEnvironment.Production;
        public int UserId => 1;
        public int Events => 1;

        private ListLogErrorsViewModel _listLogErrorsViewModel;

        public ListLogerrorsViewModelBuilder()
        {
            _listLogErrorsViewModel = WithDefaultValues();
        }

        public ListLogErrorsViewModel Build()
        {
            return _listLogErrorsViewModel;
        }

        public ListLogErrorsViewModel WithDefaultValues()
        {
            return new ListLogErrorsViewModel(userId: UserId, title: Title, level: Level, environment: Environment, source: Source, details: Details, events: Events);
        }
    }
}
