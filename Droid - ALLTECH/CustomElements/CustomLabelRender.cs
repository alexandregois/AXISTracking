using Android.Content;
using axistracking.CustomElements;
using axistracking.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRender))]
namespace axistracking.Droid.CustomElements
{
	public class CustomLabelRender : LabelRenderer
	{
		
		public CustomLabelRender(
			Context context
		) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if(Control != null)
			{
				CustomLabel customLabel = e.NewElement as CustomLabel;
				//Control.SetPadding(
				//	(int)customLabel.Padding.Left
				//	, (int)customLabel.Padding.Top
				//	, (int)customLabel.Padding.Right
				//	, (int)customLabel.Padding.Bottom
				//);
				Control.SetPadding(
					(int)customLabel.Padding.Left
					, (int)customLabel.Padding.Top
					, (int)customLabel.Padding.Right
					, (int)customLabel.Padding.Bottom
				);
				Control.RefreshDrawableState();
			}
		}
	}
}
