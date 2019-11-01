using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using axistracking.Droid.Services;
using axistracking.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DroidStreetViewService))]
namespace axistracking.Droid.Services
{
    public class DroidStreetViewService : IStreetViewService
    {
        public void openStreetView(double latitude, double longitude)
        {
            Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse("google.streetview:cbll=" + latitude + "," + longitude);
            Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
            mapIntent.SetPackage("com.google.android.apps.maps");
            Android.App.Application.Context.StartActivity(mapIntent);

        }
    }
}