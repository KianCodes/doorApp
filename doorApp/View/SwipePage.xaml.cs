using System;
using MLToolkit.Forms.SwipeCardView.Core;
using doorApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwipePage : ContentPage
    {
        public SwipePage()
        {
            InitializeComponent();
            this.BindingContext = new SwipePageViewModel();
            SwipeCardView.Dragging += OnDragging;
        }

        private void OnDragging(object sender, DraggingCardEventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            switch (e.Position)
            {
                case DraggingCardPosition.Start:
                    break;
                case DraggingCardPosition.UnderThreshold:
                    view.BackgroundColor = Color.AntiqueWhite;
                    break;
                case DraggingCardPosition.OverThreshold:
                    switch (e.Direction)
                    {
                        case SwipeCardDirection.Left:
                            view.BackgroundColor = Color.FromHex("#FF6A4F");
                            break;
                        case SwipeCardDirection.Right:
                            view.BackgroundColor = Color.FromHex("#63DD99");
                            break;
                    }
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    view.BackgroundColor = Color.White;
                    break;
                case DraggingCardPosition.FinishedOverThreshold:
                    view.BackgroundColor = Color.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRejectClicked(object sender, EventArgs e)
        {
            this.SwipeCardView.InvokeSwipe(SwipeCardDirection.Left);
        }
        private void OnRefreshClicked(object sender, EventArgs e)
        {

        }
        private void OnAcceptClicked(object sender, EventArgs e)
        {
            this.SwipeCardView.InvokeSwipe(SwipeCardDirection.Right);
        }
    }
}