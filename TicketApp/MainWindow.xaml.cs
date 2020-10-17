using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


        /// <summary>
        /// Encapsulation to store both sets of data in one file
        /// </summary>
        struct DataInfo
        {
            internal UserInfo UserInfo { get; set; }
            internal List<Form> Form { get; set; }

            public DataInfo(UserInfo userInfo, List<Form> form)
            {
                UserInfo = userInfo;
                Form = form;
            }
        }

        public MainWindow()
        {
            bool? result = Data<DataInfo>.CheckDir();
            if (result == true)
            {
                try
                {
                    DataInfo dataInfo = Data<DataInfo>.Load();
                    UserInfo = dataInfo.UserInfo;
                    Form = dataInfo.Form;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else if (result == false)
            {
                DataInfo dataInfo = new DataInfo();
                dataInfo.UserInfo = new UserInfo();
                dataInfo.Form = new List<Form>();
                Data<DataInfo>.Save(dataInfo);
            }
            else
                MessageBox.Show("Cannot create data file", "Error", MessageBoxButton.OK);


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
            DefaultInfo.Visibility = screen == 1 ? Visibility.Visible : Visibility.Hidden;
            SettingsScreen.Visibility = screen == 2 ? Visibility.Visible : Visibility.Hidden;
            LoadingScreen.Visibility = screen == 3 ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Check for any processes that match this one 
        /// </summary>
        private void CheckRunningApp()
        {
            var processesArr =  CheckRunningProcces(Process.GetCurrentProcess().ProcessName);
            if (processesArr.Length > 1)
            {
                var result = CustomMessageBox.ShowOKCancel("This program is already running on your computer",
                    "Process already running", "Kill process", "Cancel", MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                    KillProcess(processesArr);
                else if (result == MessageBoxResult.Cancel)
                    KillCurrentProcess(processesArr);
            }

            if(CheckRunningProcces("Millennium Tray App").Length < 1)
            {
                Process firstProc = new Process();
                firstProc.StartInfo.FileName = "TrayIcon";
                firstProc.EnableRaisingEvents = true;
                firstProc.Start();
            }
        }

        private Process[] CheckRunningProcces(string name) => Process.GetProcessesByName(name);

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

        private void SendTicket(object sender, MouseButtonEventArgs e)
        {
            if (HeaderField.Text == "" || HeaderField.Text == _startingHead || BodyField.Text == "" ||
                BodyField.Text == _startingBody)
            {
                var form = new Form(HeaderField.Text, BodyField.Text);
                if(Form == null)
                    Form = new List<Form>();
                Form.Add(form);
                var newDataInfo = new DataInfo(UserInfo, Form);
                SwitchScreen(3);
                Data<DataInfo>.Save(newDataInfo);
                //SendEmail(form);
            }
        }

        private void SendEmail(Form form)
        {
            throw new NotImplementedException();
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
    }
}
