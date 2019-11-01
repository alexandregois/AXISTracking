
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Telephony;
using axistracking.Droid.CustomClass;
using Com.OneSignal;

namespace axistracking.Droid
{
    [Activity(
        Label = "2Min Tracking"
        , Icon = "@drawable/ic_launcher"
        , Theme = "@style/MyTheme"
        , MainLauncher = true
        , ConfigurationChanges = ConfigChanges.ScreenSize
                                | ConfigChanges.Orientation
        , ScreenOrientation = ScreenOrientation.Portrait
    )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            try
            {

                base.OnCreate(bundle);

                //Push Notification
                OneSignal.Current.StartInit("e2de4625-175f-4dcb-8b5b-d00d63b75eb6")
                   .EndInit();

                global::Xamarin.Forms.Forms.Init(this, bundle);

                global::Xamarin.FormsGoogleMaps.Init(this, bundle);

                LoadApplication(new App(true, "2minutos"));

            }
            catch (System.Exception)
            {
                LoadApplication(new App(false, "tracking"));
            }
        }
    

    }
}
