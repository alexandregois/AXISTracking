using System;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
	public class ViewCellBase : ViewCell
	{

		private Int32 _enumPage { get; set; }

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create(
				"SelectedBackgroundColor"
				, typeof(Color)
				, typeof(ListDetalhes_ViewCell)
				, Color.Default
			);

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		public ViewCellBase(Color paramColor)
		{
			SelectedBackgroundColor = paramColor;
		}
	}

}
