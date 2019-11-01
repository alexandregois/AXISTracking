using System;
using System.Linq;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.Views.Interface;
using axistracking.Views.Template;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Views
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewDetalhes : ContentPage, IViewDetalhes
    {

        private App _app { get; set; }
        private ListPagePadrao _pageContent { get; set; }

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

        private StackLayout _painelMapaBox;
        public StackLayout PainelMapaBox
        {
            get
            {
                if (_painelMapaBox == null)
                {
                    _painelMapaBox = new StackLayout()
                    {
                        BackgroundColor = Color.Gray,
                        Margin = 0,
                        Padding = 0,
                        Spacing = 0
                    };

                    _painelMapaBox.SetBinding(
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

                    _painelMapaBox.SetBinding(
                        VisualElement.HeightRequestProperty
                        , new Binding(
                            "DefaultHeightContent"
                            , BindingMode.Default
                            , null
                            , null
                            , null
                            , this.BindingContext
                        )
                    );

                    _painelMapaBox.SetBinding(
                        VisualElement.IsVisibleProperty
                        , new Binding(
                            "PainelMapaBoxIsVisible"
                            , BindingMode.Default
                            , null
                            , null
                            , null
                            , this.BindingContext
                        )
                    );

                    _pageContent.PageContent.Children.Add(_painelMapaBox);

                }
                return _painelMapaBox;
            }

        }

        private Map _mapaDetalhes;
        public Map MapaDetalhes
        {
            get
            {
                if (_mapaDetalhes == null)
                {
                    PainelMapaBox.Children.Clear();
                    _mapaDetalhes = new Map();
                    PainelMapaBox.Children.Add(_mapaDetalhes);
                }
                return _mapaDetalhes;
            }
        }

        private ViewModelDetalhes _viewModelDetalhes { get; set; }
        private PainelDto _painelDto { get; set; }

        public EnumPage _actualPage { get; set; }

        public ViewDetalhes(object obj, EnumPage Page)
        {
            InitializeComponent();

            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

            _viewModelDetalhes = new ViewModelDetalhes(
                obj
                , _actualPage
            );
            _viewModelDetalhes._viewDetalhes = this as IViewDetalhes;

            _pageContent = new ListPagePadrao(_viewModelDetalhes);

            PanelGeral.Content = _pageContent;

            this.BindingContext = _viewModelDetalhes;

            _painelDto = (PainelDto)obj;

            _actualPage = Page;

            _pageContent.ListComandos.ItemTemplate = new DataTemplate(() =>
            {
                return new ListDetalhes_ViewCell(_painelDto, _viewModelDetalhes, _actualPage);
            });

            ExibeTitulo(null);

            PainelTopLoad.ShowAlert();

            if (_actualPage == EnumPage.DetalhesAlerta)
            {
                _pageContent.ListViewPanelTop.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            var lifecycleHandler = (ViewModelDetalhes)this.BindingContext;
            lifecycleHandler.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelDetalhes)this.BindingContext;
            lifecycleHandler.OnDisappearing();
        }


        public void ExibirLoad()
        {
            Device.BeginInvokeOnMainThread(_painelTopLoad.ShowAlert);
        }

        public void MudarTamanhoLoad()
        {
            AbsoluteLayout.SetLayoutBounds(
                _painelTopLoad._shadowBox
                , new Rectangle(
                    0
                    , 0
                    , _viewModelDetalhes.DefaultWidth
                    , (
                        _viewModelDetalhes.ListPainelTop_Height
                        + _viewModelDetalhes.PainelFiltroTextHeight
                    )
                )
            );
        }

        public void EscondePercentual()
        {

            if (_actualPage == EnumPage.DetalhesAlerta)
            {
                _pageContent.ListComandos.Margin = new Thickness(0, -60, 0, 0);
            }

        }

        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(_painelTopLoad.HideAlert);
        }

        public void ExibeTitulo(Int32? paramTotal)
        {
            String strTexto = "";
            if (paramTotal.HasValue)
            {
                strTexto = paramTotal.ToString() + " ";

                if (_actualPage == EnumPage.DetalhesUnidade)
                {
                    strTexto += AppResources.Rastreadores;

                }
                else if (_actualPage == EnumPage.DetalhesAlerta)
                {
                    strTexto += AppResources.Alertas;
                }
            }
            else
            {
                if (_actualPage == EnumPage.DetalhesUnidade)
                {
                    strTexto += AppResources.Rastreadores;
                }
                else if (_actualPage == EnumPage.DetalhesAlerta)
                {
                    strTexto += AppResources.Alerta;
                }
            }

            _viewModelDetalhes.TxtTitulo = strTexto;


        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}