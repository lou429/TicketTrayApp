using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using TicketApp.Class;
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

        public LoadingScreen(EmailMsg emailMsg)
        {
            InitializeComponent();
            LoadingLabel.Content = "Sending ticket...";
            var answer = Email.SendEmailForm(emailMsg);
            LoadingLabel.Content = $"Result: {answer.Result.StatusCode}";
        }

        //public LoadingScreen(Form form)
        //{
        //    InitializeComponent();
        //    Thread.Sleep(200);
        //    LoadingLabel.Content = "Sending ticket...";
        //    var answer = SendEmail(form).Result;
        //    if (answer)
        //    {
        //        if (MessageBox.Show("Ticket submitted successfully", "Successful", MessageBoxButton.OK,
        //            MessageBoxImage.Information) == MessageBoxResult.OK)
        //        {

        //        }
        //            //Switcher.Switch(new MainPage());
        //    }
        //}
    }
}
