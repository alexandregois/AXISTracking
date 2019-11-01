using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace axistracking.UITestMultiplataforma
{
    public class AppInitializer
    {
		
        public static IApp StartApp(Platform platform)
        {
			
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
					.ApkFile("../../../Droid/bin/Debug/br.com.systemsat.axistracking.apk")
					.StartApp();
			}

			return ConfigureApp
				.iOS
				.AppBundle("../../../iOS/bin/iPhoneSimulator/Debug/axistracking.iOS.app")
				.StartApp();//
        }

		public static void CloseKeyboard(Platform platform, IApp app)
		{
			if(platform == Platform.iOS)
			{
				(app as Xamarin.UITest.iOS.iOSApp)?.PressEnter();
			}
			else
			{
				(app as Xamarin.UITest.Android.AndroidApp)?.PressUserAction();
			}
		}
    }
}

