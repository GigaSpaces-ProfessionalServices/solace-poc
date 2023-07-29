using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piper.Common
{
    public class Logger
    {
   
       
        private static object _Lock = new object();
        private static Logger _Logger;
        private FileStream fs;
        private StreamWriter _logFileStream;

        public Logger(long partionid)
        {
            string fileName = "DataProcessor_" + partionid.ToString() + "_" +
                            DateTime.Now.ToString("yyyyMMdd") + ".log";
            string fileNamePath = "C:\\GigaSpaces\\XAP.NET-16.3.0-patch-p-3-x64\\NET v4.0\\Logs\\" + fileName;
            fs = new FileStream(fileNamePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            _logFileStream = new StreamWriter(fs);
            _logFileStream.AutoFlush = true;


            _logFileStream.WriteLine("Logger V2 Started at " + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        public static void Init(long partionid)
        {
              lock (_Lock)
              {
                    if (_Logger == null)
                    {
                        _Logger = new Logger( partionid);
                    }
               }

        }

        public static void Write(string msg)
        {
            lock (_Lock)
            {
                if (_Logger != null)
                {
                    _Logger._logFileStream.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + msg);
                }
            }
        }
    }
        

}
