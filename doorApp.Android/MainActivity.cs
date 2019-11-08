using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;
using doorApp.Interfaces;
using System.Threading.Tasks;
using System.Configuration;
using static doorApp.Droid.MainActivity;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace doorApp.Droid
{
    [Activity(Label = "doorApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            

            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public class CloseApplication : ICloseApplication
        {
            public void closeApplication()
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }

            public Task PushAsync()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}