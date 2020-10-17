using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp
{
    public class Form
    {
        private string Heading { get; set; }
        private string Body { get; set; }

        private string ComputerName { get; }

        public Form(string heading, string body)
        {
            Heading = heading;
            Body = body;
            ComputerName = Environment.MachineName;
        }
    }
}
