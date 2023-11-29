using AspNetCore.TaskScheduler.Hangfire.Infrastructure.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AspNetCore.TaskScheduler.Hangfire.Infrastructure
{
    public class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));
            options
                .AddJob<LoggingBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                {
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(scheduler =>
                        {
                            scheduler.WithIntervalInSeconds(2).RepeatForever();
                        });

                });
        }
    }
}
