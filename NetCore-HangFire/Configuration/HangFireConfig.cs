

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCore_HangFire.Configuration
{
    public static class HangFireConfig
    {
        public static IServiceCollection AddHangFireConfiguration(this IServiceCollection services,IConfiguration configuration)
        {          
            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                configuration.GetSection("HangFireConfiguration:ConnectionString").Value, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            
            services.AddHangfireServer();

            return services;
        }

        public static IApplicationBuilder UseHangFire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard(); // Will be available under http://localhost:5000/hangfire
            
            return app;
        }
    }
}
