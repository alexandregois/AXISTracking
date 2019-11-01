
using Com.OneSignal;
using Foundation;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;

namespace axistracking.iOS
{
	[Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(
			UIApplication app
			, NSDictionary options
		)
        {
            global::Xamarin.Forms.Forms.Init();

			AppCenter.Start(
				"8b9ba4c2-f4ec-417a-af3b-2f162072ce30"
				, typeof(Analytics)
				, typeof(Crashes)
				, typeof(Push)
			);


            AppCenter.SetEnabledAsync(true);


            AppCenter.IsEnabledAsync();


            //OneSignal.Current.StartInit("e2de4625-175f-4dcb-8b5b-d00d63b75eb6")
            //.EndInit();

#if DEBUG
                Xamarin.FormsGoogleMaps.Init("AIzaSyBzIBGYnbFo3XOE6fJGYRegaQ3hn1vaBbk");
#else
            Xamarin.FormsGoogleMaps.Init("AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws");
            #endif


            // Code for starting up the Xamarin Test Cloud Agent
            #if DEBUG
                Xamarin.Calabash.Start();
            #endif

			app.StatusBarStyle = UIStatusBarStyle.LightContent;
			UIApplication.SharedApplication
			             .SetStatusBarStyle(UIStatusBarStyle.LightContent, true);

            LoadApplication(new App(false, "tracking"));


            return base.FinishedLaunching(app, options);

        }

        
		// The following Exports are needed to run OneSignal in the iOS Simulator.
		//   The simulator doesn't support push however this prevents a crash due to a Xamarin bug
		//   https://bugzilla.xamarin.com/show_bug.cgi?id=52660
		[Export("oneSignalApplicationDidBecomeActive:")]
		public void OneSignalApplicationDidBecomeActive(UIApplication application)
		{
			// Remove line if you don't have a OnActivated method.
			OnActivated(application);
		}

		[Export("oneSignalApplicationWillResignActive:")]
		public void OneSignalApplicationWillResignActive(UIApplication application)
		{
			// Remove line if you don't have a OnResignActivation method.
			OnResignActivation(application);
		}

		[Export("oneSignalApplicationDidEnterBackground:")]
		public void OneSignalApplicationDidEnterBackground(UIApplication application)
		{
			// Remove line if you don't have a DidEnterBackground method.
			DidEnterBackground(application);
		}

		[Export("oneSignalApplicationWillTerminate:")]
		public void OneSignalApplicationWillTerminate(UIApplication application)
		{
			// Remove line if you don't have a WillTerminate method.
			WillTerminate(application);
		}

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
		{
			var result = Push.DidReceiveRemoteNotification(userInfo);
			if (result)
			{
				completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
			}
			else
			{
				completionHandler?.Invoke(UIBackgroundFetchResult.NoData);
			}
		}
               
	}
}
