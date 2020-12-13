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
using TicketApp.User_controls;
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
        public Form Form;

        public MainWindow()
        {
            Switcher.Initialize(this);
            InitializeComponent();
            CheckRunningApp();

            Switcher.Switch(new MainPage());
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
                firstProc.StartInfo.FileName = "TrayIcon.exe";
                firstProc.EnableRaisingEvents = true;
                //firstProc.Start();
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
        
        /// <summary>
        /// Handle navigation through the app
        /// </summary>
        /// <param name="newPage"></param>
        public void Navigate(UserControl newPage)
        {
            this.Content = newPage;
        }
    }
}
