
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
using Firebase;

namespace axistracking.Droid
{
    [Activity(
        Label = "Khronos Tracking"
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

                //Push Notification - Khronos Tracking
                OneSignal.Current.StartInit("c93df7ac-74ff-4eb8-b69d-3a35d8dbd74d")
                   .EndInit();

                global::Xamarin.Forms.Forms.Init(this, bundle);

                global::Xamarin.FormsGoogleMaps.Init(this, bundle);

                FirebaseOptions options = new FirebaseOptions.Builder()
                   .SetApplicationId("br.com.khronosnet.tracking")
                   .SetDatabaseUrl("https://ssx-khronos-tracking.firebaseio.com")
                   .SetGcmSenderId("285882600569")
                   .Build();

                var apps = Firebase.FirebaseApp.GetApps(this);
                if (apps.Count == 0)
                {
                    FirebaseApp.InitializeApp(this, options);
                }


                //Firebase.FirebaseApp.InitializeApp(this);
                //Firebase.Iid.FirebaseInstanceId.GetInstance(Firebase.FirebaseApp.InitializeApp(this));

                LoadApplication(new App(true, "khronos"));

            }
            catch (System.Exception)
            {
                LoadApplication(new App(true, "khronos"));
            }
        }
    

    }
}
