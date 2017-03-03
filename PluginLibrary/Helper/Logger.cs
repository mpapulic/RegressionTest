using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLibrary.Helper
{
    class Logger
    {
        //public static void Main()
        //{
        //    using (StreamWriter w = File.AppendText("log.txt"))
        //    {
        //        Log("Test1", w);
        //        Log("Test2", w);
        //    }

        //    using (StreamReader r = File.OpenText("log.txt"))
        //    {
        //        DumpLog(r);
        //    }
        //}

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0}: {1}", DateTime.Now.ToLongTimeString(), logMessage);
            //w.WriteLine("-------------------------------");
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
