using axistracking.CustomElements;
using axistracking.iOS.CustomElements;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRender))]
namespace axistracking.iOS.CustomElements
{
	public class CustomEntryRender : EntryRenderer
	{
		protected override void OnElementPropertyChanged(
			object sender
			, System.ComponentModel.PropertyChangedEventArgs e
		)
		{
			base.OnElementPropertyChanged(sender, e);



		}

		protected override void OnElementChanged(
			ElementChangedEventArgs<Entry> e
		)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				CustomEntry temp = e.NewElement as CustomEntry;
				if(temp.BorderStyle == "Hide")
					Control.BorderStyle = UIKit.UITextBorderStyle.None;
			}

		}
	}
}
