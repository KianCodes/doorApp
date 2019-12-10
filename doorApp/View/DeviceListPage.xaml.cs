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
    public partial class DeviceListPage : ContentPage
    {
        int devices = 0;
        public DeviceListPage()
        {
            InitializeComponent();
            btnRefresh.Clicked += BtnRefresh_Clicked;
            btnBlock.Clicked += BtnBlock_Clicked;
            btnReset.Clicked += BtnReset_Clicked;
        }

        private async void BtnReset_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Would you like to clear the device list?", "All devices currently on the list will lose internet access.", "Yes", "No");
            
        }

        private async void BtnBlock_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Would you like to block all devices?", "", "Yes", "No");

        }

        private async void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            newFrame("192.168.68.4", "08:00:27:87:78:DA","Allowed", devices + 1);
            devices++;
        }



        private void newFrame(string ip, string mac, string status, int device)
        {
            //stackTest.Children.Clear();
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
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        },
                        new Label
                        {
                            Text = $"IP Address: {ip}\nMAC Address: {mac}\nStatus: {status}"
                        }
                    }
                }
            };
            stackTest.Children.Add(cardFrame);

        }
    }
}