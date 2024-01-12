using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace FND.API.Services
{
    public class NotificationService
    {
        private GmailService GmailService { get; set; }
        string[] Scopes = { GmailService.Scope.MailGoogleCom };
        string ApplicationName = "Fake News Detection System";
        string SenderEmail = "fakenewsdetectionsystemfit@gmail.com";

        public NotificationService()
        {
            UserCredential credential;
            using (var stream = new FileStream("GmailKey.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "gmail-dotnet-credentials.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            // Create Gmail API service.
            GmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        public async Task SendMailAsync(Notification message)
        {
            try
            {
                Message InternalMessage=GetSendMessage(message);
                await GmailService.Users.Messages.Send(InternalMessage, "me").ExecuteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Message GetSendMessage(Notification gmailMessage)
        {
            string plainText = $"From:{ApplicationName}<{SenderEmail}>\r\n" +
                               $"To:{GenerateReceipents(gmailMessage.TO)}\r\n" +
                               $"Bcc:{GenerateReceipents(gmailMessage.Bcc)}\r\n" +
                               $"Subject:{gmailMessage.Subject}\r\n" +
                               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                               $"{gmailMessage.HtmlBody}";

            Message message = new Message();
            message.Raw = Encode(plainText.ToString());
            return message;
        }
        private string GenerateReceipents(IEnumerable<string> receipents)
        {
            return receipents == null ? string.Empty : string.Join(",", receipents);
        }
        private string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
    public class Notification
    {
        public string Subject { get; set; }
        public IEnumerable<string> TO { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public string HtmlBody { get; set; }

        public Notification(string subject, IEnumerable<string> to, string htmlBody)
        {
            Subject = subject;
            TO = to;
            HtmlBody = htmlBody;
            Bcc = null;
        }
        public Notification(string subject, IEnumerable<string> bcc, IEnumerable<string> to, string htmlBody)
        {
            Subject = subject;
            TO = to;
            Bcc = bcc;
            HtmlBody = htmlBody;
        }

    }
}
