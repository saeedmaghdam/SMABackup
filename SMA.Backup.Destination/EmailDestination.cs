using System.Threading.Tasks;
using Google.Apis.Logging;
using SMA.Backup.Destination.Framework;
using SMA.Backup.Destination.Model;
using SMA.Backup.Util;
using SMA.Backup.Destination.Configuration;
using System.Net.Mail;

namespace SMA.Backup.Destination
{
    public class EmailDestination : IEmailDestination
    {
        private readonly ICommonUtil _commonUtil;
        private readonly ILogger _logger;
        private EmailConfiguration _configuration;

        public EmailDestination(ICommonUtil commonUtil)
        {
            _commonUtil = commonUtil;
        }

        public async Task<OutputModel> Upload(IDestinationConfiguration destinationConfiguration)
        {
            var result = new OutputModel();

            try
            {
                _configuration = destinationConfiguration as EmailConfiguration;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_configuration.MailServer);
                mail.From = new MailAddress(_configuration.From);
                foreach (var to in _configuration.To.Split(','))
                    mail.To.Add(to.Trim());
                if (!string.IsNullOrEmpty(_configuration.Cc))
                {
                    foreach (var cc in _configuration.Cc.Split(','))
                        mail.CC.Add(cc.Trim());
                }
                if (!string.IsNullOrEmpty(_configuration.Bcc))
                {
                    foreach (var bcc in _configuration.Bcc.Split(','))
                        mail.Bcc.Add(bcc.Trim());
                }
                mail.Subject = $"{_configuration.Name}_{System.IO.Path.GetFileName(_configuration.FileName)}";
                mail.Body = "Sent by SMABackupService!";

                mail.Attachments.Add(new Attachment(_configuration.FileName));

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_configuration.Username, _configuration.Password);
                SmtpServer.EnableSsl = _configuration.Ssl;

                SmtpServer.Send(mail);

                result.IsSuccessful = true;
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}
