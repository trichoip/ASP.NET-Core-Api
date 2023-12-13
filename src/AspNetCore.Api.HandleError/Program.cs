using AspNetCore.Api.HandleError.Exceptions;
using AspNetCore.Api.HandleError.Extensions;
using AspNetCore.Api.HandleError.Filters;
using AspNetCore.Api.HandleError.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;

namespace AspNetCore.Api.HandleError;

public class Program
{
    /* 
     * + thứ tự cách xử lý exception (debug để hiểu rõ luồng đi)
     * - bất cứ method nào thì: 
     * - trước khi vào method thì vào ErrorHandlerMiddleware.cs trước
     * - kết thúc method thì có 2 trường hợp: 
     *  1. không có lỗi nào (không có throw): khi kết thúc method thì vào HttpResponseExceptionFilter.cs -> ErrorHandlerMiddleware.cs
     *  2. có lỗi (có throw): khi kết thúc method thì vào HttpResponseExceptionFilter.cs -> ExceptionHandlingFilterAttribute.cs -> ErrorHandlerMiddleware.cs
     *      - nếu trong HttpResponseExceptionFilter.cs đã xử lý hết exception thì không vào ExceptionHandlingFilterAttribute.cs mà nhảy qua ErrorHandlerMiddleware.cs
     *      - trong trường hợp 2 thì nếu đã xữ lý exception trong file nào thì trong file sau khi không xữ lý exception nữa
     *      ví dụ: trong HttpResponseExceptionFilter.cs đã xữ lý exception thì trong ExceptionHandlingFilterAttribute.cs không xữ lý exception nữa
     *      ví dụ 2: trong HttpResponseExceptionFilter.cs không xữ lý exception thì trong ExceptionHandlingFilterAttribute.cs xữ lý exception và ErrorHandlerMiddleware.cs không xữ lý exception nữa
     *      
     * - nếu mà HttpResponseExceptionFilter , ExceptionHandlingFilterAttribute , ErrorHandlerMiddleware đều không xữ lý exception thì nó sẽ vào UseExceptionHandler
     */
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        builder.Services.AddControllers(options =>
        {
            #region Handle error cach 2
            options.Filters.Add<HttpResponseExceptionFilter>();
            #endregion
            #region Handle error cach 5
            options.Filters.Add<ExceptionHandlingFilterAttribute>();
            #endregion
        });

        // những return error như NotFound(), BadRequest(), Unauthorized(), Forbidden(),..
        // nếu không truyền object thì nó áp dụng theo AddProblemDetails bên dưới
        // là tất cả sẽ có thêm extension nodeId = Environment.MachineName
        // còn return throw new ... thì nó đưa tất cả thông tin vào ProblemDetails json
        // và gồm cả field extension exception rất dài
        // nếu không muốn nó đưa ra extension exception thì thêm app.UseExceptionHandler();
        // nếu dùng IProblemDetailsService thì cần phải có AddProblemDetails()
        // còn nếu dùng ProblemDetailsFactory thì không cần AddProblemDetails()
        builder.Services.AddProblemDetails(options =>
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.Add("nodeId", Environment.MachineName);
                });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseDeveloperExceptionPage();
            app.UseExceptionHandler();

            #region Handle error cach 4
            app.UseExceptionApplication();
            app.UseException();
            // 2 cách dưới giống nhau, chọn 1 trong 2
            // nếu áp dụng cả 2 thì nó sẽ vào cái 2 trước nếu trong (2) không xữ lý exception thì nó sẽ vào cái (1)
            app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler()); // (1)
            app.UseExceptionHandler(exceptionHandlerApp => // (2)
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    // IProblemDetailsService: requied AddProblemDetails() 
                    if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        var exceptionType = exceptionHandlerFeature?.Error;

                        switch (exceptionType)
                        {
                            case Demo2Exception e:
                                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                                break;
                            default:
                                throw new DemoException("DemoException_AddProblemDetails");
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                break;
                        }

                        await problemDetailsService.WriteAsync(new ProblemDetailsContext
                        {
                            HttpContext = context,
                            ProblemDetails =
                            {
                                //Title = title,
                                //Type = type,
                                Detail = exceptionType?.Message

                            }
                        });
                    }
                });
            });
            #endregion

            #region Handle error cach 1
            // cách này không nên sài
            // UseExceptionHandler chỉ cấu hình 1 lần , nếu cấu hình nhiều thì nó lấy cái cuối cùng
            //app.UseExceptionHandler("/error");
            #endregion
        }

        #region Handle error cach 3
        //app.UseMiddleware<ErrorHandlerMiddleware>();
        //app.UseMiddleware<ExceptionHandler2Middleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        #endregion

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}