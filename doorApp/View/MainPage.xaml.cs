using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using doorApp.ViewModel;
using Plugin.Fingerprint;
using System.Diagnostics;
using System.Timers;
using System.Net;
using System.Net.Http;
using doorApp.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {

            fingerprint();


            this.BindingContext = new MainPageViewModel(this.Navigation);

            

        }


        //Function to prompt the fingerprint to pop up
        //Currently does not close the application if failure
        //Need to fix
        private async void fingerprint()
        {
            bool result = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (result)
            {
                var auth = await CrossFingerprint.Current.AuthenticateAsync("Authenticate");
                if (auth.Authenticated)
                {
                    InitializeComponent();
                    // Declaring button(s)
                    btnReboot.Clicked += BtnReboot_Clicked;
                }
                else
                {
                    await DisplayAlert("Failed", "Fingerprint authentication failed, app is closing.", "OK");
                    var closer = DependencyService.Get<ICloseApplication>();
                    closer?.closeApplication();
                }
            }
        }

        private async void BtnReboot_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Reboot", "Are you sure you wish to reboot the router?", "Yes", "No");
            if (answer)
            {
                // Calling fx that'll reboot the router
                rebootRouter();
            }
            
        }

        private async void rebootRouter()
        {
            // This will be currently hard coded for the static domain: 192.168.68.1
            // It will start with *.*.*.3 and above all the way to *.*.*.255
            string reboot = "http://192.168.64.2/cgi-bin/luci/command/cfg1b9944";

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(reboot);
                HttpResponseMessage response1 = client.GetAsync("").Result;
                response1.EnsureSuccessStatusCode();
                string result1 = response1.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result1);
            }

        }
    }
}