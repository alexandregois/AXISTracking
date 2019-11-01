using System;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.ViewModels;
using axistracking.Views.Interface;
using axistracking.Views.Services;
using axistracking.Views.Template;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using axistracking.Resx;

namespace axistracking.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewHistorico : ContentPage, IViewHistorico
    {
        public Map mapaHistorico { get; set; }
        public Boolean exibeBuscarMais { get; set; }

        private ViewModelHistorico _viewModelHistorico;
        private App _app { get; set; }
        private Button _refreshButton { get; set; }
        private PainelDto _painelDto { get; set; }

		public Button _mapa { get; set; }
		public Button _mapaType { get; set; }

        private MessageService _messageService { get; set; }

		private CustomDialogAlert _painelTopLoad { get; set; }

        public ViewHistorico(
			PainelDto Painel
			, object obj
		)
        {
            InitializeComponent();

            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

            _painelDto = Painel;

			exibeBuscarMais = false;

            ListHistorico.BackgroundColor = Color.White;

            ListHistorico.ItemTemplate = new DataTemplate(() =>
			{
				return new ListDetalhes_ViewCell(
					_painelDto
					, this.BindingContext
					, EnumPage.Historico
				);
			});

			_viewModelHistorico = new ViewModelHistorico(
				_painelDto
				, obj
				, EnumPage.Historico
			);
            _viewModelHistorico._viewHistorico = this as IViewHistorico;

			this.BindingContext = _viewModelHistorico;
			MontaLoad();

			painelBuscarMais.Text = AppResources.SearchMore;
        }

		protected override void OnAppearing()
		{

			if(mapaHistorico == null)
			{
				mapaHistorico = new Map();
				painelMapaBox.Children.Add(mapaHistorico);
			}

			var lifecycleHandler = (ViewModelHistorico)this.BindingContext;
			lifecycleHandler.OnAppearing();
        }

		protected override void OnDisappearing()
		{
			var lifecycleHandler = (ViewModelHistorico)this.BindingContext;
			lifecycleHandler.OnDisappearing();
		}
        
		private void MontaLoad()
		{
			_painelTopLoad = new CustomDialogAlert(
				painelMapa
				, Color.White.MultiplyAlpha(0.8)
				, false
			);
			ActivityIndicator activity = _painelTopLoad.RequireActivityIndicator();
			activity.Color = Color.Gray;
			_painelTopLoad.ShowAlert(activity);
            
        }

		public void ExibirLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelTopLoad.ShowAlert();
            });
		}

		public void EscondeLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelTopLoad.HideAlert();
            });
		}
    }
}