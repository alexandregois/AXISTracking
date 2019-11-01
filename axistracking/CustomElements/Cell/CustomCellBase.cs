using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace axistracking.CustomElements.Cell
{
	public class CustomCellBase : ViewCell
	{
		public String CellLabel { get; set; }
		public String CellPlaceholder { get; set; }
		public String CellText { get; set; }

		public IList<Behavior> Behaviors { get; }

		public Thickness MarginLabel { get; set; }
		public Thickness MarginEntry { get; set; }
		public Thickness MarginStack { get; set; }
		public Double CellFontSize { get; set; }
		public Double CellFontSizeMenor { get; set; }

		public CustomCellBase()
		{
			Behaviors = new List<Behavior>();
			MarginStack = new Thickness(
				20
				, 10
			);
			MarginLabel = new Thickness(
				0
				, 0
				, 0
				, 5
			);
			MarginEntry = 0;
			CellFontSize = 16;
			CellFontSizeMenor = 12;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if(String.IsNullOrWhiteSpace(CellLabel))
			{
				MarginEntry = new Thickness(
					MarginLabel.Left
					, MarginLabel.Top
					, MarginLabel.Right
					, MarginLabel.Top
				);
			}
		}
	}
}
