using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace TicketApp
{
    public class UserInfo
    {
        public string FirstName;
        public string LastName;
        public string Email;
        public List<int> Number;

        public UserInfo()
        {
        }

        public UserInfo(string firstName, string lastName, string email = null, List<int> number = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email ?? "";
            Number = number ?? new List<int>();
        }
    }

    public class ProgramDir
    {
        internal static readonly string FolderPathTicketCreator =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Millennium",
                "Ticket Creator");

        internal static readonly string ProgramFilesLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Millennium");

        public static readonly string TicketCreatorLocation = Path.Combine(ProgramFilesLocation, "Ticket creator", "Ticket creator.exe");

        public static readonly string TrayAppLocation = Path.Combine(ProgramFilesLocation, "System tray", "TrayApp.exe");

        public static string DataFile => CheckPath("data.json");

        public static string ErrorFile => CheckPath("error.json");

        internal static string CheckPath(string filePath)
        {
            string dir = Path.Combine(FolderPathTicketCreator, filePath);
            return CheckDir(dir) == true ? dir : "";
        }

        internal static bool? CheckDir(string file = "")
        {
            if (File.Exists(file == "" ? DataFile : file))
                return true;
            else
            {
                try
                {
                    File.Create(file == "" ? DataFile : file);
                    return false;
                }
                catch (Exception ex)
                {
                    Error _ = new Error(ex);
                    Environment.ExitCode = 0;
                    return null;
                }
            }
        }
    }
}
