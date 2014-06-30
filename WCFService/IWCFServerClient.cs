using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{

    [ServiceContract(CallbackContract = typeof(ICallBack), SessionMode=SessionMode.Required)]
    public interface IWCFServerClient
    {
        [OperationContract]
        void ClientRegistration(string registInfo);

        [OperationContract]
        void ClientProcesses(string[] processList);

        [OperationContract]
        void ClientServices(string[] serviceList);

        [OperationContract]
        void ClientApplications(string[] applicationList);

        [OperationContract]
        List<string> GetClients();

        [OperationContract]
        string[] GetProcessById(string machineKey, int processId);

        [OperationContract]
        bool StopProcessById(string machineKey, int processId);

        [OperationContract]
        bool RestartProcessById(string machineKey, int processId, string procName);

        [OperationContract]
        bool StopServiceByName(string machineKey, string serviceName);

        [OperationContract]
        bool StartServiceByName(string machineKey, string serviceName);

        [OperationContract]
        bool StartApplicationByPath(string machineKey, string appPath);

        [OperationContract]
        List<string> GetProcessesFromClient(string key);

        [OperationContract]
        List<string> GetServicesFromClient(string key);

        [OperationContract]
        List<string> GetApplicationsFromClient(string key);

        [OperationContract]
        void RemoveClientFromList(string clientId);

        [OperationContract]
        void ServerClosed();
    }
}
