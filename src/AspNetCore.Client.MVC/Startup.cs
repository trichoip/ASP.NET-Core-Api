using AspNetCore.Client.MVC.Data;
using AspNetCore.Client.MVC.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetCore.Client.MVC;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddDbContext<ETransportationSystemContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ETransportationSystemContext") ?? throw new InvalidOperationException("Connection string 'ETransportationSystemContext' not found.")));

        services.AddSignalR();

        services.AddSession(options =>
        {
            //options.IdleTimeout = TimeSpan.FromMinutes(0.1); // Thời gian hết hạn phiên làm việc
            options.IdleTimeout = TimeSpan.FromSeconds(120); // Thời gian hết hạn phiên làm việc
            options.Cookie.Name = "trichoip-cookies";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseStaticFiles();

        // Lưu ý: app.UseSession() phải được đặt sau app.UseRouting() và trước app.UseEndpoints()
        // để đảm bảo phiên làm việc hoạt động chính xác trong quá trình xử lý yêu cầu.
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapGet("/", async context =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            //endpoints.MapControllerRoute(
            //    name: "controller / action",
            //    pattern: "{controller=home}/{action=index}"
            //    );
            endpoints.MapControllerRoute(
               name: "default123",
               // {id?} khong co dau ? thi bat buoc phai co id khi truyen ve controller , action,
               // con neu co ? thi khong bat buoc truyen id
               pattern: "{controller=account}/{action=index}/{id?}"
               );

            endpoints.MapHub<ChatHub>("/chatHub");
        });
        // app.MapHub<SignalrServer>("/signalrServer");
    }
}
