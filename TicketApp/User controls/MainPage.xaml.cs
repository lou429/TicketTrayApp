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

namespace TicketApp.User_controls
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private readonly string _startingHead;
        private readonly string _startingBody;
        public UserInfo UserInfo;
        public Form Form;

        public MainPage()
        {
            InitializeComponent();
            _startingHead = HeaderField.Text;
            _startingBody = BodyField.Text;
            try
            {
                var data = new HandleFile();
                var dataInfo = data.Load<DataInfo>();
                UserInfo = dataInfo.UserInfo;
            }
            catch (Exception ex)
            {
                var _ = new Error(ex);
            }
        }

        private void Field_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox) sender).Text == _startingHead || ((TextBox) sender).Text == _startingBody)
            {
                ((TextBox) sender).Text = "";
                ((TextBox) sender).Foreground = Brushes.Black;
                ((TextBox) sender).BorderThickness = new Thickness(0, 0, 0, 3);
            }
        }

        private void Field_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox) sender).Text == "")
            {
                ((TextBox) sender).Text = ((TextBox) sender).TabIndex == 1 ? _startingHead : _startingBody;
                ((TextBox) sender).Foreground = Brushes.Gray;
                ((TextBox) sender).BorderThickness = new Thickness(0, 0, 0, 1);
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
                Switcher.Switch(new LoadingScreen(Form));
            }
        }

        private void SendButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((Border) sender).Margin = new Thickness(85, 16, 85, 76);
            SendButton.FontSize += 2;
            SendButton.Foreground = Brushes.White;
            ((Border) sender).Background = new SolidColorBrush(Color.FromRgb(0, 43, 113));
            ((Border) sender).BorderBrush = Brushes.Gray;
        }

        private void SendButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((Border) sender).Margin = new Thickness(90, 20, 90, 80);
            SendButton.FontSize -= 2;
            SendButton.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#4C4C4C"));
            ((Border) sender).Background = Brushes.White;
            ((Border) sender).BorderBrush = new SolidColorBrush(Color.FromRgb(0, 43, 113));
        }

        private void Settings_OhMouseEnter(object sender, MouseEventArgs e) =>
            ((Image) sender).Margin = new Thickness(68, 78, 68, 18);

        private void Settings_OhMouseLeave(object sender, MouseEventArgs e) =>
            ((Image) sender).Margin = new Thickness(70, 80, 70, 20);
    }
}