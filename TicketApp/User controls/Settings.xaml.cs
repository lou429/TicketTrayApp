using System;
using System.Windows.Controls;
using System.Windows.Input;
using TicketApp.User_controls;

namespace TicketApp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            DeviceNameLabel.Content = Environment.MachineName;
        }

        private void HandleNavigation(object sender, MouseButtonEventArgs e) => Switcher.Switch(getUserControl(((Label)sender).Content.ToString().ToCharArray()[0]));

        private UserControl getUserControl(char let)
        {
            return let switch
            {
                'D' => new DeviceInfo(), //Device Info
                'R' => new ReportProblem(), //Report a problem 
                'S' => new UserControl(), //Advanced Settings
                'C' => Switcher.LastPage(), //Close
                _ => null,
            };
        }
    }
}
