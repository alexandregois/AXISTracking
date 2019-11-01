using axistracking.ViewModels.Base;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
	public class DefaultPageTemplate : Grid
	{
		public ViewModelBase _viewModel;
		protected App _app => (Application.Current as App);

		public DefaultPageTemplate()
		{
			ColumnSpacing = 0;
			RowSpacing = 0;
			VerticalOptions = LayoutOptions.FillAndExpand;
			Margin = _app.DefaultTemplateMargin;
			WidthRequest = _app.ScreenWidth;

			GridColumnDefinitions();
			GridRowDefinition();
			GridAddChildren();
		}

		private void GridColumnDefinitions()
		{
			ColumnDefinitions = new ColumnDefinitionCollection();

			ColumnDefinition column = new ColumnDefinition()
			{
				Width = GridLength.Star
			};

			ColumnDefinitions.Add(column);
		}

		private void GridRowDefinition()
		{
			RowDefinition row01 = new RowDefinition()
			{
				Height = _app.DefaultTemplateHeightNavegationBar
			};

			RowDefinition row03 = new RowDefinition()
			{
				Height = _app.DefaultTemplateHeightContent
			};

			RowDefinitions = new RowDefinitionCollection();
			RowDefinitions.Add(row01);
			RowDefinitions.Add(row03);
		}

		private void GridAddChildren()
		{

			Children.Add(MontaGridCabecalho(), 0, 0);

			ContentPresenter contentPresenter = new ContentPresenter()
			{
				Margin = new Thickness(0),
				WidthRequest = this.WidthRequest,
				HeightRequest = _app.DefaultTemplateHeightContent
			};

			Children.Add(contentPresenter, 0, 1);

		}

		public Grid MontaGridCabecalho()
		{
			Grid collum0 = new Grid()
			{
				RowSpacing = 0,
				ColumnSpacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = _app.DefaultTemplateHeightNavegationBar,
				//BackgroundColor = Color.FromHex("#FF363636")
                BackgroundColor = Color.FromHex("#1C1C22")
            };

			RowDefinition row01 = new RowDefinition()
			{
				Height = GridLength.Star
			};

			collum0.RowDefinitions = new RowDefinitionCollection();
			collum0.RowDefinitions.Add(row01);

			collum0.ColumnDefinitions = new ColumnDefinitionCollection();

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Auto
			});

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Star
			});

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Auto
			});

			Frame boxLeft = new Frame()
			{
				Margin = new Thickness(5, 0),
				Padding = 0,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Transparent,
				HeightRequest = collum0.HeightRequest,
				CornerRadius = 0,
				Opacity = 1
			};

			boxLeft.SetBinding(
				Frame.ContentProperty
				, new TemplateBinding("Parent.BindingContext.BoxLeftContent")
			);
			collum0.Children.Add(boxLeft, 0, 0);

			Frame boxMiddle = new Frame()
			{
				HasShadow = false,
				Margin = boxLeft.Margin,
				Padding = 0,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Transparent,
				HeightRequest = collum0.HeightRequest,
				CornerRadius = 0,
				Opacity = 1
			};

			boxMiddle.SetBinding(
				Frame.ContentProperty
				, new TemplateBinding("Parent.BindingContext.BoxMiddleContent")
			);
			collum0.Children.Add(boxMiddle, 1, 0);

			Frame boxRight = new Frame()
			{
				Margin = boxLeft.Margin,
				Padding = 0,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Transparent,
				HeightRequest = collum0.HeightRequest,
				CornerRadius = 0,
				Opacity = 1
			};

			boxRight.SetBinding(
				Frame.ContentProperty
				, new TemplateBinding("Parent.BindingContext.BoxRightContent")
			);
			collum0.Children.Add(boxRight, 2, 0);

			return collum0;
		}
	}
}

