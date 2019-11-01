using System;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Telephony;
using axistracking.Droid.CustomClass;
using axistracking.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(UtilPlataform))]
namespace axistracking.Droid.CustomClass
{
	public class UtilPlataform : IUtilPlataform
	{

		public double GetAlturaStatusBar()
		{
			return 0;
		}

        public string GetIdentifier()
        {

            String ret = "";

            try
            {

                String strDeviceId = Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

                Context contexto = Android.App.Application.Context;
                TelephonyManager telephonyManager =
                    (TelephonyManager)contexto.GetSystemService(Context.TelephonyService);


                if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.O)
                {
                    try
                    {
                        ret = telephonyManager.GetDeviceId(0);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ret = telephonyManager.GetDeviceId(1);
                        }
                        catch (Exception)
                        {
                            ret = telephonyManager.DeviceId;
                        }
                    };
                }
                else
                {
                    try
                    {
                        ret = telephonyManager.GetImei(0);
                        if (ret == null)
                            ret = strDeviceId;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ret = strDeviceId; //telephonyManager.GetImei(1);
                            //if (ret == null)
                            //    ret = strDeviceId;
                        }
                        catch (Exception)
                        {
                            ret = telephonyManager.Imei;
                        }
                    };
                }

            }
            catch (Exception) { }

            return ret;
        }


        public string GetAppName()
		{
			String label = "2Min Tracking";
			try
			{
				Context contexto = Android.App.Application.Context;
				ApplicationInfo applicationInfo = contexto.ApplicationInfo;
				int stringId = applicationInfo.LabelRes;
				label = stringId == 0 ? 
					applicationInfo.NonLocalizedLabel.ToString() 
				                   : contexto.GetString(stringId);	
			} catch(Exception) { }

			return label;
		}

		public double GetScreenWidth()
		{

			return (Resources.System.DisplayMetrics.WidthPixels 
			        / Resources.System.DisplayMetrics.Density);
		}

		public double GetHeightStatusBar()
		{
			int result = 0;
			int resourceId = Resources.System.GetIdentifier("status_bar_height", "dimen", "android");
			if (resourceId > 0)
			{
				result = Resources.System.GetDimensionPixelSize(resourceId);
			}
			return result;
		}

		public double GetScreenHeight()
		{
			return ((Resources.System.DisplayMetrics.HeightPixels
			         - GetHeightStatusBar()) / Resources.System.DisplayMetrics.Density);
		}

		public double GetDeviceFontSize()
		{
			//https://stackoverflow.com/questions/17228378/device-font-size
			double scale = Resources.System.Configuration.FontScale;
			//GetResources().getConfiguration().fontScale;
			return scale;
		}

        public void OpenWaze(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(wazeURL));
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException ex)
            {
                // If Waze is not installed, open it in Google Play:
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.waze"));
                Android.App.Application.Context.StartActivity(intent);
            }

        }

        public void OpenGoogleMaps(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(wazeURL));
                intent.SetPackage("com.google.android.apps.maps");
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException ex)
            {
                // If Waze is not installed, open it in Google Play:
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.google.android.apps.maps"));
                Android.App.Application.Context.StartActivity(intent);
            }

        }

    }
}
