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



namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        int devices = 0;

        private static System.Timers.Timer aTimer;
        private string ipWildcard = "192.168.68.*";
        private int i = 0;
        private int temp = 0;
        private string tempDevices = "";
        private int numDevices = 0;
        private string use = "";
        private string[] arrS = new string[255];
        public IP_MAC[] arrCopy = new IP_MAC[255];
        private bool flag = true;


        public Page1()
        {
            InitializeComponent();
            btnAddDev.Clicked += BtnAddDev_Clicked;
        }

        //Button to simulate adding a new card to the device list
        private void BtnAddDev_Clicked(object sender, EventArgs e)
        {

            for(int i = 0; i < numDevices; i++)
            {
                newFrame(arrCopy[i].IP, arrCopy[i].MAC, i+1);
                Task.Delay(1000);
            }

            //newFrame("123", "321", devices + 1);
            devices++;
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
            //Debug.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //                  e.SignalTime);

            // Creating array of objects
            IP_MAC[] arr = InitializeArray<IP_MAC>(255);

            // Clearing vars
            i = 0;
            temp = 0;
            numDevices = 0;


            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("http://192.168.64.2/cgi-bin/luci/command/cfg0f9944" + "/-sP%20" + ipWildcard);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result);

                while (i != -1)
                {

                    i = result.IndexOf("Nmap scan report for", i);
                    use = result.Substring(i + 21, result.IndexOf("\n", i + 22) - (i + 21));
                    if (use == "192.168.68.2") // Breaking out of loop if it identifies the router's IP (which will be inherently the last one listed
                        break;
                    arr[temp].IP = use;
                    i = result.IndexOf("MAC Address:", i);
                    arr[temp].MAC = result.Substring(i + 13, result.IndexOf(" ", i + 14) - (i + 13));
                    temp++;
                }
                temp = 0;

                // TODO
                // Compare arr[temp] with the list of devices located on DB. Grab each devices from DB and search for it on the device
                // if not found, prompt the app
                // if found, do nothing



                while (arr[temp].IP != "0.0.0.0")
                {
                    //arrS[temp] = "Device # " + (temp + 1) + "\n" +
                    //    arr[temp].IP + "\n" +
                    //    arr[temp].MAC;
                    temp++;
                    
                }
                tempDevices = string.Join("\n\n", arrS.Where(s => !string.IsNullOrEmpty(s)));

                // Clearing string array
                //Array.Clear(arrS, 0, arrS.Length);
            }
            numDevices = temp;

            Debug.WriteLine(numDevices);
            arrCopy = arr;
            Debug.WriteLine(arrCopy[0].IP);
            Debug.WriteLine(arrCopy[1].IP);
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
            Debug.WriteLine("This is the button page!");
        }

        protected override void OnDisappearing()
        {
            stopTimer();
            Debug.WriteLine("Leaving the button page!");
        }
    }
}