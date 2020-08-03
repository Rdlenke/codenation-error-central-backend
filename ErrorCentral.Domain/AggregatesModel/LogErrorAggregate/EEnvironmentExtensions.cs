using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Domain.AggregatesModel.LogErrorAggregate
{
    public static class EEnvironmentExtensions
    {
        public static string ToFriendlyString(this EEnvironment environment)
        {
            switch(environment)
            {
                case EEnvironment.Development:
                    return "Development";
                case EEnvironment.Homologation:
                    return "Homologation";
                case EEnvironment.Production:
                    return "Production";
                default:
                    return null;
            }
        }
    }
}
