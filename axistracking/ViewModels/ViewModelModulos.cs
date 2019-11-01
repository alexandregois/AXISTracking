using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelModulos : ViewModelBase
    {

        private CancellationTokenSource _tokensource { get; set; }
        private TimeSpan _refreshTime { get; set; }
        private ViewModelSincronismo _viewSicronismo { get; set; }

        public ViewModelModulos()
        {

            DefaultTemplateBuild();

            _viewSicronismo = new ViewModelSincronismo();

            _refreshTime = App.Configuracao.RefreshTime;
            _tokensource = new CancellationTokenSource();

            PainelFundoHeightRequest = _app.ScreenHeight;  //_app.DefaultTemplateHeightNavegationBar);
            PainelFundoWidthRequest = _app.ScreenWidth;

            this.ListPainel_RefreshCommand = new Command(this.Refresh);

        }

        public ICommand ListPainel_RefreshCommand
        {
            get;
            set;
        }

        Boolean _listPainel_IsRefreshing;
        public Boolean ListPainel_IsRefreshing
        {
            get
            {
                return _listPainel_IsRefreshing;
            }
            set
            {
                _listPainel_IsRefreshing = value;
                RefreshButtonIsEnabled = !value;
                this.Notify("ListPainel_IsRefreshing");
            }
        }

        Boolean _rastreamentoButtonIsEnabled;
        public Boolean RastreamentoButtonIsEnabled
        {
            get
            {
                return _rastreamentoButtonIsEnabled;
            }
            set
            {
                _rastreamentoButtonIsEnabled = value;
                this.Notify("RastreamentoButtonIsEnabled");
            }
        }

        Boolean _alertaButtonIsEnabled;
        public Boolean AlertaButtonIsEnabled
        {
            get
            {
                return _alertaButtonIsEnabled;
            }
            set
            {
                _alertaButtonIsEnabled = value;
                this.Notify("AlertaButtonIsEnabled");
            }
        }

        Boolean _graficoButtonIsEnabled;
        public Boolean GraficoButtonIsEnabled
        {
            get
            {
                return _graficoButtonIsEnabled;
            }
            set
            {
                _graficoButtonIsEnabled = value;
                this.Notify("GraficoButtonIsEnabled");
            }
        }

        Boolean _comandoButtonIsEnabled;
        public Boolean ComandoButtonIsEnabled
        {
            get
            {
                return _comandoButtonIsEnabled;
            }
            set
            {
                _comandoButtonIsEnabled = value;
                this.Notify("ComandoButtonIsEnabled");
            }
        }

        //gridModulos.Children.Add(stackLayoutRastreamento, 0, 1);
        //gridModulos.Children.Add(stackLayoutAlerta, 2, 1);
        //gridModulos.Children.Add(stackLayoutGrafico, 0, 3);
        //.Children.Add(stackLayoutComando, 2, 3);

        Boolean _refreshButtonIsEnabled;
        public Boolean RefreshButtonIsEnabled
        {
            get
            {
                return _refreshButtonIsEnabled;
            }
            set
            {
                _refreshButtonIsEnabled = value;
                this.Notify("RefreshButtonIsEnabled");
            }
        }

        Double _painelFundoHeightRequest = 0;
        public Double PainelFundoHeightRequest
        {
            get
            {
                return _painelFundoHeightRequest;
            }
            set
            {
                _painelFundoHeightRequest = value;
                this.Notify("PainelFundoHeightRequest");
            }
        }

        Double _painelFundoWidthRequest = 0;
        public Double PainelFundoWidthRequest
        {
            get
            {
                return _painelFundoWidthRequest;
            }
            set
            {
                _painelFundoWidthRequest = value;
                this.Notify("PainelFundoWidthRequest");
            }
        }

        public override void OnAppearing()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _messageService.ShowAlertAsync(ex.Message, AppResources.Erro);
            }

        }

        public override void OnDisappearing()
        {
            if (_tokensource != null)
                _tokensource.Cancel(false);
        }

        public override void OnLayoutChanged()
        {
        }

        public override void DefaultTemplateBuild()
        {

            TokenDataStore store = new TokenDataStore();
            TokenRealm _token = store.Get(1);

            if (_token == null)
            {
                _messageService.ShowAlertAsync(
                    AppResources.LoginNaoEncontrado
                    , AppResources.Erro
                );
                _navigationService.NavigateToLogin();
            }
            else
            {
                Button foto = VoltarButtonDefault();
                foto.Image = "ic_placeholder_foto.png";
                foto.WidthRequest = 50;
                foto.Command = new Command(ImageLogout);
                BoxLeftContent = foto;

                StackLayout stackTopo = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = 0,
                    Spacing = 0
                };

                Label labelTitulo = PanelTituloLabel_Titulo();
                labelTitulo.Text = _token.NomeUsuario;

                Label labelSubTitulo = PanelTituloLabel_SubTitulo();
                labelSubTitulo.Text = _token.NomeCliente;

                labelTitulo.HeightRequest = labelSubTitulo.HeightRequest;
                labelTitulo.VerticalTextAlignment = TextAlignment.End;

                stackTopo.Children.Add(labelTitulo);
                stackTopo.Children.Add(labelSubTitulo);

                BoxMiddleContent = stackTopo;


                Button refreshButton = new Button()
                {
                    Image = "ic_refresh.png",
                    HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                    WidthRequest = 18,
                    Margin = new Thickness(5, 0),
                    BorderRadius = 0,
                    BorderWidth = 0,
                    BorderColor = Color.Transparent,
                    BackgroundColor = Color.Transparent,
                    Command = new Command(Refresh)
                };

                refreshButton.SetBinding(
                    Button.IsEnabledProperty
                    , new TemplateBinding("Parent.BindingContext.RefreshButtonIsEnabled")
                );

                BoxRightContent = refreshButton;

            }

        }

        private void Refresh(object obj)
        {
            if (!this.ListPainel_IsRefreshing)
            {
                _tokensource.Cancel();
                _tokensource = new CancellationTokenSource();
                this.ListPainel_IsRefreshing = true;
            }
        }

        private async Task Loop(TimeSpan paramTimeSpan)
        {

            try
            {
                await Task.Delay(paramTimeSpan, _tokensource.Token);

                if (!_tokensource.IsCancellationRequested)
                    Refresh(null);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

        }


        public async void ImageLogout(object sender)
        {
            await Deslogar();
        }

        public async Task Deslogar()
        {
            Boolean answer;

            answer = await
                this._messageService.ShowAlertChooseAsync(
                    AppResources.ConfirmaSair
                    , AppResources.cancelar
                    , AppResources.Ok
                    , null);

            if (answer == true)
            {
                await Task.Run(async () =>
                {
                    TokenDataStore store = new TokenDataStore();
                    store.Clean();
                });

                this._navigationService.NavigateToLogin();

            }
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
