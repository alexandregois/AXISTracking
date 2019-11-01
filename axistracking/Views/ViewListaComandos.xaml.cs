using System;
using axistracking.CustomClass;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using axistracking.Views.Interface;
using axistracking.Views.Template;
using Xamarin.Forms;

namespace axistracking.Views
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewListaComandos : ContentPage, IViewListaComandos
    {
        private readonly IMessageService _messageService;


		private ListPagePadrao _pageContent { get; set; }
		private App _app => (Application.Current as App);

		private CustomDialogAlert _painelTopLoad;
		public CustomDialogAlert PainelTopLoad
        {
            get
            {
				if (_painelTopLoad == null)
                {
					_painelTopLoad = new CustomDialogAlert(
						_pageContent.PainelFiltro
						, Color.White.MultiplyAlpha(0.8)
						, false
					);
					ActivityIndicator activity = _painelTopLoad.RequireActivityIndicator();
					activity.Color = Color.Gray;
					_painelTopLoad.AddChildren(activity);

					MudarTamanhoLoad();
                }
				return _painelTopLoad;
            }

        }

        private CustomDialogAlert _painelDetalhes;
        public CustomDialogAlert PainelDetalhes
        {
            get
            {
                if (_painelDetalhes == null)
                {
                    _painelDetalhes = new CustomDialogAlert(
						_pageContent
						, Color.White.MultiplyAlpha(0.8)
                      , false
                  );
                }
                return _painelDetalhes;
            }
        }

        private ViewModelListaComandos _viewModelListaComandos { get; set; }
        private PainelDto _painelDto { get; set; }

        public EnumPage _actualPage { get; set; }

		public ViewListaComandos(
			object obj
			, EnumPage Page
			, string paramNomeUnidade
		)
        {
            InitializeComponent();

            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

			_viewModelListaComandos = new ViewModelListaComandos(
				obj
				, _actualPage
				, paramNomeUnidade
			);
			_viewModelListaComandos._viewComandos = this as IViewListaComandos;
			_pageContent = new ListPagePadrao(_viewModelListaComandos);
			PanelGeral.Content = _pageContent;
			this.BindingContext = _viewModelListaComandos;

            _painelDto = (PainelDto)obj;

            _actualPage = Page;

			_pageContent.ListComandos.ItemTemplate = new DataTemplate(() =>
            {
                return new ListComandos_ViewCell(_actualPage);
            });

			PainelTopLoad.ShowAlert();

			_pageContent.ListComandos.ItemTapped += ListComandos_ItemTapped;

            this._messageService =
                DependencyService.Get<IMessageService>();
        }

        protected override void OnAppearing()
        {

            if (_painelTopLoad == null)
            {
				PainelTopLoad.ShowAlert();
            }

            var lifecycleHandler = (ViewModelListaComandos)this.BindingContext;
            lifecycleHandler.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelListaComandos)this.BindingContext;
            lifecycleHandler.OnDisappearing();
        }

        public void ExibirLoad()
		{
			Device.BeginInvokeOnMainThread(PainelTopLoad.ShowAlert);
        }

        public void MudarTamanhoLoad()
        {
            AbsoluteLayout.SetLayoutBounds(
				PainelTopLoad._shadowBox
                , new Rectangle(
                    0
                    , 0
					, _app.ScreenWidth
                    , (
						_viewModelListaComandos.ListPainelTop_Height
						+ _viewModelListaComandos.PainelFiltroTextHeight
					)
                )
            );
        }

        public void EscondeLoad()
        {
			Device.BeginInvokeOnMainThread(PainelTopLoad.HideAlert);
        }

        private async void ListComandos_ItemTapped(
			object sender
			, ItemTappedEventArgs e
		)
        {

            ComandoLog paramComando = (ComandoLog)e.Item;

			if(paramComando != null)
			{
				ComandosCellTapp temp = new ComandosCellTapp(
					paramComando
					, PainelDetalhes
				);
			}
		}
    }
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}