
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace axistracking.Droid
{
	[Activity(
		Label = "GNS Brasil"
		, Icon = "@drawable/ic_launcher"
		, Theme = "@style/MyTheme.Splash"
		, MainLauncher = true
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait
	)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            //SetContentView(Resource.Layout.Main);
        }

		// Launches the startup task
		protected override void OnResume()
		{
			base.OnResume();
			SimulateStartup();
		}

		// Prevent the back button from canceling the startup process
		public override void OnBackPressed() { }

		// Simulates background work that happens behind the splash screen
		public void SimulateStartup()
		{
			StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));
		}

		//public override void OnRequestPermissionsResult(int requestCode, string[] permissions
		//                                                , Permission[] grantResults)
		//{
		//	PermissionsHandler.CheckPermissions(Forms.Context, this);
		//}

	}
}
