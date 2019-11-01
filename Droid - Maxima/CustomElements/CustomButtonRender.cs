using System;
using Android.Content;
using axistracking.CustomElements;
using axistracking.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRender))]
namespace axistracking.Droid.CustomElements
{
	public class CustomButtonRender : ButtonRenderer
	{
		public CustomButtonRender(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			CustomButton customButton = e.NewElement as CustomButton;

			Android.Widget.Button thisButton = this.Control as Android.Widget.Button;
			thisButton.SetPadding(0, 0, 0, 0);
		}

	}
}