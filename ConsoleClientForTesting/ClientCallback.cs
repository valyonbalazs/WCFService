using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClientForTesting
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class ClientCallback : WCFService.IWCFServerClientCallback
    {
        void WCFService.IWCFServerClientCallback.Method1(string clientsData)
        {
            Console.WriteLine("A felügyeleti szerverhez kapcsolódott a " + clientsData + " azonosítójú ügynök\n");

        }

        string[] WCFService.IWCFServerClientCallback.ProcessById(int processId)
        {
            ClientMethods cm = new ClientMethods();

            return cm.ProcessDetailsById(processId).ToArray();
        }

        void WCFService.IWCFServerClientCallback.GetProcessesFromMachine()
        {
            ClientMethods cm = new ClientMethods();

            Console.WriteLine("Processz lista felküldése a szerverre...\n");

            cm.SendProcessListToServer();
        }

        void WCFService.IWCFServerClientCallback.NotifyServer()
        {
            throw new NotImplementedException();
        }


        public void GetServicesFromMachine()
        {
            ClientMethods cm = new ClientMethods();

            Console.WriteLine("Szolgáltatás lista felküldése a szerverre...\n");

            cm.SendServiceListToServer();
        }

        public void GetApplicationsFromMachine()
        {
            ClientMethods cm = new ClientMethods();

            Console.WriteLine("Alklamazás lista felküldése a szerverre...\n");

            cm.SendApplicationListToServer();
        }


        public bool StopProcessId(int processId)
        {
            ClientMethods cm = new ClientMethods();
            return cm.StopProcess(processId);
        }


        public bool RestartProcessId(int processId, string procName)
        {
            ClientMethods cm = new ClientMethods();
            return cm.RestartProcess(processId, procName);
        }


        public bool StopServiceName(string serviceName)
        {
            ClientMethods cm = new ClientMethods();
            return cm.StopService(serviceName);
        }


        public bool StartServiceName(string serviceName)
        {
            ClientMethods cm = new ClientMethods();
            return cm.StartService(serviceName);
        }


        public bool StartApplicationPath(string appPath)
        {
            ClientMethods cm = new ClientMethods();
            return cm.StartApplicationByPath(appPath);
        }
    }
}
