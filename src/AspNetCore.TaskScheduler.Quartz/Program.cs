using AspNetCore.TaskScheduler.Quartz.Scheduler;
using AspNetCore.TaskScheduler.Quartz.Scheduler.Jobs;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuartz(options =>
{
    //options.UsePersistentStore(options =>
    //{
    //    // options.UseProperties = true;
    //    options.UsePostgres(builder.Configuration["Quartz:ConnectionString"] ?? "");
    //    options.UseNewtonsoftJsonSerializer();
    //});

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

#region Config job cach 2
builder.Services.ConfigureOptions<LoggingBackgroundJobSetup>();
#endregion

//builder.Services.AddQuartzHostedService(options =>
//{
//    options.WaitForJobsToComplete = true;
//});

builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
