using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleClientForTesting
{
    class Program
    {
        //Az alkalmazás leállításához szükséges eseménykezelőre feliratkozás
        //Amikor az alkalmazást leállítják, a WCFService-nél meghívja a
        //RemoveClientFromList metódust, hogy törölje magát a listából
        static ConsoleEventDelegate handler;
        delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static bool ConsoleEventCallback(int eventType)
        {
            if(eventType == 2)
            {
                WCFService.IWCFServerClientCallback callback = new ClientCallback();
                InstanceContext context = new InstanceContext(callback);
                WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context);
                client.RemoveClientFromList(machineKey);                
            }
            return false;
        }

        static string machineKey = String.Empty;

        static void Main(string[] args)
        {

            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            GenerateMachineKey();

            Console.WriteLine("Szerver felügyeleti kliens ügynök alkalmazás elindult");
            Console.WriteLine("-----------------------------------------------------\n");
            Console.WriteLine("A rendszer azonosítója: " + machineKey + "\n");
            Console.WriteLine("Kapcsolódás a szerverhez...\n");

            WCFService.IWCFServerClientCallback callback = new ClientCallback();
            InstanceContext context = new InstanceContext(callback);
            WCFService.WCFServerClientClient client = new WCFService.WCFServerClientClient(context);

            ClientMethods methods = new ClientMethods();
            methods.ConnectToServer();


            Console.WriteLine("Az alkalmazás leállításához gépelje be, hogy  exit   majd nyomjon entert!");
            string ifExit = Console.ReadLine();

            if(ifExit.Equals("exit"))
            {
                client.RemoveClientFromList(machineKey);
            }
            else
            {
                ifExit = Console.ReadLine();
            }
            


        }

        static void GenerateMachineKey()
        {
            string hostName = System.Environment.MachineName;
            string localIP = null;

            //IP cím kinyerése és formázása
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && localIP == null)
                {
                    localIP = ip.ToString();
                }
            }

            machineKey = hostName + " " + localIP;
        }
    }
}
