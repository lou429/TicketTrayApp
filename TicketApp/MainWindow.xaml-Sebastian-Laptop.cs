using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using FontAwesome.WPF;
using SendGrid;
using SendGrid.Helpers.Mail;
using WPFCustomMessageBox;

namespace TicketApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _startingHead;
        private readonly string _startingBody;
        public UserInfo UserInfo;
        public List<Form> Form;
        private int LastScreen;
        private int CurrentScreen;

        public MainWindow()
        {
            try
            {
                var data = new HandleFile();
                var dataInfo = data.Load<DataInfo>();
                UserInfo = dataInfo.UserInfo;
                Form = dataInfo.Form;
            }
            catch (Exception ex)
            {
                var _ = new Error(ex);
            }

            InitializeComponent();
            SwitchScreen(3);
            DeviceNameLabel.Content = Environment.MachineName;
            _startingHead = HeaderField.Text;
            _startingBody = BodyField.Text;
            CheckRunningApp();
            SwitchScreen(1);
        }

        /// <summary>
        /// Switches Screen
        /// </summary>
        /// <param name="screen">1 Default Info screen
        /// 2 Settings screen
        /// 3 Loading screen
        /// </param>
        private void SwitchScreen(int screen)
        {
            LastScreen = CurrentScreen;
            DefaultInfo.Visibility = screen == 1 ? Visibility.Visible : Visibility.Hidden;
            SettingsScreen.Visibility = screen == 2 ? Visibility.Visible : Visibility.Hidden;
            LoadingScreen.Visibility = screen == 3 ? Visibility.Visible : Visibility.Hidden;
            CurrentScreen = screen;
        }

        private void SwitchLastScreen()
        {
            SwitchScreen(LastScreen);
        }

        /// <summary>
        /// Check for any processes that match this one 
        /// </summary>
        private void CheckRunningApp()
        {
            var processesArr =  CheckRunningProcess(Process.GetCurrentProcess().ProcessName);
            if (processesArr.Length > 1)
            {
                var result = CustomMessageBox.ShowOKCancel("This program is already running on your computer",
                    "Process already running", "Kill process", "Cancel", MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                    KillProcess(processesArr);
                else if (result == MessageBoxResult.Cancel)
                    KillCurrentProcess(processesArr);
            }

            if(CheckRunningProcess("Millennium Tray App").Length < 1)
            {
                Process firstProc = new Process();
                firstProc.StartInfo.FileName = "TrayIcon.exe";
                firstProc.EnableRaisingEvents = true;
                //firstProc.Start();
            }
        }

        private Process[] CheckRunningProcess(string name) => Process.GetProcessesByName(name);

        /// <summary>
        /// Kill any process running with the same process name 
        /// </summary>
        /// <param name="processes">Process array to kill</param>
        private void KillProcess(Process[] processes)
        {
            var id = Process.GetCurrentProcess().Id;
            foreach (var proc in processes)
                if (proc.Id != id)
                    proc.Kill();
        }

        private void KillCurrentProcess(Process[] processes)
        {
            var id = Process.GetCurrentProcess().Id;
            foreach (var proc in processes)
                if (proc.Id == id)
                    proc.Kill();
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == _startingHead || ((TextBox)sender).Text == _startingBody)
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Foreground = Brushes.Black;
                ((TextBox)sender).BorderThickness = new Thickness(0, 0, 0, 3);
            }
        }

        private void Field_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = ((TextBox)sender).TabIndex == 1 ? _startingHead : _startingBody;
                ((TextBox)sender).Foreground = Brushes.Gray;
                ((TextBox)sender).BorderThickness = new Thickness(0, 0, 0, 1);
            }
        }

        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchScreen(2);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsScreen.Visibility = Visibility.Hidden;
            DefaultInfo.Visibility = Visibility.Visible;
        }

        private async void SendTicket(object sender, MouseButtonEventArgs e)
        {
            if (HeaderField.Text == "" || HeaderField.Text == _startingHead || BodyField.Text == "" ||
                BodyField.Text == _startingBody)
            {
                
            }
            else
            {
                var form = new Form(HeaderField.Text, BodyField.Text);
                SwitchScreen(3);
                LoadingLabel.Content = "Sending ticket...";
                CloseButton.Visibility = Visibility.Hidden;
                BackButton.Visibility = Visibility.Visible;
                BackButton.Content = "Cancel";
                SaveFile(form);
                try
                {
                    var result = await SendEmail(form);
                    var output = await MessageSentDialog(result.StatusCode == HttpStatusCode.Accepted);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Message failed to send\nEnsure you are connected to the internet",
                        "Message send error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task<bool> MessageSentDialog(bool result)
        {
            try
            {
                var timeNow = DateTime.Now.Second;
                if (timeNow > 54)
                    timeNow = 0;

                var timeToCheck = timeNow + 5;

                var tempIcon = RotatingIcon;
                RotatingIcon.Visibility = Visibility.Hidden;
                RotatingIcon.Icon = result ? FontAwesomeIcon.Check : FontAwesomeIcon.Times;
                RotatingIcon.Spin = false;
                RotatingIcon.Visibility = Visibility.Visible;
                var tempLabel = LoadingLabel;
                LoadingLabel.Content =
                    result ? "Ticket sent successfully!" : "Message sent failed\nCheck internet connection";

                while (timeNow <= timeToCheck)
                    timeNow = DateTime.Now.Second;
                BackButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
                //CloseButton.Content = "Exit";

                RotatingIcon = tempIcon;
                LoadingLabel = tempLabel;
                return true;
            }
            catch (Exception ex)
            {
                var _ = new Error(ex);
                return false; 
            }
        }

        private async Task<Response> SendEmail(Form form)
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

        internal void SaveFile(Form form)
        {
            if (!new HandleFile().Save(form))
                SaveFile("Failed to save data file");
        }

        internal void SaveFile(string exception)
        {
            if (!new HandleFile().Save(exception))
                Debug.WriteLine("Recursion error");
        }

        private void SendButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Margin = new Thickness(85, 16, 85, 76);
            SendButton.FontSize += 2;
            SendButton.Foreground = Brushes.White;
            ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(0, 43, 113));
            ((Border)sender).BorderBrush = Brushes.Gray;
        }

        private void SendButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Margin = new Thickness(90, 20, 90, 80);
            SendButton.FontSize -= 2;
            SendButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C4C4C"));
            ((Border)sender).Background = Brushes.White;
            ((Border)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(0, 43, 113));
        }
        
        private void Settings_OhMouseEnter(object sender, MouseEventArgs e) => ((Image)sender).Margin = new Thickness(68, 78, 68, 18);

        private void Settings_OhMouseLeave(object sender, MouseEventArgs e) => ((Image)sender).Margin = new Thickness(70, 80, 70, 20);

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            SwitchLastScreen();
        }
    }
}
