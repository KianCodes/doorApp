using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        private string[] arrRules = new string[255];
        private string numRule = "";
        List<string> listDays = new List<string>();

        public Page2()
        {
            InitializeComponent();
            // Initializing buttons
            btnActivate.Clicked += BtnActivate_Clicked;
            btnDeactivate.Clicked += BtnDeactivate_Clicked;

            // Scanning firewall rules and setting the ruleNumber for Time operability
            firewallRules();
            
        }

        private void BtnActivate_Clicked(object sender, EventArgs e)
        {
            addTimeRule();
            //Debug.WriteLine(cMonday.IsChecked);
            //Debug.WriteLine(cTuesday.IsChecked);

            //Debug.WriteLine(timeStartPicker.Time);
            //Debug.WriteLine(timeEndPicker.Time);
        }

        private void BtnDeactivate_Clicked(object sender, EventArgs e)
        {
            // Calling iptables -S and parsing the information to gather needed data to remove the rule
            //firewallRules();

            // Parsing array to remove data
            removeTimeRule();
        }


        private void addTimeRule()
        {
            string setFWstart = "http://192.168.64.2/cgi-bin/luci/command/cfg209944/";
            string setFWstop = "http://192.168.64.2/cgi-bin/luci/command/cfg219944/";
            string setFWdays = "http://192.168.64.2/cgi-bin/luci/command/cfg229944/";
            string setEnabled = "http://192.168.64.2/cgi-bin/luci/command/cfg239944/1";
            string restartFW = "http://192.168.64.2/cgi-bin/luci/command/cfg119944";

            string days = "";
            string startTime = "";
            string stopTime = "";
            string startTimeASCII = "";
            string stopTimeASCII = "";

            TimeSpan UTC = new TimeSpan(8,0,0);

            // Adding days to list if they were selected
            if (cMonday.IsChecked)
                listDays.Add("Mon");
            if (cTuesday.IsChecked)
                listDays.Add("Tue");
            if (cWednesday.IsChecked)
                listDays.Add("Wed");
            if (cThursday.IsChecked)
                listDays.Add("Thu");
            if (cFriday.IsChecked)
                listDays.Add("Fri");
            if (cSaturday.IsChecked)
                listDays.Add("Sat");
            if (cSunday.IsChecked)
                listDays.Add("Sun");

            
            // Converting list into a string with a delimeter of " "
            days = string.Join(" ", listDays.ToArray());
            listDays.Clear();

            // Obtaining start and stop times 
            //startTime = timeStartPicker.Time.ToString();
            //stopTime = timeEndPicker.Time.ToString();

            Debug.WriteLine(DateTime.Parse(timeStartPicker.Time.ToString()).AddHours(8).ToString("HH:mm:ss")); 
            Debug.WriteLine(DateTime.Parse(timeEndPicker.Time.ToString()).AddHours(8).ToString("HH:mm:ss"));
            Debug.WriteLine(days);

            startTime = DateTime.Parse(timeStartPicker.Time.ToString()).AddHours(8).ToString("HH:mm:ss");
            stopTime = DateTime.Parse(timeEndPicker.Time.ToString()).AddHours(8).ToString("HH:mm:ss");

            startTimeASCII = startTime.Substring(0, 2) + "%3A" + startTime.Substring(3, 2) + "%3A" + startTime.Substring(6, 2);
            stopTimeASCII = stopTime.Substring(0, 2) + "%3A" + stopTime.Substring(3, 2) + "%3A" + stopTime.Substring(6, 2);

            // Executing URL commands
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {

                client.BaseAddress = new Uri(setFWstart + startTimeASCII);
                HttpResponseMessage response1 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result1 = response1.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result1);

                client.BaseAddress = new Uri(setFWstop + stopTimeASCII);
                HttpResponseMessage response2 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result2 = response2.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result2);

                client.BaseAddress = new Uri(setFWdays + days);
                HttpResponseMessage response3 = client.GetAsync("").Result;
                response3.EnsureSuccessStatusCode();
                string result3 = response3.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result3);

                client.BaseAddress = new Uri(setEnabled);
                HttpResponseMessage responseX = client.GetAsync("").Result;
                responseX.EnsureSuccessStatusCode();
                string resultX = responseX.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + resultX);

                client.BaseAddress = new Uri(restartFW);
                HttpResponseMessage response4 = client.GetAsync("").Result;
                response4.EnsureSuccessStatusCode();
                string result4 = response4.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result4);
            }
        }

        private void firewallRules()
        {
            string scanFirewall = "http://192.168.64.2/cgi-bin/luci/command/cfg1c9944";


            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(scanFirewall);
                HttpResponseMessage response1 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result1 = response1.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result1);
                string[] str = result1.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                arrRules = str;
            }

            // Finding index of array that has the corresponding rule
            for (int i = 0; i < arrRules.Length; i++)
            {
                if (arrRules[i].Contains(".weekdays"))
                {
                    numRule = arrRules[i].Substring(arrRules[i].IndexOf("[") + 1, 2);
                    break;
                }
            }



        }

        private void removeTimeRule()
        {
            string remFWstart = "http://192.168.64.2/cgi-bin/luci/command/cfg1d9944";
            string remFWstop = "http://192.168.64.2/cgi-bin/luci/command/cfg1e9944";
            string remFWdays = "http://192.168.64.2/cgi-bin/luci/command/cfg1f9944";
            string setEnabled = "http://192.168.64.2/cgi-bin/luci/command/cfg239944/0";
            string restartFW = "http://192.168.64.2/cgi-bin/luci/command/cfg119944";

            // Executing URL commands
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {

                client.BaseAddress = new Uri(remFWstart);
                HttpResponseMessage response1 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result1 = response1.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result1);

                client.BaseAddress = new Uri(remFWstop);
                HttpResponseMessage response2 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result2 = response2.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result2);

                client.BaseAddress = new Uri(remFWdays);
                HttpResponseMessage response3 = client.GetAsync("").Result;
                response3.EnsureSuccessStatusCode();
                string result3 = response3.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result3);

                client.BaseAddress = new Uri(setEnabled);
                HttpResponseMessage responseX = client.GetAsync("").Result;
                responseX.EnsureSuccessStatusCode();
                string resultX = responseX.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + resultX);

                client.BaseAddress = new Uri(restartFW);
                HttpResponseMessage response4 = client.GetAsync("").Result;
                response4.EnsureSuccessStatusCode();
                string result4 = response4.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result4);
            }

        }

    }



}