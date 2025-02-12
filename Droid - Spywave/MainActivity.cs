﻿
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
        Label = "GNS Brasil"
        , Icon = "@drawable/ic_launcher"
        , Theme = "@style/MyTheme"
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
                OneSignal.Current.StartInit("ffc1df41-2695-45d8-9957-0b38186f9579")
                   .EndInit();

                global::Xamarin.Forms.Forms.Init(this, bundle);

                global::Xamarin.FormsGoogleMaps.Init(this, bundle);

                LoadApplication(new App(true, "spywave"));

            }
            catch (System.Exception)
            {
                LoadApplication(new App(true, "spywave"));
            }
        }
    

    }
}
