using AspNetCore.TaskScheduler.Hangfire.Data;
using AspNetCore.TaskScheduler.Hangfire.Services;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Config
builder.Services.AddDbContext<DataContext>(
options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnectionMysql")!));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddTransient<ITimeService, TimeService>();
#endregion

// Hangfire congif database
var connectionStringMysql = builder.Configuration.GetConnectionString("DefaultConnectionMysql");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    //.UseSqlServerStorage(connectionStringSqlServer, new SqlServerStorageOptions
    //{
    //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    //    QueuePollInterval = TimeSpan.Zero,
    //    UseRecommendedIsolationLevel = true,
    //    DisableGlobalLocks = true
    //})
    .UseStorage(new MySqlStorage(connectionStringMysql, new MySqlStorageOptions
    {
        QueuePollInterval = TimeSpan.FromSeconds(10),
        JobExpirationCheckInterval = TimeSpan.FromHours(1),
        CountersAggregateInterval = TimeSpan.FromMinutes(5),
        PrepareSchemaIfNecessary = true,
        DashboardJobListLimit = 25000,
        TransactionTimeout = TimeSpan.FromMinutes(1),
        TablesPrefix = "Hangfire_",
    })));
builder.Services.AddHangfireServer();

var app = builder.Build();

// UseHangfireDashboard phải có database trước , nếu không sẽ bị lỗi
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[]
    {
        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            RequireSsl = false,
            SslRedirect = false,
            LoginCaseSensitive = true,
            Users = new []
            {
                new BasicAuthAuthorizationUser
                {
                    Login = "admin",
                    PasswordClear =  "admin"
                }
            }

        })
    }

});
app.MapHangfireDashboard(); // /hangfire

// Hangfire run job
//RecurringJob.AddOrUpdate<ITimeService>("print-time", service => service.PrintNow(), Cron.MinuteInterval(1));// có thể dùng -> "* * * * * *"
RecurringJob.AddOrUpdate<ITimeService>("print-time", service => service.PrintNow(), "*/1 * * * * *");// có thể dùng -> "* * * * * *"

#region Congif
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion
