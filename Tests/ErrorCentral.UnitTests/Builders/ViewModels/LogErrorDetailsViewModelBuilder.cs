using ErrorCentral.Application.ViewModels.LogError;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class LogErrorDetailsViewModelBuilder : IBuilder<LogErrorDetailsViewModel>
    {
        private LogErrorDetailsViewModel _logErrorDetailsViewModel;
        public LogErrorDetailsViewModelBuilder()
        {

        }
        public LogErrorDetailsViewModel Build()
        {
            return _logErrorDetailsViewModel;
        }
    }
}
