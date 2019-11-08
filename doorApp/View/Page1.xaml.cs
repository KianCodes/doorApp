using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Timers;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using SQLite;
using doorApp.Droid.Model;

namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        int devices = 0;

        public SQLiteConnection conn;
        public Registration regmodel;

        private static System.Timers.Timer aTimer;
        private string ipWildcard = "192.168.68.*";
        private int i = 0;
        private int temp = 0;
        private string tempDevices = "";
        private string tempDevices2 = "";
        private int numDevices = 0;
        private string use = "";
        private string[] arrS = new string[255];
        private string[] arrS2 = new string[255];
        public IP_MAC[] arrCopy = new IP_MAC[255];

        private int numOWRT = 0;
        List<IP_MAC> listOWRT = new List<IP_MAC>();
        List<IP_MAC> tempListOWRT = new List<IP_MAC>();
        List<Registration> listDB = new List<Registration>();



        public Page1()
        {
            InitializeComponent();
            btnAddDev.Clicked += BtnAddDev_Clicked;
            btnDeleteDB.Clicked += BtnDeleteDB_Clicked;

            // DB connection
            conn = DependencyService.Get<Isqlite>().GetConnection();
            conn.CreateTable<Registration>();
        }

        /// <summary>
        /// Deleting all rows from table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteDB_Clicked(object sender, EventArgs e)
        {
            // Deleting all table items
            conn.DeleteAll<Registration>();
            Debug.WriteLine("Delete performed!\n" +
                "Numer of items in DB: " + listDB.Count);
        }

        //Button to simulate adding a new card to the device list
        private async void BtnAddDev_Clicked(object sender, EventArgs e)
        {
            string result = "";
            string tempMAC = "";
            string tempIP = "";

            // Preventing APP to crash
            if (tempListOWRT.Count != 0)
            {
                tempMAC = tempListOWRT[0].MAC;
                tempIP = tempListOWRT[0].IP;
            }

            deviceOWRT.Text = tempDevices;
            deviceDB.Text = tempDevices2;
          
            Debug.WriteLine(tempMAC);
            // Checking if DB is empty
            if (listDB.Count == 0 && numOWRT != 0)
            {
                result = await DisplayActionSheet("New device connected:\n" +
                    tempMAC, "Ignore", null, "ACCEPT", "DENY");

                // INSERTING device into DB with the appropriate status
                insertDB(result, tempMAC, tempIP);
            }

            //for (int i = 0; i < numDevices; i++)
            //{
            //    newFrame(arrCopy[i].IP, arrCopy[i].MAC, i+1);
            //    Task.Delay(1000);
            //}

            ////newFrame("123", "321", devices + 1);
            //devices++;
        }

        //Function to create a new card for the device list.  
        //Uses specific inputs rather than an object.
        //Will implement overloaded method using object as parameter
        private void newFrame(string ip, string mac, int device)
        {
            stackTest.Children.Clear();
            Frame cardFrame = new Frame
            {
                BorderColor = Color.Gray,
                CornerRadius = 5,
                Padding = 8,
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = $"Device {device}",
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            FontAttributes = FontAttributes.Bold
                        },
                        new BoxView
                        {
                            Color = Color.Gray,
                            HeightRequest = 2,
                            HorizontalOptions = LayoutOptions.Fill
                        },
                        new Label
                        {
                            Text = $"IP Address: {ip}\nMAC Address: {mac}"
                        }
                    }
                }
            };
            stackTest.Children.Add(cardFrame);
            
        }

        /// <summary>
        /// Used to constantly scan the devices and compare them to the list of devices located on our DB
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Scanning oWRT with nmap to obtain list of devices
            scanOWRT();

            // Scanning DB with current devices
            scanDB();

            // Debugging
            Debug.WriteLine("Number of items in listOWRT: " + listOWRT.Count);
            Debug.WriteLine("Number of items in listDB: " + listDB.Count);

            // Clearing listOWRT for it to scan the system again
            numOWRT = listOWRT.Count;
            tempListOWRT = listOWRT;
            //listOWRT.Clear();
        }

        T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }


        private void setTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(6000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void stopTimer()
        {
            aTimer.Stop();
        }


        protected override void OnAppearing()
        {
            setTimer();
            Debug.WriteLine("This is where it happens!");
        }

        protected override void OnDisappearing()
        {
            stopTimer();
            Debug.WriteLine("Leaving the best behind!");
        }


        private void scanDB()
        {
            // Getting back the info from table
            //Debug.WriteLine("This is the count of devices stored: " + conn.Table<Registration>().Count());
            listDB = conn.Table<Registration>().ToList();
            Debug.WriteLine(listDB.Count);

            //Debug.WriteLine(listDB[0].ipAddr.ToString());

            for (int j = 0; j < listDB.Count; j++)
            {
                arrS2[j] = "Device # " + listDB[j].id + "\n" +
                    listDB[j].ipAddr + "\n" +
                    listDB[j].macAddr + "\n" +
                    listDB[j].status;


            }
            tempDevices2 = "DB devices: \n" + string.Join("\n\n", arrS2.Where(s => !string.IsNullOrEmpty(s)));

            // Clearing string array
            Array.Clear(arrS2, 0, arrS2.Length);
        }

        private void insertDB(string status, string IP, string MAC)
        {
            string rejectTCP = "http://192.168.64.2/cgi-bin/luci/command/cfg0f9944";
            string rejectUDP = "http://192.168.64.2/cgi-bin/luci/command/cfg109944";
            string rejectTCPend = "%20-p%20tcp%20-m%20comment%20--comment%20%22!fw3%3A%20Block%20100%22%20-j%20reject";
            string rejectUDPend = "%20-p%20udp%20-m%20comment%20--comment%20%22!fw3%3A%20Block%20100%22%20-j%20reject";

            Registration reg = new Registration();
            if (status == "ACCEPT")
            {
                reg.ipAddr = IP;
                reg.macAddr = MAC;
                reg.status = status;
                reg.nickname = "Device " + reg.id;
            }
            else if (status == "DENY")
            {
                reg.ipAddr = IP;
                reg.macAddr = MAC;
                reg.status = status;
                reg.nickname = "Device " + reg.id;

                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    client.BaseAddress = new Uri(rejectTCP + "/%20" + IP + rejectTCPend);
                    HttpResponseMessage response2 = client.GetAsync("").Result;
                    response2.EnsureSuccessStatusCode();
                    string result2 = response2.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Result: " + result2);

                    client.BaseAddress = new Uri(rejectUDP + "/%20" + IP + rejectUDPend);
                    HttpResponseMessage response3 = client.GetAsync("").Result;
                    response3.EnsureSuccessStatusCode();
                    string result3 = response3.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Result: " + result3);
                }
            }
            int x = 0;
            try
            {
                x = conn.Insert(reg);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (x == 1)
            {
                DisplayAlert("Registration", "Successful", "Ok");
            }
            else
            {
                DisplayAlert("Registration", "Failed", "Ok");
            }

            
            
            //var details = (from y in conn.Table<Registration>() select y).ToList();
            //myDevice.Text = String.Join(", ", details.ToArray());
        }

        private void scanOWRT()
        {
            // Creating array of objects
            //IP_MAC[] arr = InitializeArray<IP_MAC>(255);

            // Clearing vars
            i = 0;
            temp = 0;
            numDevices = 0;


            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("http://192.168.64.2/cgi-bin/luci/command/cfg0e9944" + "/-sP%20" + ipWildcard);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result);

                tempListOWRT = listOWRT;
                listOWRT.Clear();
                while (i != -1)
                {

                    i = result.IndexOf("Nmap scan report for", i);
                    use = result.Substring(i + 21, result.IndexOf("\n", i + 22) - (i + 21));
                    if (use == "192.168.68.2") // Breaking out of loop if it identifies the router's IP (which will be inherently the last one listed
                        break;
                    //listOWRT[temp].IP = use;
                    i = result.IndexOf("MAC Address:", i);
                    //listOWRT[temp].MAC = result.Substring(i + 13, result.IndexOf(" ", i + 14) - (i + 13));

                    listOWRT.Add(new IP_MAC { IP = use, MAC = result.Substring(i + 13, result.IndexOf(" ", i + 14) - (i + 13)) }) ;
                    temp++;
                }
                temp = 0;
                Debug.WriteLine("This is the first IP: "  + listOWRT[0].IP);
                Debug.WriteLine("This is the first MAC: " + listOWRT[0].MAC);
                for(int j = 0; j < listOWRT.Count; j++)
                {
                    arrS[j] = "Device # " + (j + 1) + "\n" +
                        listOWRT[j].IP + "\n" +
                        listOWRT[j].MAC;

                }
                tempDevices = "oWRT devices: \n" + string.Join("\n\n", arrS.Where(s => !string.IsNullOrEmpty(s)));

                // Clearing string array
                Array.Clear(arrS, 0, arrS.Length);
            }
            //arrCopy = arr;
        }
        


    }

}