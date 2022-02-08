using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

namespace COT.API.Logger
{
    [ExcludeFromCodeCoverage]
    public class CustomLogger : LogBase
    {
        private string CurrentDirectory { get; set; }
        private string FileName { get; set; }
        private string FilePath { get; set; }
        public CustomLogger(string logFileName = "Log.txt")
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            FileName = logFileName;

            #region create log folder if not exists
            bool exists = Directory.Exists($"{CurrentDirectory}/Logs");
            if (!exists)
                System.IO.Directory.CreateDirectory($"{CurrentDirectory}/Logs");

            #endregion

            FilePath = $"{CurrentDirectory}/Logs/{FileName}";
        }

        public override string Log(string Message, bool logSwitch = true)
        {

            using (StreamWriter w = File.AppendText(FilePath))
            {
                w.WriteLine($"---------- {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} ------------");
                w.WriteLine($"Message: {Message}");
                w.WriteLine("-----------------------------------------------");
            }
            return FilePath;
        }        
    }
}
