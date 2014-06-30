using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace WCFService
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple,
        InstanceContextMode=InstanceContextMode.PerSession,
        AddressFilterMode=AddressFilterMode.Any)]
    public class WCFServerClient : IWCFServerClient
    {
        public static Dictionary<string, ICallBack> clients = new Dictionary<string, ICallBack>();
        public static Dictionary<string, List<string>> clientsProcesses = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> clientsServices = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> clientsApplications = new Dictionary<string, List<string>>();

        public static ICallBack serverCallback = null;

        public WCFServerClient()
        {

        }


        public void ClientRegistration(string registInfo)
        {
            ICallBack client = OperationContext.Current.GetCallbackChannel<ICallBack>();

            if(serverCallback != null)
            {
                serverCallback.NotifyServer();
            }

            if(!clients.ContainsKey(registInfo))
            {
                clients.Add(registInfo, client);
            }
            else 
            {
                client.Method1("Nem jól lett leállítva az ügynök alkalmazás!\n");
            }

            //adott kliens visszahívása tesztelésre
            string data = clients.FirstOrDefault(x => x.Key == registInfo).Key;
            client.Method1(data);
        }

        public void ClientProcesses(string[] processList)
        {
            string clientId = processList.ElementAt(0);
   
            if (clientsProcesses.ContainsKey(clientId))
            {
                clientsProcesses[clientId] = processList.ToList();
            }
            else
            {
                clientsProcesses.Add(clientId, processList.ToList());
            }
        }

        public void ClientServices(string[] serviceList)
        {
            string clientId = serviceList.ElementAt(0);

            if(clientsServices.ContainsKey(clientId))
            {
                clientsServices[clientId] = serviceList.ToList();
            }
            else
            {
                clientsServices.Add(clientId, serviceList.ToList());
            }
        }

        public void ClientApplications(string[] applicationList)
        {
            string clientId = applicationList.ElementAt(0);

            if (clientsApplications.ContainsKey(clientId))
            {
                clientsApplications[clientId] = applicationList.ToList();
            }
            else
            {
                clientsApplications.Add(clientId, applicationList.ToList());
            }
        }


        public List<string> GetClients()
        {
            List<string> clientList = new List<string>();

            if(clients.Count == 0)
            {

            }
            else
            {
                serverCallback = OperationContext.Current.GetCallbackChannel<ICallBack>();

                foreach (var c in clients)
                {
                    clientList.Add(c.Key);
                }
            }

            return clientList;
        }

        public string[] GetProcessById(string machineKey, int processId)
        {
            string[] processDetails = new string[10];
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;

            processDetails = clientChannel.ProcessById(processId);
            return processDetails;
            
        }

        public bool StopProcessById(string machineKey, int processId)
        {
            bool isTheProcessStopped = false;
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;

            isTheProcessStopped = clientChannel.StopProcessId(processId);
            return isTheProcessStopped;
        }

        public bool RestartProcessById(string machineKey, int processId, string procName)
        {
            bool isTheProcessRestarted = false;
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;

            isTheProcessRestarted = clientChannel.RestartProcessId(processId, procName);
            return isTheProcessRestarted;
        }

        public bool StopServiceByName(string machineKey, string serviceName)
        {
            bool isTheServiceStopped = false;
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;

            isTheServiceStopped = clientChannel.StopServiceName(serviceName);
            return isTheServiceStopped;
        }

        public bool StartServiceByName(string machineKey, string serviceName)
        {
            bool isTheServiceStarted = false;
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;
            isTheServiceStarted = clientChannel.StartServiceName(serviceName);

            return isTheServiceStarted; 
        }

        public bool StartApplicationByPath(string machineKey, string appPath)
        {
            bool isTheApplicationStarted = false;
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == machineKey).Value;
            isTheApplicationStarted = clientChannel.StartApplicationPath(appPath);

            return isTheApplicationStarted;
        }


        public List<string> GetProcessesFromClient(string key)
        {
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == key).Value;
            clientChannel.GetProcessesFromMachine();

            return clientsProcesses.FirstOrDefault(x => x.Key == key).Value;
        }

        public void ServerClosed()
        {            
            clients.Clear();
            clientsProcesses.Clear();
        }


        public List<string> GetServicesFromClient(string key)
        {
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == key).Value;
            clientChannel.GetServicesFromMachine();

            return clientsServices.FirstOrDefault(x => x.Key == key).Value;
        }

        public List<string> GetApplicationsFromClient(string key)
        {
            ICallBack clientChannel = clients.FirstOrDefault(x => x.Key == key).Value;
            clientChannel.GetApplicationsFromMachine();

            return clientsApplications.FirstOrDefault(x => x.Key == key).Value;
        }

        public void RemoveClientFromList(string clientId)
        {
            if(clients.ContainsKey(clientId))
            {
                try
                {
                    clients.Remove(clientId);
                    if(clients.Count == 0)
                    {
                        serverCallback = null;
                    }
                }
                catch (Exception)
                {                    
                    throw;
                }
            }
            if(clientsProcesses.ContainsKey(clientId))
            {
                try
                {
                    clientsProcesses.Remove(clientId);
                }
                catch (Exception)
                {                    
                    throw;
                }
            }
            if(clientsServices.ContainsKey(clientId))
            {
                try
                {
                    clientsServices.Remove(clientId);
                }
                catch (Exception)
                {                    
                    throw;
                }
            }
            if(clientsApplications.ContainsKey(clientId))
            {
                try
                {
                    clientsApplications.Remove(clientId);
                }
                catch (Exception)
                {                    
                    throw;
                }
            }

        }



    }
}
