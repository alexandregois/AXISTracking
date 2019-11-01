using axistracking.Enum;
using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using axistracking.Views.Template;
using Xamarin.Forms;

namespace axistracking.Views
{
	public partial class ViewModulos : ContentPage
    {
		private ViewModelModulos _viewModelModulos;
        protected App _app => (Application.Current as App);

        private EnumPage page { get; set; }

        public INavigationService _navigationService { get; set; }

        public ViewModulos()
        {
            InitializeComponent();
            
            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

            //ListPainel.ItemTemplate = new DataTemplate(() =>
            //{
            //  return new ListPainel_ViewCell((ViewModelPainel)this.BindingContext);
            //});

            this._navigationService =
                   DependencyService.Get<INavigationService>();


            _viewModelModulos = new ViewModelModulos();

			this.BindingContext = _viewModelModulos;

            MontaGrid_Modulos();

        }

        protected override void OnAppearing()
        {
            var lifecycleHandler = (ViewModelModulos)this.BindingContext;
			lifecycleHandler.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelModulos)this.BindingContext;
			lifecycleHandler.OnDisappearing();
        }

		protected override bool OnBackButtonPressed()
		{
			//Device.BeginInvokeOnMainThread(() =>
			//{
                    //             _viewModelModulos
                    //                 .Deslogar();
			//});

			base.OnBackButtonPressed();
			return true;
		}

        private void MontaGrid_Modulos()
        {

            gridModulos.RowDefinitions.Add(new RowDefinition()
            {
                Height = (_app.ScreenHeight / 100) * 13
            });

            gridModulos.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            gridModulos.RowDefinitions.Add(new RowDefinition()
            {
                Height = (_app.ScreenHeight / 100) * 15
            });

            gridModulos.RowDefinitions.Add(new RowDefinition(){
                //Height = _app.ScreenHeight / 2.5
                Height = GridLength.Auto
            });

            gridModulos.ColumnDefinitions.Add(new ColumnDefinition());
            gridModulos.ColumnDefinitions.Add(new ColumnDefinition()
            { 
                Width = 1
            });


            gridModulos.ColumnDefinitions.Add(new ColumnDefinition());


            // RASTREAMENTO
            Label lblRastreamento = new Label {
                Text = "Rastreamento",
                TextColor = Color.White
            };
            Image imageRastreamento = new Image
            {
                Source = "ic_moduloBotaoRastreamento.png"
            };
            imageRastreamento.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ToRastreamento),
                NumberOfTapsRequired = 1
            });
            StackLayout stackLayoutRastreamento = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Fill
                //,IsEnabled = _viewModelModulos.RastreamentoButtonIsEnabled
            };

            stackLayoutRastreamento.Children.Add(imageRastreamento);
            stackLayoutRastreamento.Children.Add(lblRastreamento);


            // ALERTA
            Label lblAlerta = new Label
            {
                Text = "Alertas",
                TextColor = Color.White
            };
            Image imageAlerta = new Image
            {
                Source = "ic_moduloBotaoAlerta.png"
            };
            imageAlerta.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ToAlerta),
                NumberOfTapsRequired = 1
            });
            StackLayout stackLayoutAlerta = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Fill
                //,IsEnabled = _viewModelModulos.AlertaButtonIsEnabled
            };

            stackLayoutAlerta.Children.Add(imageAlerta);
            stackLayoutAlerta.Children.Add(lblAlerta);


            // GRAFICO
            Label lblGrafico = new Label
            {
                Text = "Gráficos",
                TextColor = Color.White
            };
            Image imageGrafico = new Image
            {
                Source = "ic_moduloBotaoGrafico.png"
            };
            imageGrafico.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ToPainel),
                NumberOfTapsRequired = 1
            });
            StackLayout stackLayoutGrafico = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Fill
                //,IsEnabled = _viewModelModulos.GraficoButtonIsEnabled
            };

            stackLayoutGrafico.Children.Add(imageGrafico);
            stackLayoutGrafico.Children.Add(lblGrafico);



            // COMANDOS
            Label lblComando = new Label
            {
                Text = "Comandos",
                TextColor = Color.White

            };
            Image imageComando = new Image
            {
                Source = "ic_moduloBotaoComando.png"
            };
            imageComando.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ToComandos),
                NumberOfTapsRequired = 1
            });

            StackLayout stackLayoutComando = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                //HeightRequest = _app.ScreenHeight / 2.5,
                VerticalOptions = LayoutOptions.Fill
                //,IsEnabled = _viewModelModulos.ComandoButtonIsEnabled
            };

            stackLayoutComando.Children.Add(imageComando);
            stackLayoutComando.Children.Add(lblComando);


            StackLayout stackColuna = new StackLayout();
            stackColuna.WidthRequest = 1;
            stackColuna.BackgroundColor = Color.Gray;
            /*gridModulos.Children.Add(stackColuna, 1, 1);
            gridModulos.Children.Add(stackColuna, 1, 2);
            gridModulos.Children.Add(stackColuna, 1, 3);*/


            gridModulos.Children.Add(stackLayoutRastreamento, 0, 1);
            gridModulos.Children.Add(stackLayoutAlerta, 2, 1);
            gridModulos.Children.Add(stackLayoutGrafico, 0, 3);
            gridModulos.Children.Add(stackLayoutComando, 2, 3);

        }

        private void ToComandos()
        {
            page = EnumPage.ListaComandos;
            this._navigationService.NavigateToListarComandos((object)App.ListPainelSource[2], page);
        }

        private void ToRastreamento()
        {
            page = EnumPage.DetalhesUnidade;
            this._navigationService.NavigateToDetalhes(null, (object)App.ListPainelSource[0], page);
        }

        private void ToAlerta()
        {
            page = EnumPage.DetalhesUnidade;
            this._navigationService.NavigateToDetalhes(null, (object)App.ListPainelSource[1], page);
        }

        private void ToPainel()
        {
            page = EnumPage.Painel;
            this._navigationService.NavigateToPainel();
        }

    }

}