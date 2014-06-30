using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFService
{
    public interface ICallBack
    {
        [OperationContract]
        void Method1(string clientsData);

        [OperationContract]
        string[] ProcessById(int processId);

        [OperationContract]
        bool StopProcessId(int processId);

        [OperationContract]
        bool RestartProcessId(int processId, string procName);

        [OperationContract]
        bool StopServiceName(string serviceName);

        [OperationContract]
        bool StartServiceName(string serviceName);

        [OperationContract]
        bool StartApplicationPath(string appPath);

        [OperationContract]
        void GetProcessesFromMachine();

        [OperationContract]
        void GetServicesFromMachine();

        [OperationContract]
        void GetApplicationsFromMachine();

        [OperationContract]
        void NotifyServer();
    }
}
