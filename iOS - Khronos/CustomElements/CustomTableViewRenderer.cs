using System;
using axistracking.CustomElements;
using axistracking.iOS.CustomElements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTableView), typeof(CustomTableViewRenderer))]
namespace axistracking.iOS.CustomElements
{
	public class CustomTableViewRenderer : TableViewRenderer
	{
		
		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
				return;

			var tableView = Control as UITableView;
			var coloredTableView = Element as CustomTableView;

			tableView.LayoutMargins = UIEdgeInsets.Zero;

			tableView.RowHeight = UITableView.AutomaticDimension;
			tableView.SectionFooterHeight = 0;
			tableView.SectionHeaderHeight = 0;

			tableView.WeakDelegate = new CustomHeaderTableModelRenderer(coloredTableView);

		}

		private class CustomHeaderTableModelRenderer : UnEvenTableViewModelRenderer
		{
			private readonly CustomTableView _coloredTableView;
			public CustomHeaderTableModelRenderer(TableView model) : base(model)
			{
				_coloredTableView = model as CustomTableView;
			}

			public override UIView GetViewForHeader(UITableView tableView, nint section)
			{
				UIView temp = base.GetViewForHeader(tableView, section);

				if(temp == null)
				{
					tableView.SectionHeaderHeight = 10;
				}
				else
				{
					
				}

				return temp;
			}
		}
	}
}
