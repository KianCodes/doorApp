using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        int devices = 0;
        public Page1()
        {
            InitializeComponent();
            btnOrange.Clicked += BtnOrange_Clicked;
        }

        //Button to simulate adding a new card to the device list
        private void BtnOrange_Clicked(object sender, EventArgs e)
        { 
            newFrame("123", "321", devices + 1);
            devices++;
        }

        //Function to create a new card for the device list.  
        //Uses specific inputs rather than an object.
        //Will implement overloaded method using object as parameter
        private void newFrame(string ip, string mac, int device)
        {
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
    }
}