using Android.Content;
using axistracking.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryRender_Droid))]
namespace axistracking.Droid.CustomElements
{
	public class EntryRender_Droid: EntryRenderer
    {
		public EntryRender_Droid(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
				Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

        }

    }
}