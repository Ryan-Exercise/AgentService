// ServiceCommon.Logger.FileLogger in $projectname$
// Copyright(c) 2023 $registeredogranisation$ All Rights Reserved.
// Author:  cxu
// Date:    6/8/2023 6:02:44 PM
// Description:
//      <Describ here>

namespace ServiceCommon.Logger
{
    public class FileLogger
    {
        private string filePath;
        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void Log(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
        }
    }
}
