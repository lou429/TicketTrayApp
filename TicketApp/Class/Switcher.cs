using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TicketApp
{
    public static class Switcher
    {
        public static MainWindow PageSwitcher;
        internal static List<UserControl> History;

        public static void Switch(UserControl nextPage)
        {  
            History.Add(nextPage);
            PageSwitcher.Navigate(nextPage);
        }

        public static void Initialize(MainWindow obj)
        {
            History = new List<UserControl>();
            PageSwitcher = obj;
        }

        public static UserControl LastPage() => History[History.Count - 1];
    }
}
