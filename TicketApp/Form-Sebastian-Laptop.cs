using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TicketApp
{
    public class Form
    {
        //private string To { get => "Techsupport@millennium.ltd.uk"; set => From = value; }
        private string To { get => "sebastianb@millennium.ltd.uk"; set => From = value; }
        private string From { get => "sebastianb@millennium.ltd.uk"; set => From = value; }

        private string UserName { get; set; }
        private string Heading { get; set; }
        private string Body { get; set; }

        private string ComputerName { get; }

        public Form(string heading, string body)
        {
            //var outlookData = new OutlookData();
            UserName = Environment.UserName;
            Heading = heading;
            Body = body;
            ComputerName = Environment.MachineName;
        }

        public Form(string userName, string heading, string body)
        {
            UserName = userName;
            Heading = heading;
            Body = body;
            ComputerName = Environment.MachineName;
        }

        public string getHeading() => Heading;
        public string getBody() => Body;

        public string getFromAddress() => From;

        public string getToAddress() => To;

        public string getUserName() => UserName;

    }
}
