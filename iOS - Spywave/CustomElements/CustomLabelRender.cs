using System;
using axistracking.CustomElements;
using axistracking.iOS.CustomElements;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRender))]
namespace axistracking.iOS.CustomElements
{
	public class CustomLabelRender: LabelRenderer
	{
		protected override void OnElementChanged(
			ElementChangedEventArgs<Label> e
		)
		{
			if (Control == null)
			{
				SetNativeControl(new TagUiLabel(e.NewElement as CustomLabel));
			}
			
			base.OnElementChanged(e);

			//if (Control != null)
			//{
				
			//}
		}


	}


	public class TagUiLabel : UILabel 
	{

		UIEdgeInsets insets = UIEdgeInsets.Zero;
		private UIEdgeInsets _edgeInsets 
		{
			get {
				return insets;
			}
			set 
			{
				insets = value;
				SetNeedsDisplay();
			}
		}

		public TagUiLabel(CustomLabel paramCustomLabel)
		{
			_edgeInsets = new UIEdgeInsets(
				new nfloat(paramCustomLabel.Padding.Top)
				, new nfloat(paramCustomLabel.Padding.Left)
				, new nfloat(paramCustomLabel.Padding.Bottom)
				, new nfloat(paramCustomLabel.Padding.Right)
			);
		}
		public override void DrawText (
			CoreGraphics.CGRect rect
		)
		{
			base.DrawText(_edgeInsets.InsetRect(rect));
		}

		public override CGSize IntrinsicContentSize
		{
			get
			{
				var originalSize = base.IntrinsicContentSize;

				originalSize.Width += _edgeInsets.Left + _edgeInsets.Right;
				originalSize.Height += _edgeInsets.Top + _edgeInsets.Bottom;

				return originalSize;
			}
		}
	}
}
