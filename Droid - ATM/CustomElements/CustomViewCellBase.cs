using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using axistracking.Droid.CustomElements;
using axistracking.Views.Template;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ViewCellBase), typeof(CustomViewCellBase))]
namespace axistracking.Droid.CustomElements
{
	public class CustomViewCellBase: ViewCellRenderer
	{

		private Android.Views.View _cellCore;
		private Drawable _unselectedBackground; 
		private bool _selected;

		protected override Android.Views.View GetCellCore(Cell item, 
		                                              Android.Views.View convertView, 
		                                              ViewGroup parent, 
		                                              Context context)
		{
			_cellCore = base.GetCellCore(item, convertView, parent, context);
			_selected = false;
			_unselectedBackground = _cellCore.Background;

			return _cellCore;
		}

		#pragma warning disable RECS0133
		protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			base.OnCellPropertyChanged(sender, args);

			if (args.PropertyName == "IsSelected")
			{
				_selected = !_selected;
				
				ViewCellBase extendedViewCell = sender as ViewCellBase;
				if (_selected)
				{
					_cellCore.SetBackgroundColor(extendedViewCell.SelectedBackgroundColor.ToAndroid());
				}
				else
				{
					_cellCore.SetBackground(_unselectedBackground);
				}

				_cellCore.SetBackgroundColor(extendedViewCell.SelectedBackgroundColor.ToAndroid());
			}
		}
		#pragma warning restore RECS0133
	}

}