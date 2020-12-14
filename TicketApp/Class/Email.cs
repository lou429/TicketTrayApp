using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TicketApp.Class
{
    class Email
    {
        public Email(EmailMsg emailMsg)
        {

        }
        public static async Task<Response> SendEmailForm(EmailMsg emailMsg)
        {
            var client = new SendGridClient(APIKeys.SendGridKey);
            var from = new EmailAddress(EmailInfo.From);
            var to = new EmailAddress(EmailInfo.To);
            var subject = emailMsg.GetHeading();
            var body = $"{EmailInfo.UserName}\n{EmailInfo.ComputerName}\n{EmailInfo.UserEmail}\n{emailMsg.GetBody()}";
            var htmlBody = $"<strong>{body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, htmlBody);
            return await client.SendEmailAsync(msg);
        }
    }

    public class EmailMsg
    {
        private string Heading { get; set; }
        private string Body { get; set;  }

        public EmailMsg(string heading, string body)
        {
            Body = body;
            Heading = heading;
        }
        
        public EmailMsg()
        {

        }

        public string GetHeading() => Heading; 
        public void SetHeading(string val) => Heading = val;
        public string GetBody() => Body;
        public void SetBody(string val) => Body = val;
    }

    public static class EmailInfo
    {
        public static string To { get => APIKeys.EmailToAddress; set => To = value; }
        public static string From { get => APIKeys.EmailFromAddress; set => From = value; }
        public static string ComputerName = Environment.MachineName;
        public static string UserName = Environment.UserName;
        public static string UserEmail = Properties.Settings.Default.UserName; //How to call system vars - save user info as as system var


    }

}
