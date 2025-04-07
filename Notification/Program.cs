using Notification.MailService;
using Notification.Models;
namespace Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfigurations>();

            builder.Services.AddSingleton(emailConfig);


            builder.Services.AddSingleton<SendMailService>();        // Your custom service to send emails
            builder.Services.AddSingleton<MessageConsumer>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();



            // Start the RabbitMQ Consumer
            using (var scope = app.Services.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<MessageConsumer>();
                consumer.Start();
            }


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
