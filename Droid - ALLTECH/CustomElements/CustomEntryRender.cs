using System;
using Android.Content;
using axistracking.CustomElements;
using axistracking.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRender))]
namespace axistracking.Droid.CustomElements
{
	public class CustomEntryRender: EntryRenderer
	{
		public CustomEntryRender(Context context) : base(context)
		{
		}

		protected override void OnElementPropertyChanged(
			object sender
			, System.ComponentModel.PropertyChangedEventArgs e
		)
		{
			base.OnElementPropertyChanged(sender, e);

			CustomEntry temp = (CustomEntry)Element;

			if(!String.IsNullOrWhiteSpace(temp.ErrorMessage))
			{
				Control.Error = temp.ErrorMessage;
			}
			else
			{
				Control.Error = null;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
				//CustomEntry temp = (CustomEntry)Element;

				//Control.Error = temp.ErrorMessage;
			}

		}

	}
}