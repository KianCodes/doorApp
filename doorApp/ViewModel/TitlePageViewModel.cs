using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doorApp.ViewModel
{
    public class TitlePageViewModel : BasePageViewModel
    {
        public ICommand NavigateCommand { get; private set; }

        public TitlePageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            NavigateCommand = new Command<Type>(OnNavigateCommand);
        }

        private INavigation Navigation { get; set; }

        private async void OnNavigateCommand(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await this.Navigation.PushAsync(page);
        }
    }
}
