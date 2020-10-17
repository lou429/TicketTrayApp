using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public List<string> Email;
        public List<int> Number;

        public UserInfo()
        {
        }

        UserInfo(string firstName, string lastName, List<string> email = null, List<int> number = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email ?? new List<string>();
            Number = number ?? new List<int>();
        }
    }

    static class Data<T>
    {
        private static readonly string FolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Millennium",
                "Ticket Creator");
        private static readonly string JsonFile = Path.Combine(FolderPath, "data.json");

        internal static bool? CheckDir(string file = "")
        {
            if (File.Exists(file == "" ? JsonFile : file))
                return true;
            else
            {
                try
                {
                    File.Create(file == "" ? JsonFile : file);
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    string errorFile = Path.Combine(FolderPath, "error.log");
                    if (CheckDir(errorFile) == null)
                    {
                        MessageBox.Show("Recursion Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.ExitCode = 0;
                    }
                    
                    List<Exception> errorList = JsonConvert.DeserializeObject<List<Exception>>(File.ReadAllText(errorFile));
                    errorList.Add(ex);
                    File.WriteAllText(errorFile, JsonConvert.SerializeObject(errorList));
                    return null; 
                }
            }
        }
        internal static T Load() => JsonConvert.DeserializeObject<T>(File.ReadAllText(JsonFile));

        internal static void Save(T a) => File.WriteAllText(JsonFile, JsonConvert.SerializeObject(a));
    }
}
