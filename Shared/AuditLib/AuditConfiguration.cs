using Audit.Core;
using DTO;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLib
{
    public static class AuditConfiguration
    {
        /// <summary>
        /// Global Audit configuration
        /// </summary>
        public static IServiceCollection ConfigureAudit(this IServiceCollection serviceCollection, ConfigAuditLog configAuditLog)
        {
            Configuration.Setup().UseCustomProvider(new AuditCustomDataProvider(configAuditLog)).WithCreationPolicy(EventCreationPolicy.Manual);
            //Configuration.AddCustomAction(ActionType.OnEventSaving, scope =>
            //{
            //    if (scope.Event.Environment.Exception != null)
            //    {
            //        scope.SetCustomField("Exception", scope.Event.Environment.Exception);
            //    }
            //});
            return serviceCollection;
        }

       
    }
}
