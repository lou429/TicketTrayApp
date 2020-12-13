using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SendGrid;
using SendGrid.Helpers.Mail;
using WPFCustomMessageBox;

namespace TicketApp.User_controls
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : UserControl
    {
        public LoadingScreen(string val = "")
        {
            InitializeComponent();
            if (val != "")
                LoadingLabel.Content = val;
        }

        public LoadingScreen(Form form)
        {
            InitializeComponent();
            LoadingLabel.Content = "Sending ticket...";
            var answer = SendEmail(form).Result;
            if (answer)
            {
                if(MessageBox.Show("Ticket submitted successfully", "Successful", MessageBoxButton.OK,
                    MessageBoxImage.Information) == MessageBoxResult.OK)
                    Switcher.Switch(new MainPage());
            }
        }

        private async Task<bool> SendEmail(Form form)
        {
            try
            {
                var result = await SendEmailForm(form);
                if (result.StatusCode == HttpStatusCode.Accepted)
                {
                    LoadingLabel.Content = "Sent successfully";
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Message failed to send\nEnsure you are connected to the internet",
                    "Message send error", MessageBoxButton.OK, MessageBoxImage.Error);
                var _ = new Error(ex);
                return false; 
            }
        }

        private async Task<Response> SendEmailForm(Form form)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(form.getFromAddress(), form.getUserName());
            var to = new EmailAddress(form.getToAddress());
            var subject = form.getHeading();
            var body = form.getBody();
            var htmlBody = $"<strong>{form.getBody()}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, htmlBody);
            return await client.SendEmailAsync(msg);
        }
    }
}
