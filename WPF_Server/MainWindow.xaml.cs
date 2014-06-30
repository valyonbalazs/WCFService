using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Application.Current.Exit += Current_Exit;

        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
            InstanceContext context = new InstanceContext(callback);
            WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context);
            client.ServerClosed();
            client.Close();
        }

        private void machineLstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(machineLstBox.SelectedItem != null)
            {
                string tmp1 = machineLstBox.SelectedItem.ToString();
                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    var data = client.GetProcessesFromClient(tmp1);
                    var data2 = client.GetServicesFromClient(tmp1);
                    var data3 = client.GetApplicationsFromClient(tmp1);

                    var listData = data.ToList();
                    listData.RemoveAt(0);
                    listData.Sort();

                    List<string> listData2 = new List<string>();
                    foreach (var item in data2)
                    {
                        var tmp = item.Split(',');
                        listData2.Add(tmp[0]);
                    }

                    listData2.RemoveAt(0);
                    listData2.Sort();

                    var listData3 = data3.ToList();
                    listData3.RemoveAt(0);


                    processLstBox.ItemsSource = listData;
                    serviceLstBx.ItemsSource = listData2;
                    appsLstBx.ItemsSource = listData3;
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
            InstanceContext context = new InstanceContext(callback);
            WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context);

            var data = client.GetClients();
            machineLstBox.ItemsSource = data;

            if(data.Length == 0)
            {
                processLstBox.ItemsSource = null;
                serviceLstBx.ItemsSource = null;
                appsLstBx.ItemsSource = null;
            }
            ClientConnectedLbl.Content = "";

        }

        private void processLstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (processLstBox.SelectedItem == null)
            {
                processLstBox.SelectedItem = processLstBox.Items.GetItemAt(1);
            }
            else
            {
                string tmp1 = processLstBox.SelectedItem.ToString();
                var tmp2 = tmp1.Split(' ');
                int id = Int32.Parse(tmp2[1].ToString());
                string machineKey = machineLstBox.SelectedItem.ToString();

                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    string[] procDetails = client.GetProcessById(machineKey, id);

                    procNameResultLbl.Content = procDetails.ElementAt(0);
                    procIdResultLbl.Content = procDetails.ElementAt(1);
                    procPrioResultLbl.Content = procDetails.ElementAt(2);
                    procSessIdResultLbl.Content = procDetails.ElementAt(3);
                    procContanResultLbl.Content = procDetails.ElementAt(4);
                    procPagedMemResultLbl.Content = procDetails.ElementAt(5);
                    procPkVirtMemResultLbl.Content = procDetails.ElementAt(6);
                    procContanResultLbl.Content = procDetails.ElementAt(7);
                    //threadsLstBx.ItemsSource = procDetails.ElementAt(8);
                }
            }


        }

        private void processStpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (processLstBox.SelectedItem != null)
            {
                string tmp1 = processLstBox.SelectedItem.ToString();
                var tmp2 = tmp1.Split(' ');
                int id = Int32.Parse(tmp2[1].ToString());
                string machineKey = machineLstBox.SelectedItem.ToString();
                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    bool success = client.StopProcessById(machineKey, id);

                    if (success)
                    {
                        MessageBox.Show("Sikeresen leállított folyamat!", "Sikeresen leállítva!", MessageBoxButton.OK, MessageBoxImage.Information);

                        var data = client.GetProcessesFromClient(machineKey);
                        var listData = data.ToList();
                        listData.RemoveAt(0);
                        listData.Sort();
                        processLstBox.ItemsSource = listData;
                    }
                }
            }
            else
            {
                MessageBox.Show("Még nem választottál ki folyamatot!", "Nem választottál ki folyamatot!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void processRstrtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (processLstBox.SelectedItem != null)
            {
                string tmp1 = processLstBox.SelectedItem.ToString();
                var tmp2 = tmp1.Split(' ');
                string procName = tmp2[0].ToString();
                int id = Int32.Parse(tmp2[1].ToString());
                string machineKey = machineLstBox.SelectedItem.ToString();
                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    bool success = client.RestartProcessById(machineKey, id, procName);

                    if (success)
                    {
                        MessageBox.Show("Sikeresen újraindított folyamat!", "Sikeresen újraindítva!", MessageBoxButton.OK, MessageBoxImage.Information);

                        var data = client.GetProcessesFromClient(machineKey);
                        var listData = data.ToList();
                        listData.RemoveAt(0);
                        listData.Sort();
                        processLstBox.ItemsSource = listData;
                    }
                }
            }
            else
            {
                MessageBox.Show("Még nem választottál ki folyamatot!", "Nem választottál ki folyamatot!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void serviceLstBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serviceLstBx.SelectedItem == null)
            {
                serviceLstBx.SelectedItem = serviceLstBx.Items.GetItemAt(0);
            }
            else
            {
                string tmp1 = machineLstBox.SelectedItem.ToString();
                string selectedService = serviceLstBx.SelectedItem.ToString();
                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    var data = client.GetServicesFromClient(tmp1);
                    var selected = data.FirstOrDefault(x => x.Contains(selectedService));
                    var dataTmp = selected.Split(',');

                    serviceStateResLbl.Content = dataTmp[1];
                    serviceContResLbl.Content = dataTmp[2];
                    serviceTypeResLbl.Content = dataTmp[3];
                }
            }
        }

        private void serviceStpBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (serviceLstBx.SelectedItem != null)
            {
                string machineKey = machineLstBox.SelectedItem.ToString();
                string serviceName = serviceLstBx.SelectedItem.ToString();

                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    bool success = client.StopServiceByName(machineKey, serviceName);
                    MessageBox.Show("Sikeresen leállított szolgáltatás!", "Sikeresen leállítva!", MessageBoxButton.OK, MessageBoxImage.Information);

                    var data = client.GetServicesFromClient(machineKey);

                    var selected = data.FirstOrDefault(x => x.Contains(serviceName));
                    var dataTmp = selected.Split(',');

                    serviceStateResLbl.Content = dataTmp[1];
                    serviceContResLbl.Content = dataTmp[2];
                    serviceTypeResLbl.Content = dataTmp[3];

                    List<string> listData2 = new List<string>();
                    foreach (var item in data)
                    {
                        var tmp = item.Split(',');
                        listData2.Add(tmp[0]);
                    }

                    listData2.RemoveAt(0);
                    listData2.Sort();

                    

                    serviceLstBx.ItemsSource = listData2;
                }
            }
            else
            {
                MessageBox.Show("Még nem választottál ki szolgáltatást!", "Nem választottál ki szolgáltatást!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void serviceRstrtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (serviceLstBx.SelectedItem != null)
            {
                string machineKey = machineLstBox.SelectedItem.ToString();
                string serviceName = serviceLstBx.SelectedItem.ToString();

                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    bool success = client.StartServiceByName(machineKey, serviceName);

                    if(success)
                    {
                        MessageBox.Show("Sikeresen elindult a szolgáltatás!", "Sikeresen elindítva!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nem indult el a szolgáltatás!", "Hiba történt!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    var data = client.GetServicesFromClient(machineKey);

                    var selected = data.FirstOrDefault(x => x.Contains(serviceName));
                    var dataTmp = selected.Split(',');

                    serviceStateResLbl.Content = dataTmp[1];
                    serviceContResLbl.Content = dataTmp[2];
                    serviceTypeResLbl.Content = dataTmp[3];

                    List<string> listData2 = new List<string>();
                    foreach (var item in data)
                    {
                        var tmp = item.Split(',');
                        listData2.Add(tmp[0]);
                    }

                    listData2.RemoveAt(0);
                    listData2.Sort();

                    serviceLstBx.ItemsSource = listData2;
                }
            }
            else
            {
                MessageBox.Show("Még nem választottál ki szolgáltatást!", "Nem választottál ki szolgáltatást!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void applStartManPath_Click(object sender, RoutedEventArgs e)
        {
            if(appStrtPathTxtf.Text != null)
            {
                string machineKey = machineLstBox.SelectedItem.ToString();
                string appPath = appStrtPathTxtf.Text.ToString();

                WCFServiceReference.IWCFServerClientCallback callback = new ServerCallBack();
                InstanceContext context = new InstanceContext(callback);
                using (WCFServiceReference.WCFServerClientClient client = new WCFServiceReference.WCFServerClientClient(context))
                {
                    bool success = client.StartApplicationByPath(machineKey, appPath);

                    var data = client.GetProcessesFromClient(machineKey);
                    var listData = data.ToList();
                    listData.RemoveAt(0);
                    listData.Sort();
                    processLstBox.ItemsSource = listData;

                    if (success)
                    {
                        MessageBox.Show("Sikeresen elindult az alkalmazás!", "Sikeresen elindítva!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nem indult el az alkalmazás!", "Hiba történt!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
