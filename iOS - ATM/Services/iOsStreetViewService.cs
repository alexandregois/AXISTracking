using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using axistracking.iOS.Services;
using axistracking.Services;
using Foundation;
using UIKit;
using Xamarin.Forms.Internals;


[assembly: Xamarin.Forms.Dependency(typeof(iOsStreetViewService))]
namespace axistracking.iOS.Services
{
    public class iOsStreetViewService : IStreetViewService
    {
        public void openStreetView(double latitude, double longitude)
        {
            if (UIApplication.SharedApplication.CanOpenUrl(new Foundation.NSUrl("comgooglemaps://")))
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl("comgooglemaps://?center=" + latitude + "," + longitude + "&mapmode=streetview"));
            else
            {
                //Log.e("dasd", "Google maps not supported");
                //UIAlertView _error = new UIAlertView("Opps", "Please install google maps to access this feature.", null, "Okay", null);
                //_error.Show();
            }
        }
    }
}