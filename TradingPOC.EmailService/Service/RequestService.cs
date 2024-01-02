using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using TradingPOC.EmailService.Model;

namespace TradingPOC.EmailService.Service
{
    public class RequestService : IRequestService
    {
        private ILogger<Worker> _Logger;
        public RequestService(ILogger<Worker> logger)
        { 
            _Logger = logger;
        }

        public bool HasError { get; private set; }

        public string ErrorDescription { get; private set; }

        public void ProcessNotificationEmailRequest(RequestModel? request)
        {
            _ResetError();
            try
            {
                string fromMail = "testermail12181@gmail.com";
                string password = "uwfderpiezyeihtj";
                string smtpHost = "smtp.gmail.com";
                int port = 587;

                request.TradeType = request.TradeType == "B" ? "Buy" : "Sell";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = $"Trade confirmation - {request.TradeType}";
                message.To.Add(new MailAddress(request.UserEmail));
                message.Body = _GetMailTemplate();

                #region htmlFormatting
                message.Body = string.Format(message.Body, request.UserName, request.TradeType, request.ScriptName,
                    request.OrderId, request.Quantity, request.Price);
                #endregion

                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = port,
                    Credentials = new NetworkCredential(fromMail, password),
                    EnableSsl = true,
                };
                smtpClient.Send(message);
                smtpClient.Dispose();

            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorDescription = ex.Message;
                _Logger.LogError("Error in sending mail");
            }
        }

        private void _ResetError()
        {
            HasError = false;
            ErrorDescription = string.Empty;
        }

        private string _GetMailTemplate()
        {
            //MailReceiptTemplate
            string html = "";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"MailTemplate\MailReceiptTemplate.html");
            using (StreamReader reader = new StreamReader(path))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }
    }
}
