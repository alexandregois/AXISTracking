
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace axistracking.Droid
{
	[Activity(
		Label = "Cliente1 - Tracking"
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

			base.OnCreate(bundle);

            ////Push Notification
            //OneSignal.Current.StartInit("e2de4625-175f-4dcb-8b5b-d00d63b75eb6").EndInit();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::Xamarin.FormsGoogleMaps.Init(this, bundle);

			LoadApplication(new App());
		}

	}
}
