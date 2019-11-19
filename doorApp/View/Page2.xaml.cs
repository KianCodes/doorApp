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
    public partial class Page2 : ContentPage
    {
        int devices = 0;
        public Page2()
        {
            InitializeComponent();
            btn1.Clicked += Btn1_Clicked;
        }

        private async void Btn1_Clicked(object sender, EventArgs e)
        {
            newFrame("123", "321", devices + 1);
            devices++;
        }


        private void newFrame(string ip, string mac, int device)
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
                            Text = $"IP Address: {ip}\nMAC Address: {mac}"
                        }
                    }
                }
            };
            stackTest.Children.Add(cardFrame);

        }
    }
}