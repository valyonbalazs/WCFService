using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Server
{
    public class ServerCallBack : WCFServiceReference.IWCFServerClientCallback
    {
        public void Method1(string clientsData)
        {
            throw new NotImplementedException();
        }


        //public string[] ProcessById(int processId)
        //{
        //    throw new NotImplementedException();
        //}


        string[] WCFServiceReference.IWCFServerClientCallback.ProcessById(int processId)
        {
            throw new NotImplementedException();
        }


        public void GetProcessesFromMachine()
        {
            throw new NotImplementedException();
        }


        public void NotifyServer()
        {
            var mw = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is MainWindow) as MainWindow;
            mw.Dispatcher.Invoke(new Action(() => mw.ClientConnectedLbl.Content = "+1"));
        }



        public void GetServicesFromMachine()
        {
            throw new NotImplementedException();
        }

        public void GetApplicationsFromMachine()
        {
            throw new NotImplementedException();
        }


        public bool StopProcessId(int processId)
        {
            throw new NotImplementedException();
        }


        public bool RestartProcessId(int processId, string procName)
        {
            throw new NotImplementedException();
        }


        public bool StopServiceName(string serviceName)
        {
            throw new NotImplementedException();
        }


        public bool StartServiceName(string serviceName)
        {
            throw new NotImplementedException();
        }


        public bool StartApplicationPath(string appPath)
        {
            throw new NotImplementedException();
        }
    }
}
