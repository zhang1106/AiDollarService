using System;
using System.IO;
using System.ServiceProcess;
using log4net.Config;

namespace AiDollar.Gateway
{
    partial class AiDollarGateway : ServiceBase
    {
        public AiDollarGateway()
        {
            InitializeComponent();
        }

        static void Main(string[] args)
        {
            var currentDomain = AppDomain.CurrentDomain;
            Directory.SetCurrentDirectory(currentDomain.BaseDirectory);
            XmlConfigurator.Configure();

            var service = new AiDollarGateway();

            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("Press any key to stop program");

                Console.ReadKey();
                service.OnStop();
            }
            else
            {
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            var root = Edgar.Service.CompositionRoot.CompositeRootInstanace();
            var svc = root.Initialize();
            svc.Start();
        }

        protected override void OnStop()
        {

        }
    }
}
