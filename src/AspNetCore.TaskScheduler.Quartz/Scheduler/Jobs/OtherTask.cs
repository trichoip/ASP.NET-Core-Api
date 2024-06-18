using Quartz;

namespace AspNetCore.TaskScheduler.Quartz.Scheduler.Jobs;

public class OtherTask : IJob
{
    public const string OrderIdKey = "orderId";

    private readonly ILogger<OtherTask> _logger;

    public OtherTask(ILogger<OtherTask> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;

        var orderId = dataMap.GetIntValue(OrderIdKey);

        _logger.LogInformation("Clear expired order {orderId} at {time}", orderId, DateTimeOffset.UtcNow);

        return Task.CompletedTask;
    }
}
