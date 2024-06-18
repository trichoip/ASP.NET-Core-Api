using AspNetCore.TaskScheduler.Quartz.Scheduler.Jobs;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace AspNetCore.TaskScheduler.Quartz.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class QuartzController : ControllerBase
{
    private readonly ILogger<QuartzController> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public QuartzController(
        ILogger<QuartzController> logger,
        ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    [HttpGet]
    public async Task<IActionResult> OtherTaskDemo(int orderId, int second)
    {
        _logger.LogInformation("Order {orderId} will be expired at {time}", orderId, DateTimeOffset.Now.AddSeconds(second));
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.Start();

        var job = JobBuilder.Create<OtherTask>()
            .UsingJobData(OtherTask.OrderIdKey, orderId)
            .Build();

        var trigger = TriggerBuilder.Create()
            .StartAt(DateTimeOffset.Now.AddSeconds(second))
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        return Ok("finish task");
    }
}
