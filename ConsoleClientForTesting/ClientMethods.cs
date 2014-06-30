using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClientForTesting
{
    public class ClientMethods
    {
        public static string clientId = string.Empty;

        /// <summary>
        /// Ezzel a metódussal kapcsolódik a felügyelő alkalmazáshoz,
        /// továbbá itt küldi el először az azonosítóját, amely
        /// az adott számítógép nevéből és IP címéből áll.
        /// </summary>
        public void ConnectToServer()
        {
            WCFService.IWCFServerClientCallback callback = new ClientCallback();
            InstanceContext context = new InstanceContext(callback);
            WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context);

            string hostName = System.Environment.MachineName;
            string localIP = null;

            //IP cím kinyerése és formázása
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());                
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && localIP==null)
                {
                    localIP = ip.ToString();
                }
            }

            clientId = hostName + " " + localIP;

            client.ClientRegistration(clientId);
        }

        public void SendProcessListToServer()
        {
            WCFService.IWCFServerClientCallback callback = new ClientCallback();
            InstanceContext context = new InstanceContext(callback);
            using (WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context))
            {
                List<string> processList = new List<string>();
                var processes = Process.GetProcesses();

                processList.Add(clientId);
                foreach (var p in processes)
                {
                    processList.Add(p.ProcessName + " " + p.Id);
                }

                client.ClientProcesses(processList.ToArray());                
            }

        }

        public void SendServiceListToServer()
        {
            WCFService.IWCFServerClientCallback callback = new ClientCallback();
            InstanceContext context = new InstanceContext(callback);
            using (WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context))
            {

                List<string> serviceList = new List<string>();

                serviceList.Add(clientId);

                ServiceController[] services = ServiceController.GetServices();

                foreach (var i in services)
                {
                    serviceList.Add(i.DisplayName + "," + i.Status + "," + i.Container + "," + i.ServiceType);                    
                }
                
                client.ClientServices(serviceList.ToArray());
            }
        }

        public void SendApplicationListToServer()
        {
            WCFService.IWCFServerClientCallback callback = new ClientCallback();
            InstanceContext context = new InstanceContext(callback);
            using (WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context))
            {

                List<Object> appList = new List<Object>();

                string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            appList.Add(subkey.GetValue("DisplayName"));
                        }
                    }
                }

                appList.Sort();

                List<String> appListString = new List<string>();

                appListString.Add(clientId);

                foreach (var a in appList)
                {
                    if(a == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        appListString.Add((String)a);
                    }                    
                }

                HashSet<string> tmpSet = new HashSet<string>();
                foreach (var i in appListString)
                {
                    if(tmpSet.Contains(i))
                    {

                    }
                    else 
                    {
                        tmpSet.Add(i);
                    }

                }

                client.ClientApplications(tmpSet.ToArray());
                //client.ClientApplications(appListString.ToArray());
            }
        }


        public string[] ProcessDetailsById(int processId)
        {
            string[] processDetails = new string[10];
            var process = Process.GetProcessById(processId);

            processDetails[0] = process.ProcessName;
            processDetails[1] = process.Id.ToString();
            //processDetails[2] = process.PriorityClass.ToString();
            processDetails[3] = process.SessionId.ToString();
            //processDetails[4] = process.Container.ToString();
            processDetails[5] = process.PagedMemorySize64.ToString();
            processDetails[6] = process.PeakVirtualMemorySize64.ToString();
            //processDetails[7] = process.StartTime.ToLocalTime().ToString();
            //processDetails.Add(process.Threads.ToString());

            return processDetails;
        }

        public bool StopProcess(int processId)
        {
            bool wasItSuccess = false;

            try
            {
                var proc = Process.GetProcessById(processId);
                proc.Kill();
                wasItSuccess = true;
            }
            catch (Exception)
            {                
                throw;
            }
            
            return wasItSuccess;
        }

        public bool RestartProcess(int processId, string procName)
        {
            bool wasItSuccess = false;

            try
            {
                var proc = Process.GetProcessById(processId);
                proc.Kill();
                proc.WaitForExit();

                Process.Start(procName);
                
                wasItSuccess = true;
            }
            catch (Exception)
            {                
                throw;
            }

            return wasItSuccess;
        }

        public bool StopService(string serviceName)
        {
            bool wasItSuccess = false;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(2000);
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                wasItSuccess = true;
            }
            catch (Exception)
            {                
                throw;
            }

            return wasItSuccess;
        }

        public bool StartService(string serviceName)
        {
            bool wasItSuccess = false;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(2000);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                wasItSuccess = true;
            }
            catch (Exception)
            {
                throw;
            }

            return wasItSuccess;
        }

        public bool StartApplicationByPath(string appPath)
        {
            bool wasItSuccess = false;
            string startpath = @appPath;

            try
            {
                Process.Start(startpath);
                wasItSuccess = true;
            }
            catch (Exception)
            {
                
                throw;
            }

            return wasItSuccess;
        }
    }
}
