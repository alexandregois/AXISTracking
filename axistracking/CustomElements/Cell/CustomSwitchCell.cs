using System;
using Xamarin.Forms;
namespace axistracking.CustomElements.Cell
{
	public class CustomSwitchCell: CustomCellBase
	{
		public Switch entry { get; set; }
		public Label tempLabel { get; set; }
		
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			Grid temp = new Grid()
			{
				Margin = 0,
				Padding = MarginStack,
				RowSpacing = 0,
				ColumnSpacing = 5
			};

			temp.ColumnDefinitions = new ColumnDefinitionCollection();

			temp.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Star
			});

			temp.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Auto
			});

			temp.RowDefinitions = new RowDefinitionCollection();
			temp.RowDefinitions.Add(new RowDefinition()
			{
				Height = GridLength.Auto
			});

			tempLabel = new Label()
			{
				Text = CellLabel,
				FontSize = CellFontSize,
				LineBreakMode = LineBreakMode.WordWrap,
				VerticalTextAlignment = TextAlignment.Center
			};
			tempLabel.PropertyChanged += HandlePropertyChangedEventHandler;
			temp.Children.Add(tempLabel, 0, 0);

			entry = new Switch()
			{
				IsToggled = (CellText == "1"),
				VerticalOptions = LayoutOptions.Center
			};
			entry.PropertyChanged += Entry_PropertyChanged;
			temp.Children.Add(entry, 1, 0);

			View = temp;

		}

		void HandlePropertyChangedEventHandler(
			object sender
			, System.ComponentModel.PropertyChangedEventArgs e
		)
		{
			if(e.PropertyName == "Height")
			{
				Label senderLabel = (Label)sender;
				Grid grid = ((Grid)senderLabel.Parent);
				senderLabel.PropertyChanged -= HandlePropertyChangedEventHandler;

				Double altura = senderLabel.Height;
				if(altura < entry.Height)
					altura = entry.Height;

				senderLabel.HeightRequest = altura;
				
				grid.HeightRequest = 
					altura 
					+ grid.Margin.Top
					+ grid.Margin.Bottom;
			}
		}

		void Entry_PropertyChanged(
			object sender
			, System.ComponentModel.PropertyChangedEventArgs e
		)
		{
			if(e.PropertyName == "Height")
			{
				Switch senderSwitch = (Switch)sender;
				Grid grid = ((Grid)senderSwitch.Parent);
				senderSwitch.PropertyChanged -= Entry_PropertyChanged;

				Double altura = senderSwitch.Height;
				if(altura < tempLabel.Height)
					altura = tempLabel.Height;

				tempLabel.HeightRequest = altura;

				grid.HeightRequest = 
					altura 
					+ grid.Margin.Top
					+ grid.Margin.Bottom;
			}
		}
	}
}