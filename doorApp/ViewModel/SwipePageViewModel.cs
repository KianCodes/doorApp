using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MLToolkit.Forms.SwipeCardView.Core;
using Xamarin.Forms;
using doorApp.Model;



namespace doorApp.ViewModel
{
    public class SwipePageViewModel : BasePageViewModel
    {
        private ObservableCollection<Devices> _deviceItems = new ObservableCollection<Devices>();
        private int counter = 0;
        public SwipePageViewModel()
        {
            //Check DB intially.
            CheckDB();

            this.SwipedCommand = new Command<SwipedCardEventArgs>(this.OnSwipedCommand);
            this.DraggingCommand = new Command<DraggingCardEventArgs>(this.OnDraggingCommand);
            this.RefreshCommand = new Command<Type>(OnRefreshCommand);
        }

        public ObservableCollection<Devices> Devices
        {
            get => _deviceItems;
            set
            {
                _deviceItems = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }
        public ICommand RefreshCommand { get; }

        private void OnRefreshCommand(Type obj)
        {
            CheckDB();
        }
        private void CheckDB()
        {
            counter = 0;
            this.Devices.Clear();
            // Adding a new device as a card
            for(int i = 1; i <= 10; i++)
                this.Devices.Add(new Devices { name = $"Device {i}", ipAddress = "IP Address: 192.168.11.11", macAddress = "MAC Address: 192.168.22.22", status = "Status: ACCEPTED/REJECTED" });
            
        }
        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            // If the user swipes right or presses accept
            if(eventArgs.Direction.ToString() == "Right")
            {
                Console.WriteLine($"Rejected Device {_deviceItems[counter].name}, IP Address: {_deviceItems[counter].ipAddress}," +
                    $"MAC Address: {_deviceItems[counter].macAddress}");
                counter++;
            }
            // If the user swipes left or presses reject
            else if (eventArgs.Direction.ToString() == "Left")
            {
                //Example accessing and taking information from device collection list
                Console.WriteLine($"Rejected Device {_deviceItems[counter].name}, IP Address: {_deviceItems[counter].ipAddress}," +
                    $"MAC Address: {_deviceItems[counter].macAddress}");
                counter++;
            }
        }
        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;
                case DraggingCardPosition.UnderThreshold:
                    break;
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    return;
                case DraggingCardPosition.FinishedOverThreshold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}