﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using EllipticCurve;
using Microsoft.Win32;
using Newtonsoft.Json;
using TicketApp.Class;
using WPFCustomMessageBox;

namespace TicketApp
{
    /// <summary>
    /// Encapsulation to store both sets of data in one file
    /// </summary>
    public class DataInfo
    {
        internal UserInfo UserInfo { get; set; }
        internal List<EmailMsg> EmailMsg { get; set; }

        public DataInfo(UserInfo userInfo, List<EmailMsg> emailMsg)
        {
            UserInfo = userInfo;
            EmailMsg = emailMsg;
        }

        public DataInfo()
        {

        }

        public void Add(EmailMsg emailMsg)
        {
            if (EmailMsg == null)
                EmailMsg = new List<EmailMsg>();
            EmailMsg.Add(emailMsg);
        }

        public void Add(string heading, string body) => Add(new EmailMsg(heading, body));
    }

    class HandleFile
    {
        public bool Save(string data)
        {
            List<string> errorList = Load<List<string>>() ?? new List<string>();
            errorList.Add(data);
            return Save<List<string>>(errorList);
        }

        public bool Save(EmailMsg emailMsg)
        {
            var data = Load<DataInfo>() ?? new DataInfo();
            data.Add(emailMsg);
            return Save<DataInfo>(data);
        }

        private bool Save<T>(T data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(typeof(T) == typeof(DataInfo) ? ProgramDir.DataFile : ProgramDir.ErrorFile, json);
                return true;
            }
            catch (Exception ex)
            {
                var _ = new Error(ex);
                return false;
            }
        }

        public T Load<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(typeof(T) == typeof(DataInfo) ? ProgramDir.DataFile : ProgramDir.ErrorFile));
            }
            catch (Exception ex)
            {
                Error _ = new Error(ex, typeof(T) != typeof(DataInfo));
                return default(T);
            }
        }
    }

    public class Error
    {
        private readonly Exception _exception;

        public Error(Exception exception, bool recursion = false)
        {
            var handleFile = new HandleFile();
            _exception = exception;

            var appendedMessage = "";
            if (Verify())
                appendedMessage = SendError() ? "\nError log sent" : "\nFailed to send error log";
            else
                appendedMessage = "\nUser cancelled";
            if (!recursion)
                handleFile.Save(exception.Message + appendedMessage);
        }

        private bool Verify()
        {

            Debug.WriteLine(_exception.InnerException);
            var result = CustomMessageBox.ShowOKCancel(
                "The program encountered an error, do you want to send this report to Millennium to help resolve this issue?",
                "Error", "Send error log", "Cancel");
            return result == MessageBoxResult.OK;
        }

        private bool SendError()
        {
            return true;
        }
    }
}