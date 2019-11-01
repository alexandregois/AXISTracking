using System;
using System.Collections.Generic;
using System.Linq;
using axistracking.Domain.Dto;
using Xamarin.Forms;
namespace axistracking.CustomElements.Cell
{
	public class CustomPickerCell : CustomCellBase
	{
		public List<FormularioDinamicoElementDto> DataSource { get; set; }
		public BindingBase ItemDisplayBinding { get; set; }

		public CustomPickerCell()
		{
			DataSource = new List<FormularioDinamicoElementDto>();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

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

			Picker entry = new Picker()
			{
				ItemDisplayBinding = ItemDisplayBinding,
				ItemsSource = DataSource,
				Margin = MarginEntry
			};

			if(!String.IsNullOrWhiteSpace(CellPlaceholder))
			{
				entry.Title = CellPlaceholder;
			}

			if(!String.IsNullOrWhiteSpace(CellText))
			{
				FormularioDinamicoElementDto temp = DataSource.FirstOrDefault(x => x.Id == CellText);
				entry.SelectedItem = temp;
			}

			stack.Children.Add(entry);

			Label errorLabel = new Label()
			{
				TextColor = Color.Red,
				FontSize = CellFontSizeMenor,
				LineBreakMode = LineBreakMode.WordWrap
			};
			stack.Children.Add(errorLabel);

			View = stack;

		}
	}
}
