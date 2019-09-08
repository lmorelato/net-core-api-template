using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Template.Core.Services.Interfaces;
using Template.Core.Settings;

namespace Template.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailjetApiSettings mailjetApiSettings;

        public EmailService(IOptions<MailjetApiSettings> mailjetApiSettings)
        {
            this.mailjetApiSettings = mailjetApiSettings.Value;
        }

        public async Task SendTemplate()
        {
            var subject = "Activate your account";
            var toEmail = "lmorelato@gmail.com";
            string toName = null;

            var message = this.CreateMessage(subject, toEmail, toName);
            message.Add("TemplateID", 961145);
            message.Add("TemplateLanguage", true);
            message.Add("Variables", new JObject { { "confirmation_link", "https://www.uol.com.br/" } });

            var request = new MailjetRequest { Resource = Send.Resource };
            request.Property(Send.Messages, new JArray { message });

            var client = this.CreateMailjetClient();
            var response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($@"StatusCode: {response.StatusCode}");
                Console.WriteLine($@"ErrorInfo: {response.GetErrorInfo()}");
                Console.WriteLine(response.GetData());
                Console.WriteLine($@"ErrorMessage: {response.GetErrorMessage()}");
            }
        }

        private JObject CreateMessage(string subject, string toEmail, string toName)
        {
            var from = new JObject
            {
                { "Email", this.mailjetApiSettings.Sender },
                { "Name", this.mailjetApiSettings.SenderName }
            };

            var to = new JArray
            {
                new JObject
                {
                    { "Email", toEmail },
                    { "Name", toName }
                }
            };

            var message = new JObject
                          {
                              { "From", from },
                              { "To", to },
                              { "Subject", subject }
                          };

            return message;
        }

        private MailjetClient CreateMailjetClient()
        {
            var client = new MailjetClient(
                             this.mailjetApiSettings.ApiKey,
                             this.mailjetApiSettings.ApiSecret)
            { Version = ApiVersion.V3_1 };

            return client;
        }
    }
}