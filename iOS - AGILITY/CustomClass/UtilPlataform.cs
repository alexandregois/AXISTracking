using System;
using axistracking.iOS.CustomClass;
using axistracking.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(UtilPlataform))]
namespace axistracking.iOS.CustomClass
{
	public class UtilPlataform : IUtilPlataform
	{
		public double GetAlturaStatusBar()
		{
			return UIApplication.SharedApplication.StatusBarFrame.Height;
		}

		public string GetAppName()
		{
			return "SSX Tracking";
		}

		public double GetScreenWidth()
		{
			return UIScreen.MainScreen.Bounds.Width;
		}

		public double GetHeightStatusBar()
		{
			return UIApplication.SharedApplication.StatusBarFrame.Height;
		}

		public double GetScreenHeight()
		{
			return UIScreen.MainScreen.Bounds.Height;
		}

        public string GetIdentifier()
        {
            string serial = string.Empty;
            try
            {
                NSUuid tempImei = UIDevice.CurrentDevice.IdentifierForVendor;
                String str = tempImei.AsString().Replace("-", "");

                if (str.Length > 50)
                {
                    serial = str.Substring(0, 50);
                }
                else
                {
                    serial = str;
                }

            }
            catch (Exception) { }

            return serial;
        }


        public double GetDeviceFontSize()
		{
			//http://samwize.com/2016/05/05/dynamic-type-changes-text-sizes-and-fonts-automatically/
			//https://stackoverflow.com/questions/20510094/how-to-use-a-custom-font-with-dynamic-text-sizes-in-ios7/20510095#20510095
			//https://stablekernel.com/supporting-accessibility-larger-text-in-ios/
			String scale = UIApplication.SharedApplication.PreferredContentSizeCategory;
			//UICTContentSizeCategoryAccessibilityXXXL
			//UICTContentSizeCategoryXXXL
			//UICTContentSizeCategoryXS
			//UICTContentSizeCategoryXL
			//GetResources().getConfiguration().fontScale;

			//switch (UIApplication.SharedApplication.PreferredContentSizeCategory) {
			//case UIContentSizeCategoryAccessibilityExtraExtraExtraLarge: return 23 / 16
			//case UIContentSizeCategoryAccessibilityExtraExtraLarge: return 22 / 16
			//case UIContentSizeCategoryAccessibilityExtraLarge: return 21 / 16
			//case UIContentSizeCategoryAccessibilityLarge: return 20 / 16
			//case UIContentSizeCategoryAccessibilityMedium: return 19 / 16
			//case UIContentSizeCategoryExtraExtraExtraLarge: return 19 / 16
			//case UIContentSizeCategoryExtraExtraLarge: return 18 / 16
			//case UIContentSizeCategoryExtraLarge: return 17 / 16
			//case UIContentSizeCategoryLarge: return 1.0
			//case UIContentSizeCategoryMedium: return 15 / 16
			//case UIContentSizeCategorySmall: return 14 / 16
			//case UIContentSizeCategoryExtraSmall: return 13 / 16
			//default: return 1.0;
			String scale2 = UIContentSizeCategory.ExtraLarge.ToString();

			System.Diagnostics.Debug.WriteLine(scale);
			System.Diagnostics.Debug.WriteLine(scale2);
			System.Diagnostics.Debug.WriteLine(UIFontDescriptor.PreferredBody);

			return 10;
		}

        public void OpenWaze(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                Device.OpenUri(new Uri(wazeURL));
            }
            catch (Exception)
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/app/id323229106"));
            }
        }

        public void OpenGoogleMaps(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                Device.OpenUri(new Uri(wazeURL));
            }
			catch (Exception)
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/app/id585027354"));
            }
        }

        public void Write(String paramTexto)
		{
			Console.WriteLine (paramTexto);
		}
	}
}
