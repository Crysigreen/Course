
using Course.Services.Contarcts;
using Course.Services;
using Course.Data;
using Microsoft.EntityFrameworkCore;
using Courses.Hubs;

namespace Course
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("OnlineCoursesDB"));

            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddSignalR();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                //dbContext.Database.EnsureCreated();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Маршрут для SignalR
            app.MapHub<PaymentHub>("/paymentHub");

            // Маршрут по умолчанию на index.html
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
