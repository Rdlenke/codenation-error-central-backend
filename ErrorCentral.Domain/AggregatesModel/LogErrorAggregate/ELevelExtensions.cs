using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public static class ELevelExtensions
    {
        public static string ToFriendlyString(this ELevel level)
        {
            switch(level)
            {
                case ELevel.Debug:
                    return "Debug";
                case ELevel.Error:
                    return "Error";
                case ELevel.Warning:
                    return "Warning";
                default:
                    return null;
            }
        }
    }
}
