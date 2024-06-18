using Quartz;

namespace AspNetCore.TaskScheduler.Quartz.Scheduler.Jobs;

public class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> _logger;

    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        _logger = logger;
    }
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Quartz: {DateTime.UtcNow}");

        return Task.CompletedTask;
    }
}
