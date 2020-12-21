using System;
using System.Collections.Generic;
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
using TicketApp.Class;

namespace TicketApp.User_controls
{
    /// <summary>
    /// Interaction logic for DeviceInfo.xaml
    /// </summary>
    public partial class DeviceInfo : UserControl
    {
        private readonly string _startingHead;
        private readonly string _startingBody;

        public DeviceInfo()
        {
            InitializeComponent();
            _startingHead = HeaderField.Text;
            _startingBody = BodyField.Text;
        }
        
        private void FieldModifyState(object sender, RoutedEventArgs e)
        {
            var send = ((TextBox) sender);
            if (send.Text == _startingHead || send.Text == _startingBody || send.Text == "")
            {
                bool state = send.Text == "";
                send.Text = state ? (send.TabIndex == 1 ? _startingHead : _startingBody) : "";
                send.Foreground = state ? Brushes.Gray : Brushes.Black;
                send.BorderThickness = new Thickness(0, 0, 0, state ? 1 : 3);
            }
        }


        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Switcher.Switch(new Settings());
        }

        private void SendTicket(object sender, MouseButtonEventArgs e)
        {
            if (HeaderField.Text == "" || HeaderField.Text == _startingHead || BodyField.Text == "" ||
                BodyField.Text == _startingBody)
            {
                Switcher.Switch(new LoadingScreen(new EmailMsg(HeaderField.Text, BodyField.Text)));
            }
        }

        void ButtonModifyState(object sender, MouseEventArgs e)
        {
            var border = ((Border) sender);
            bool state = border.IsFocused; //ENTER | LEAVE
            border.Margin = state ? new Thickness(85, 16, 85, 76) : new Thickness(90, 20, 90, 80);
            border.Background = state ? new SolidColorBrush(Color.FromRgb(0, 43, 113)) : Brushes.White;
            border.BorderBrush = state ? Brushes.Gray : new SolidColorBrush(Color.FromRgb(0, 43, 113));
            SendButton.FontSize += state ? 2 : -2;
            SendButton.Foreground = state ? Brushes.White : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C4C4C"));

        }

        private void SettingsModifyState(object sender, MouseEventArgs e)
        {
            var thicknessArr = new Thickness[]
            {
                new Thickness(68, 78, 68, 18),
                new Thickness(70, 80, 70, 20)
            };

            ((Image) sender).Margin = ((Image) sender).Margin == thicknessArr[0] ? thicknessArr[1] : thicknessArr[0];
        }
    }
}
