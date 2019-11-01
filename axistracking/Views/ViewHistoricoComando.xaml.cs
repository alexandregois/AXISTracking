using axistracking.CustomClass;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.Views.Interface;
using axistracking.Views.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace axistracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewHistoricoComando : ContentPage, IViewListaComandos
    {
        private ViewModelHistoricoComando _viewModelHistoricoComando { get; set; }
        private ListPagePadrao _pageContent { get; set; }
        private App _app => (Application.Current as App);

        public EnumPage _actualPage { get; set; }

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

        public ViewHistoricoComando()
        {
            InitializeComponent();

            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

            _viewModelHistoricoComando = new ViewModelHistoricoComando
            {
                _viewComandos = this as IViewListaComandos
            };
            _pageContent = new ListPagePadrao(_viewModelHistoricoComando);

            PanelGeral.Content = _pageContent;
            this.BindingContext = _viewModelHistoricoComando;

            _pageContent.ListComandos.ItemTemplate = new DataTemplate(() =>
            {
                return new ListComandos_ViewCell(_pageContent);
            });

            _pageContent.ListComandos.IsPullToRefreshEnabled = true;

            PainelTopLoad.ShowAlert();

            _pageContent.ListComandos.ItemTapped += ListComandos_ItemTapped;
            CreateButtonMore();

        }

        protected override void OnAppearing()
        {
            PainelTopLoad.ShowAlert();
            var lifecycleHandler = (ViewModelHistoricoComando)this.BindingContext;
            lifecycleHandler.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelHistoricoComando)this.BindingContext;
            lifecycleHandler.OnDisappearing();
        }

        private void CreateButtonMore()
        {
            Button button = new Button()
            {
                Text = AppResources.SearchMore,
                BackgroundColor = Color.White,
                BorderWidth = 0,
                BorderRadius = 0,
                BorderColor = Color.Transparent
            };

            button.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            button.SetBinding(
                VisualElement.IsEnabledProperty
                , new Binding(
                    "MaisCommandIsEnabled"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            button.SetBinding(
                Button.CommandProperty
                , new Binding(
                    "BuscarMaisCommand"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            _pageContent.PageContent.Children.Add(button);
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
                        _viewModelHistoricoComando.ListPainelTop_Height
                        + _viewModelHistoricoComando.PainelFiltroTextHeight
                    )
                )
            );
        }

        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(PainelTopLoad.HideAlert);
        }

        private void ListComandos_ItemTapped(
            object sender
            , ItemTappedEventArgs e
        )
        {
            ComandoLog paramComando = (ComandoLog)e.Item;

            if (paramComando != null)
            {
                ComandosCellTapp temp = new ComandosCellTapp(
                    paramComando
                    , PainelDetalhes
                );
            }
        }
    }
}