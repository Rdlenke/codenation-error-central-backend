using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ErrorCentral.Application.ViewModels.Misc
{
    public class GetLogErrorsQueryViewModel
    {
        [FromQuery(Name = "environment")]
        public EEnvironment Environment { get; set; }
        [FromQuery(Name = "sortby")]
        public ESortBy SortBy { get; set; }
        [FromQuery(Name = "search")]
        public string Search { get; set; }

        [FromQuery(Name = "searchBy")]
        public ESearchBy SearchBy { get; set; }
    }
}
