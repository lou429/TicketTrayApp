using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace TrayIcon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartTicketApp(object sender, RoutedEventArgs routedEventArgs)
        {
            Process firstProc = new Process();
            firstProc.StartInfo.FileName = "TicketApp.exe";
            firstProc.EnableRaisingEvents = true;

            firstProc.Start();
        }

        private void ShutdownApp(object sender, RoutedEventArgs e) => Environment.Exit(0);
    }
}
