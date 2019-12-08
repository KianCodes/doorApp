using doorApp.ViewModel;
using System;
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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            this.BindingContext = new MenuPageViewModel(this.Navigation);
            InitializeComponent();
        }
    }
}