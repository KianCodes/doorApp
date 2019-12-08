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
            this.BindingContext = new MenuPageViewModel(this.Navigation);
            InitializeComponent();
            btnFingerprint.Clicked += BtnFingerprint_Clicked;
        }

        private void BtnFingerprint_Clicked(object sender, EventArgs e)
        {
            fingerprint();
        }


        //Function to prompt the fingerprint to pop up
        //Currently does not close the application if failure
        //Need to fix
        private async void fingerprint()
        {
            bool result = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (result)
            {
                var auth = await CrossFingerprint.Current.AuthenticateAsync("Please use your fingerprint for authentication");
                if (auth.Authenticated)
                {
                    await Navigation.PushAsync(new MenuPage());
                    Navigation.RemovePage(this);
                }
                else
                {
                    await DisplayAlert("Failed", "Fingerprint authentication failed, app is closing.", "OK");
                    var closer = DependencyService.Get<ICloseApplication>();
                    closer?.closeApplication();
                }
            }
        }

        private void btnFingerprint_Clicked(object sender, EventArgs e)
        {

        }
    }
}