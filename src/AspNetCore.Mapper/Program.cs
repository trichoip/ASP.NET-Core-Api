namespace AspNetCore.Mapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // AutoMapper DI
            // lưu ý: nếu viết Profile cùng 1 project với program.cs thì có dùng AppDomain.CurrentDomain.GetAssemblies()
            // còn nếu không cùng 1 project thì khi dùng AppDomain.CurrentDomain.GetAssemblies() sẽ không tìm thấy Profile này nó chỉ tìm trong project hiện tại thôi
            // để add config profile ở project khác thì có 2 cách:
            // cách 1: trong project hiện tại thì dùng builder.Services.AddAutoMapper(typeof(MapperProfiles)); MapperProfiles là profile ở project khác
            // cách 2: trong project khác viết Extensions DependencyInjection services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); sau đó app Extensions ở prject khác vào project hiện tại
            // ví dụ: trong project khác viét Extensions AddApplication trong đó có config services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); sau đó quay lại project hiện tại thêm builder.Services.AddApplication();
            //builder.Services.AddApplication();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}