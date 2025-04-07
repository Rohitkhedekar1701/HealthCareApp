using MimeKit;
using Notification.MailService;
using Notification.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StaffService.DTOs;
using System.Text;
using System.Text.Json;

namespace Notification
{
    public class MessageConsumer
    {
        private readonly SendMailService _sendMailService;

        public MessageConsumer(SendMailService sendMailService)
        {
            _sendMailService = sendMailService;
        }

        public void Start()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                    // Port = 5672 // default, can omit
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "staff_registered",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        await Task.Delay(5000); // 5 seconds delay just to see it in UI
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var staff = JsonSerializer.Deserialize<StaffRegistrationDto>(json);

                        if (staff != null && !string.IsNullOrEmpty(staff.Email))
                        {
                            var toAddresses = new List<MailboxAddress>
                            {
                                new MailboxAddress(staff.FirstName, staff.Email)
                            };

                            var message = new Message(toAddresses, "Welcome to the Team!", $"Hi {staff.FirstName}, welcome aboard! your Username: {staff.Username} and Password: {staff.Password}");
                            _sendMailService.sendEmail(message); // Make async if your service supports it
                        }
                        else
                        {
                            Console.WriteLine("[RabbitMQ] Received invalid staff data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[RabbitMQ Consumer ERROR] Failed to process message: {ex.Message}");
                    }
                };

                channel.BasicConsume(
                    queue: "staff_registered",
                    autoAck: true,
                    consumer: consumer
                );

                Console.WriteLine("[RabbitMQ] Consumer started and listening on 'staff_registered' queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMQ ERROR] Failed to connect or consume: {ex.Message}");
            }
        }
    }
}
