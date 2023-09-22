using AspNetCore.TaskScheduler.Infrastructure.Jobs;
using Quartz;

namespace AspNetCore.TaskScheduler.Infrastructure
{
    public static class DependencyInjection
    {

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                #region Config job cach 1
                var jobKey = JobKey.Create($"{nameof(LoggingBackgroundJob)}_1");
                options
                    .AddJob<LoggingBackgroundJob>(jobKey)
                    .AddTrigger(trigger =>
                    {
                        trigger.ForJob(jobKey)
                            .WithCronSchedule("0/5 * * * * ?"); // every 5 seconds;
                    });
                #endregion
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            #region Config job cach 2
            services.ConfigureOptions<LoggingBackgroundJobSetup>();
            #endregion
        }
    }
}
