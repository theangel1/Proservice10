using FileWatchingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main()
        {
#if DEBUG
            ProService10 s = new ProService10();
            s.onDebug();

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else
            ServiceBase[] ServicesToRun;
             ServicesToRun = new ServiceBase[]
            {
                new ProService10()
            };
            ServiceBase.Run(ServicesToRun);
            
#endif
        }
    }
}
