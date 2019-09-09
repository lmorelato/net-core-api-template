using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Serilog;

using Template.Core.Exceptions;
using Template.Core.Models;
using Template.Core.Services.Interfaces;
using Template.Core.Settings;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public class MailjetService : IMailjetService
    {
        private readonly ILogger<MailjetService> logger;
        private readonly ISharedResources localizer;
        private readonly MailjetApiSettings mailjetApiSettings;

        public MailjetService(
            IOptions<MailjetApiSettings> mailjetApiSettings,
            ILogger<MailjetService> logger,
            ISharedResources localizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mailjetApiSettings = mailjetApiSettings.Value;
        }

        public async Task Send(EmailSettings settings, bool throwIfError = false)
        {
            var body = this.CreateBody(settings.Subject, settings.ToEmail, settings.ToName);
            if (settings.TemplateId.GetValueOrDefault() > 0)
            {
                body.Add(Constants.Mailjet.TemplateID, settings.TemplateId);
                body.Add(Constants.Mailjet.TemplateLanguage, true);
            }

            var variables = new JObject();
            foreach (var (key, value) in settings.Variables)
            {
                variables.Add(key, value);
            }

            body.Add(Constants.Mailjet.Variables, variables);

            var request = new MailjetRequest { Resource = Mailjet.Client.Resources.Send.Resource };
            request.Property(Mailjet.Client.Resources.Send.Messages, new JArray { body });

            var client = this.CreateMailjetClient();
            var response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var message = this.localizer.GetAndApplyValues("MailjetError", settings.Subject, settings.ToEmail);
                var emailSenderException = new MailjetException(message);
                this.logger.LogError(emailSenderException, response.GetData().ToString());

                if (throwIfError)
                {
                    throw emailSenderException;
                }
            }
        }

        private JObject CreateBody(string subject, string toEmail, string toName)
        {
            var from = new JObject
            {
                { Constants.Mailjet.Email, this.mailjetApiSettings.Sender },
                { Constants.Mailjet.Name, this.mailjetApiSettings.SenderName }
            };

            var to = new JArray
            {
                new JObject
                {
                    { Constants.Mailjet.Email, toEmail },
                    { Constants.Mailjet.Name, toName }
                }
            };

            var message = new JObject
            {
                { Constants.Mailjet.From, from },
                { Constants.Mailjet.To, to },
                { Constants.Mailjet.Subject, subject }
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