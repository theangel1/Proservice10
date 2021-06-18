using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchingService
{
    public static class Logger
    {

        public static void Log(string mensaje)
        {
            try
            {
                string _mensaje = String.Format("{0} {1}", mensaje, Environment.NewLine);
                File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["log"] + "Bitacora-proservice.log", _mensaje);
            }
            catch (Exception ex)
            {
                File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["log"] + "Bitacora-proservice.log", ex.ToString());
            }


        }
    }
}
