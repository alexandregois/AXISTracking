using axistracking.iOS.CustomElements;
using axistracking.Views.Template;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCellBase), typeof(CustomViewCellBase))]
namespace axistracking.iOS.CustomElements
{
	public class CustomViewCellBase: ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			UITableViewCell cell = base.GetCell(item, reusableCell, tv);
			ViewCellBase view = item as ViewCellBase;

			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			cell.SelectedBackgroundView = new UIView
			{
				BackgroundColor = view.SelectedBackgroundColor.ToUIColor()
			};

			return cell;
		}

	}
}