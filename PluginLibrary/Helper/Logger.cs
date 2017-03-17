using System;
using System.IO;

namespace PluginLibrary.Helper
{
    static class Logger
    {
        public static void Log(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Temp\Log.txt"))
            {
                {
                    //sw.Write("\r\nLog Entry : ");
                    sw.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), logMessage);
                }
            }
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
