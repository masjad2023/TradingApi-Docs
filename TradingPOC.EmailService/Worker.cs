using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPOC.EmailService.Model;
using TradingPOC.EmailService.Model.Settings;
using TradingPOC.EmailService.Service;

namespace TradingPOC.EmailService
{
    public class Worker : BackgroundService
    {
        private ConnectionFactory _ConnectionFactory;
        private readonly RabbitMQSettings _RabbitMQSettings;
        private ILogger<Worker> _Logger;
        private IRequestService _RequestService;

        public Worker(IRequestService requestService, RabbitMQSettings rabbitMQSettings, ILogger<Worker> logger)
        {
            _RequestService = requestService;
            _RabbitMQSettings = rabbitMQSettings;
            _ConnectionFactory = new ConnectionFactory();// { HostName = "localhost" };
            _ConnectionFactory.HostName = _RabbitMQSettings.BusSettings.Host;
            _ConnectionFactory.Port = _RabbitMQSettings.BusSettings.Port;
            _ConnectionFactory.UserName = _RabbitMQSettings.BusSettings.UserName;
            _ConnectionFactory.Password = _RabbitMQSettings.BusSettings.Password;
            _ConnectionFactory.VirtualHost = _RabbitMQSettings.BusSettings.VirtualHost;
            _Logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _Logger.LogInformation("Application Started");

            using var connection = _ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            string queueName = _RabbitMQSettings.BusSettings.QueueName;

            //channel.QueueDeclare(queueName);
            //channel.BasicQos(prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            

            consumer.Received += (model, ea) =>
            {
                string response = string.Empty;
                var body = ea.Body.ToArray();
                RequestModel? requestModel = null;

                try
                {
                    _Logger.LogInformation("Request being processed");
                    var message = Encoding.UTF8.GetString(body);
                    requestModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestModel>(message);

                    _RequestService.ProcessNotificationEmailRequest(requestModel);

                    if (_RequestService.HasError)
                    {
                        _Logger.LogError("Error in processing the request.\n" + _RequestService.ErrorDescription);
                    }

                }
                catch (Exception ex)
                {
                    _Logger.LogError("Error in processing request;\n" + ex.ToString());
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    //Process error request
                }
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" [x] Awaiting requests");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            return Task.CompletedTask;
        }
    }
}
