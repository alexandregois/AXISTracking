using System;
using Xamarin.Forms;
namespace axistracking.CustomElements.Cell
{
	public class CustomEntryCell : CustomCellBase
	{
		public Keyboard Keyboard { get; set; }
		public CustomEntryCell Temp { get; set; }

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Temp = this;
			StackLayout stack = new StackLayout()
			{
				Margin = 0,
				Padding = MarginStack,
				Spacing = 0
			};

			if(!String.IsNullOrWhiteSpace(CellLabel))
			{
				Label tempLabel = new Label()
				{
					Text = CellLabel,
					FontSize = CellFontSize,
					LineBreakMode = LineBreakMode.WordWrap,
					Margin = MarginLabel
				};
				stack.Children.Add(tempLabel);
			}

			CustomEntry tempEntry = new CustomEntry()
			{
				Placeholder = CellPlaceholder,
				Margin = MarginEntry,
				FontSize = CellFontSize
			};
			if(!String.IsNullOrWhiteSpace(CellText))
				tempEntry.Text = CellText;
			
			if(Keyboard != null)
				tempEntry.Keyboard = Keyboard;

			foreach(Behavior item in Behaviors)
			{
				tempEntry.Behaviors.Add(item);
			}

			stack.Children.Add(tempEntry);
			View = stack;

		}
	}
}
