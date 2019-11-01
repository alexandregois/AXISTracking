using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Bll;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.Services;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;


namespace axistracking.ViewModels
{
    public class ViewModelPosicao : ViewModelBase
    {
        private Int64? _idPosicao { get; set; }
        private Boolean _isMapaBig = false;

        private PainelDto _painelDto { get; set; }

        private CancellationTokenSource _tokensource { get; set; }
        public IViewPosicao _view { get; set; }
        private EnumPage _actualPage { get; set; }
        private EnumPage _returnPage { get; set; }

        private Bll_Mapa _bllMapa { get; set; }

        public PosicaoHistorico _posicaoHistorico { get; set; }

        public PosicaoHistorico _posicaoDetalhes { get; set; }

        protected App _app => (Application.Current as App);

        public ICommand MapaCommand
        {
            get;
            set;
        }

        public ICommand MapaTypeCommand
        {
            get;
            set;
        }

        Thickness _marginDefault = new Thickness(7, 0, 7, 1);
        public Thickness MarginDefault
        {
            get
            {
                return _marginDefault;
            }
            set
            {
                _marginDefault = value;
                this.Notify("MarginDefault");
            }
        }

        Thickness _painelStackLayoutMargin = new Thickness(0, 10);
        public Thickness PainelStackLayoutMargin
        {
            get
            {
                return _painelStackLayoutMargin;
            }
            set
            {
                _painelStackLayoutMargin = value;
                this.Notify("PainelStackLayoutMargin");
            }
        }

        Thickness _painelStackLayoutMarginInformacoes = new Thickness(0, 5);
        public Thickness PainelStackLayoutMarginInformacoes
        {
            get
            {
                return _painelStackLayoutMarginInformacoes;
            }
            set
            {
                _painelStackLayoutMarginInformacoes = value;
                this.Notify("PainelStackLayoutMarginInformacoes");
            }
        }

        Boolean _exibeStreetView = false;
        public Boolean ExibeStreetView
        {
            get
            {
                return _exibeStreetView;
            }
            set
            {
                _exibeStreetView = value;
                this.Notify("ExibeStreetView");
            }
        }

        String _contentStreetView = null;
        String _wVStreetSource = null;
        public String WVStreetSource
        {
            get
            {
                return _wVStreetSource;
            }
            set
            {
                _wVStreetSource = value;
                this.Notify("WVStreetSource");
            }
        }

        Double _painelMapaHeightRequest = 0;
        public Double PainelMapaHeightRequest
        {
            get
            {
                return _painelMapaHeightRequest;
            }
            set
            {
                _painelMapaHeightRequest = value;
                this.Notify("PainelMapaHeightRequest");
            }
        }

        Double _scrollViewHeightRequest;
        public Double ScrollViewHeightRequest
        {
            get
            {
                return _scrollViewHeightRequest;
            }
            set
            {
                _scrollViewHeightRequest = DefaultHeightContent
                                   - value;

                this.Notify("ScrollViewHeightRequest");
            }
        }

        Dictionary<string, bool> _painelSensoresListViewSource;
        public Dictionary<string, bool> PainelSensoresListViewSource
        {
            get
            {
                return _painelSensoresListViewSource;
            }
            set
            {
                _painelSensoresListViewSource = value;
                this.Notify("PainelSensoresListViewSource");
            }
        }

        Dictionary<string, bool> _painelAtuadoresListViewSource;
        public Dictionary<string, bool> PainelAtuadoresListViewSource
        {
            get
            {
                return _painelAtuadoresListViewSource;
            }
            set
            {
                _painelAtuadoresListViewSource = value;
                this.Notify("PainelAtuadoresListViewSource");
            }
        }

        Dictionary<string, double> _painelTelemetriaListViewSource;
        public Dictionary<string, double> PainelTelemetriaListViewSource
        {
            get
            {
                return _painelTelemetriaListViewSource;
            }
            set
            {
                _painelTelemetriaListViewSource = value;
                this.Notify("PainelTelemetriaListViewSource");
            }
        }

        Dictionary<string, bool> _painelInformacoesListViewSource;
        public Dictionary<string, bool> PainelInformacoesListViewSource
        {
            get
            {
                return _painelInformacoesListViewSource;
            }
            set
            {
                _painelInformacoesListViewSource = value;
                this.Notify("PainelInformacoesListViewSource");
            }
        }

        Double _painelDetalhesHeightRequest = 0; //;
        public Double PainelDetalhesHeightRequest
        {
            get
            {
                return _painelDetalhesHeightRequest;
            }
            set
            {
                _painelDetalhesHeightRequest = (value + PainelMapaHeightRequest) - MarginDefault.Bottom;
                ScrollViewHeightRequest = _painelDetalhesHeightRequest;
                this.Notify("PainelDetalhesHeightRequest");
            }
        }

        Posicao _painelResumoListViewSource;
        public Posicao PainelResumoListViewSource
        {
            get
            {
                return _painelResumoListViewSource;
            }
            set
            {
                _painelResumoListViewSource = value;
                this.Notify("PainelResumoListViewSource");
            }
        }

        Boolean _painelDetalhes_IsRefreshing;
        public Boolean PainelDetalhes_IsRefreshing
        {
            get
            {
                return _painelDetalhes_IsRefreshing;
            }
            set
            {
                _painelDetalhes_IsRefreshing = value;
                this.Notify("PainelDetalhes_IsRefreshing");
            }
        }

        #region Label
        private Thickness _labelMargin = new Thickness(10, 0, 10, 10);
        public Thickness LabelMargin
        {
            get
            {
                return _labelMargin;
            }
            set
            {
                _labelMargin = value;
                this.Notify("LabelMargin");
            }
        }

        private Double _labelFontSize = 19;
        public Double LabelFontSize
        {
            get
            {
                return _labelFontSize;
            }
            set
            {
                _labelFontSize = value;
                this.Notify("LabelFontSize");
            }
        }

        private Double _labelFontSizeSensores = 17;
        public Double LabelFontSizeSensores
        {
            get
            {
                return _labelFontSizeSensores;
            }
            set
            {
                _labelFontSizeSensores = value;
                this.Notify("LabelFontSizeSensores");
            }
        }

        private FontAttributes _labelFontAttributes = FontAttributes.Bold;
        public FontAttributes LabelFontAttributes
        {
            get
            {
                return _labelFontAttributes;
            }
            set
            {
                _labelFontAttributes = value;
                this.Notify("LabelFontAttributes");
            }
        }
        #endregion


        String _txtTitulo;
        public String TxtTitulo
        {
            get
            {
                return _txtTitulo;
            }
            set
            {
                _txtTitulo = value;
                this.Notify("TxtTitulo");
            }
        }

        String _txtSubTitulo;
        public String TxtSubTitulo
        {
            get
            {
                return _txtSubTitulo;
            }
            set
            {
                _txtSubTitulo = value;
                this.Notify("TxtSubTitulo");
            }
        }

        Boolean _btnTopIsEnabled;
        public Boolean BtnTopIsEnabled
        {
            get
            {
                return _btnTopIsEnabled;
            }
            set
            {
                _btnTopIsEnabled = value;
                this.Notify("BtnTopIsEnabled");
            }
        }

        Color _mapaButtonBackgroundColor;
        public Color MapaButtonBackgroundColor
        {
            get
            {
                return _mapaButtonBackgroundColor;
            }
            set
            {
                _mapaButtonBackgroundColor = value;
                this.Notify("MapaButtonBackgroundColor");
            }
        }

        public ViewModelPosicao(
            PainelDto Painel
            , object obj
            , EnumPage Page
            , EnumPage paramLastPage
        )
        {
            _posicaoDetalhes = (PosicaoHistorico)obj;

            _actualPage = Page;
            _painelDto = Painel;
            _returnPage = paramLastPage;

            this.MapaCommand = new Command(this.MapaAction);
            this.MapaTypeCommand = new Command(this.MapaTypeAction);

            BtnTopIsEnabled = false;

            _tokensource = new CancellationTokenSource();

            CalculaAlturaMapa();

            MapaButtonBackgroundColor = _desactiveBackgroundColor;

        }

        public override void OnAppearing()
        {
            try
            {
                _view.ExibirLoad();

                if (_bllMapa == null)
                    _bllMapa = new Bll_Mapa(this._view.mapaPosicao);

                PainelResumoListViewSource = null;
                PainelSensoresListViewSource = null;
                PainelAtuadoresListViewSource = null;
                PainelTelemetriaListViewSource = null;
                PainelInformacoesListViewSource = null;

                BuscarPosicao();

                Device.BeginInvokeOnMainThread(() =>
                {
                    PainelDetalhesHeightRequest = 150;
                });

            }
            catch (Exception ex)
            {
                this._messageService.ShowAlertAsync(ex.Message);
            }
        }

        public override void OnDisappearing()
        {
            if (_tokensource != null)
                _tokensource.Cancel();
        }

        public override void OnLayoutChanged()
        {
        }

        public override void DefaultTemplateBuild()
        {
            try
            {
                BoxLeftContent = VoltarButtonDefault();

                StackLayout stackTop = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = 0,
                    Spacing = 0,
                    Padding = 0
                };

                Label titulo = PanelTituloLabel_Titulo();
                titulo.VerticalTextAlignment = TextAlignment.End;

                Label subTitulo = PanelTituloLabel_SubTitulo();

                titulo.HeightRequest = subTitulo.HeightRequest;

                titulo.SetBinding(
                    Label.TextProperty
                    , new TemplateBinding("Parent.BindingContext.TxtTitulo")
                );

                subTitulo.SetBinding(
                    Label.TextProperty
                    , new TemplateBinding("Parent.BindingContext.TxtSubTitulo")
                );

                stackTop.Children.Add(titulo);
                stackTop.Children.Add(subTitulo);

                BoxMiddleContent = stackTop;

                StackLayout stackRight = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = 0,
                    Spacing = 0
                };

                Button mapaViewDetalhes = new Button()
                {
                    Image = "ic_mapa.png",
                    HeightRequest = stackRight.HeightRequest,
                    WidthRequest = 29,
                    Margin = 0,
                    BorderRadius = 0,
                    BorderWidth = 0,
                    BorderColor = Color.Transparent,
                    IsEnabled = false
                };

                mapaViewDetalhes.SetBinding(
                    Button.CommandProperty
                    , new TemplateBinding("Parent.BindingContext.MapaCommand")
                );

                mapaViewDetalhes.SetBinding(
                    VisualElement.IsEnabledProperty
                    , new TemplateBinding("Parent.BindingContext.BtnTopIsEnabled")
                );

                mapaViewDetalhes.SetBinding(
                    VisualElement.BackgroundColorProperty
                    , new TemplateBinding("Parent.BindingContext.MapaButtonBackgroundColor")
                );

                stackRight.Children.Add(mapaViewDetalhes);


                Button mapaTypeViewDetalhes = new Button()
                {
                    Image = "ic_menu.png",
                    HeightRequest = stackRight.HeightRequest,
                    WidthRequest = 30,
                    Margin = new Thickness(5, 0, 0, 0),
                    BorderRadius = 0,
                    BorderWidth = 0,
                    BorderColor = Color.Transparent,
                    BackgroundColor = Color.Transparent,
                    IsVisible = true
                };

                mapaTypeViewDetalhes.SetBinding(
                    Button.CommandProperty
                    , new TemplateBinding("Parent.BindingContext.MapaTypeCommand")
                );

                mapaTypeViewDetalhes.SetBinding(
                    Button.IsEnabledProperty
                    , new TemplateBinding("Parent.BindingContext.BtnTopIsEnabled")
                );

                stackRight.Children.Add(mapaTypeViewDetalhes);

                BoxRightContent = stackRight;
            }
            catch
            {
            }
        }

        private void MontaStreetView(Double paramLatitude, Double paramLongitude)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {

                    String mapsKey = "AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws";

                    String url = String.Empty;

                    if (_app.ScreenHeight < 550)
                    {
                        url =
                            "https://maps.googleapis.com/maps/api/streetview?size="
                            + 400 + "x"
                                          + 600 + "&location="
                                          + paramLatitude.ToString() + ","
                                          + paramLongitude.ToString()
                                          + "&key=" + mapsKey;


                        //"https://maps.googleapis.com/maps/api/streetview?size="
                        //    + DefaultWidth.ToString() + "x"
                        //                  + DefaultHeightContent.ToString() + "&location="
                        //                  + paramLatitude.ToString() + ","
                        //                  + paramLongitude.ToString()
                        //                  + "&key=" + mapsKey;


                    }
                    else
                    {
                        url = "http://maps.google.com/maps?q=&layer=c&cbll=" + paramLatitude.ToString().Replace(",", ".") + ","
                                          + paramLongitude.ToString().Replace(",", ".") + "&cbp=11,0,0,0,0&key=" + mapsKey;
                    }


                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        url =
                            "https://maps.googleapis.com/maps/api/streetview?size="
                            + 400 + "x"
                            + 600 + "&location="
                            + paramLatitude.ToString() + ","
                                                         + paramLongitude.ToString()
                                                         + "&key=" + mapsKey;
                    }

                    _contentStreetView = url;

                    if (_wVStreetSource != _contentStreetView)
                    {
                        WVStreetSource = _contentStreetView;
                        _wVStreetSource = _contentStreetView;
                    }


                    //var getStreetView = DependencyService.Get<IStreetViewService>();
                    //getStreetView.openStreetView(paramLatitude, paramLongitude);

                }
                catch
                {
                    ShowErrorAlert("Exception");
                }

            });

        }

        private void MontaMapa(PosicaoHistorico paramPosicao)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Position paramPosition = new Position(
                            Convert.ToDouble(paramPosicao.Latitude)
                            , Convert.ToDouble(paramPosicao.Longitude)
                        );

                        _bllMapa.MontaMapaPosicao(paramPosicao);
                        this._view.mapaPosicao.MoveToRegion(
                            MapSpan.FromCenterAndRadius(
                                paramPosition
                                , Distance.FromMeters(300)
                            )
                            , true
                        );

                        this._view.MontaMapa();

                    }
                    catch
                    {
                        ShowErrorAlert("Exception");
                    }

                });
            }
            catch
            {
                ShowErrorAlert("Exception");
            }
        }

        public void CalculaAlturaMapa()
        {
            PainelMapaHeightRequest = 40 * (DefaultHeightContent / 100);
        }

        public void AtivarAncora(PosicaoHistorico posicaoHistorico)
        {
            ModelPosicao _modelPosicao = new ModelPosicao();

            _tokensource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                ServiceResult<AncoraAtivacaoDto> result = new ServiceResult<AncoraAtivacaoDto>();
                _view.ExibirLoad();
                BtnTopIsEnabled = false;
                try
                {
                    result = await _modelPosicao.AtivarAncora(
                        posicaoHistorico
                        , _tokensource.Token
                    );

                }
                catch
                {
                    result.MessageError = "Exception";
                }
                finally
                {
                    AtivarAncora_Finish(
                        result
                        , posicaoHistorico
                    );
                }
            }, _tokensource.Token);
        }

        private void AtivarAncora_Finish(
            ServiceResult<AncoraAtivacaoDto> paramResult
            , PosicaoHistorico paramPosicaoHistorico
        )
        {
            try
            {
                if (!_tokensource.IsCancellationRequested)
                {
                    UpdateToken(paramResult.RefreshToken);
                    if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                    {

                        paramPosicaoHistorico.Ancora_Tolerancia = paramResult.Data.ToleranciaAncoraPadrao;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _bllMapa.MontaMapaAncora(
                                paramPosicaoHistorico
                                , paramResult.Data.ToleranciaAncoraPadrao
                            );
                            _view.MontaDetalheTopoPosicao(
                                paramPosicaoHistorico
                            );
                            this._messageService.ShowAlertAsync(AppResources.AncoraCriada);
                        });
                    }
                    else
                    {
                        ShowErrorAlert(paramResult.MessageError);
                    }
                }
            }
            catch
            {
                ShowErrorAlert("Exception");
            }
            finally
            {
                _view.EscondeLoad();
                BtnTopIsEnabled = true;
            }
        }

        public void DesativarAncora(PosicaoHistorico posicaoHistorico)
        {
            ModelPosicao _modelPosicao = new ModelPosicao();

            _tokensource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                ServiceResult<Int32> result = new ServiceResult<Int32>();
                _view.ExibirLoad();
                BtnTopIsEnabled = false;
                try
                {
                    result = await _modelPosicao.DesativarAncora(
                        posicaoHistorico
                        , _tokensource.Token
                    );

                }
                catch
                {
                    result.MessageError = "Exception";
                }
                finally
                {
                    DesativarAncora_Finish(
                        result
                        , posicaoHistorico
                    );
                }
            }, _tokensource.Token);

        }

        private void DesativarAncora_Finish(
            ServiceResult<Int32> paramResult
            , PosicaoHistorico paramPosicaoHistorico
        )
        {
            try
            {
                if (!_tokensource.IsCancellationRequested)
                {
                    UpdateToken(paramResult.RefreshToken);
                    if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                    {


                        paramPosicaoHistorico.Ancora_Tolerancia = null;


                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                _bllMapa.RemoverCircle();

                                _view.MontaDetalheTopoPosicao(paramPosicaoHistorico);
                                this._messageService.ShowAlertAsync(AppResources.AncoraRemovida);
                            }
                            catch
                            {
                                ShowErrorAlert("Exception");
                            }
                        });
                    }
                    else
                    {
                        ShowErrorAlert(paramResult.MessageError);
                    }
                }
            }
            catch
            {
                ShowErrorAlert("Exception");
            }
            finally
            {
                _view.EscondeLoad();
                BtnTopIsEnabled = true;
            }
        }


        #region Mapa
        public async void MapaAction()
        {
            ExibeStreetView = false;
            if (_isMapaBig)
            {
                _isMapaBig = false;
                MapaButtonBackgroundColor = _desactiveBackgroundColor;
                CalculaAlturaMapa();
            }
            else
            {
                _isMapaBig = true;
                PainelMapaHeightRequest = DefaultHeightContent;
                MapaButtonBackgroundColor = _activeBackgroundColor;
            }

        }

        public async void MapaTypeAction()
        {
            String answer;
            answer = await _messageService.ShowMessageAsync(
                AppResources.MapType
                , AppResources.cancelar
                , null
                , new string[]{
                AppResources.Map
                , AppResources.Satellite
                , AppResources.Hybrid
                , AppResources.Terrain
                , AppResources.Trafego
                , AppResources.StreetView
            }
            );

            if (answer != AppResources.cancelar)
            {

                if (answer == AppResources.Hybrid)
                {
                    this._view.MontaMapa();
                    this._view.mapaPosicao.MapType = MapType.Hybrid;
                    this._view.mapaPosicao.IsTrafficEnabled = false;

                }
                else if (answer == AppResources.Satellite)
                {
                    this._view.MontaMapa();
                    this._view.mapaPosicao.MapType = MapType.Satellite;
                    this._view.mapaPosicao.IsTrafficEnabled = false;

                }
                else if (answer == AppResources.Terrain)
                {
                    this._view.MontaMapa();
                    this._view.mapaPosicao.MapType = MapType.Terrain;
                    this._view.mapaPosicao.IsTrafficEnabled = false;

                }
                else if (answer == AppResources.Trafego)
                {
                    this._view.MontaMapa();
                    this._view.mapaPosicao.MapType = MapType.Street;
                    this._view.mapaPosicao.IsTrafficEnabled = true;

                }
                else if (answer == AppResources.StreetView)
                {
                    _view.MontaStreetView(_posicaoDetalhes.Latitude.Value, _posicaoDetalhes.Longitude.Value);
                }
                else
                {
                    this._view.MontaMapa();
                    this._view.mapaPosicao.MapType = MapType.Street;
                    this._view.mapaPosicao.IsTrafficEnabled = false;

                }


                //}
            }

        }

        public WebView StreetView(Double paramLatitude, Double paramLongitude)
        {

            WebView WVStreet = new WebView();

            String mapsKey = "AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws";
            String url = String.Empty;


            try
            {
                if (_app.ScreenHeight < 550)
                {
                    url =
                        "https://maps.googleapis.com/maps/api/streetview?size="
                        + 400 + "x"
                        + 600 + "&location="
                        + paramLatitude.ToString() + ","
                                                     + paramLongitude.ToString()
                                                     + "&key=" + mapsKey;
                }
                else
                {
                    url = "http://maps.google.com/maps?q=&layer=tc&cbll=" + paramLatitude.ToString().Replace(",", ".") + ","
                                                                                         + paramLongitude.ToString().Replace(",", ".") + "&cbp=11,0,0,0,0&key=" + mapsKey;
                }


                if (Device.RuntimePlatform == Device.iOS)
                {
                    url =
                        "https://maps.googleapis.com/maps/api/streetview?size="
                        + 400 + "x"
                        + 600 + "&location="
                        + paramLatitude.ToString() + ","
                                       + paramLongitude.ToString()
                                       + "&key=" + mapsKey;
                }


                WVStreet.Source = url;

                Task.Delay(5000);


            }
            catch
            {
                //ShowErrorAlert("Exception");
            }


            return WVStreet;

        }

        private async Task Loop(TimeSpan paramRefreshTime)
        {

            try
            {
                await Task.Delay(paramRefreshTime, _tokensource.Token);

                if (!this.PainelDetalhes_IsRefreshing)
                {
                    BuscarPosicao();
                }

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }

        }

        #endregion

        public async Task<Boolean> ExibeMenuComandos()
        {
            ModelComando _modelComandos = new ModelComando();

            Boolean retorno = false;

            ServiceResult<List<ComandoDto>> result = new ServiceResult<List<ComandoDto>>();
            try
            {
                _tokensource = new CancellationTokenSource();

                try
                {
                    result = await _modelComandos.ListComandoRastreador(
                        PainelResumoListViewSource.IdRastreador
                        , _tokensource.Token
                    );

                    if (result.Data != null && result.Data.Count > 0)
                        retorno = true;

                }
                catch
                {
                    retorno = false;

                }

            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        private void BuscarPosicao()
        {

            try
            {
                _tokensource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    try
                    {

                        //Task.Run(async () =>
                        //{
                        if (String.IsNullOrEmpty(_posicaoDetalhes.Endereco))
                        {
                            String endereco = await FindAddressByPosition(_posicaoDetalhes.Latitude.Value, _posicaoDetalhes.Longitude.Value);
                            _posicaoDetalhes.Endereco = endereco;
                        }
                        //});


                        BtnTopIsEnabled = false;
                        ModelPosicao _modelPosicao = new ModelPosicao();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this._bllMapa.LimpaMapa();
                        });

                        Int64? idPosicao = null;
                        if (_posicaoDetalhes.ExibeUltimaPosicao == false)
                            idPosicao = _posicaoDetalhes.IdPosicao;


                        ServiceResult<Posicao> resultPainel =
                            await _modelPosicao.Get(
                                idPosicao
                                , _posicaoDetalhes.IdUnidadeRastreada
                                , _posicaoDetalhes.OrdemRastreador
                                , _tokensource.Token
                            );

                        if (!_tokensource.IsCancellationRequested)
                        {
                            UpdateToken(resultPainel.RefreshToken);

                            if (String.IsNullOrWhiteSpace(resultPainel.MessageError))
                            {

                                Posicao tempPosicao = resultPainel.Data;

                                if (tempPosicao.Latitude != null)
                                {

                                    tempPosicao.Endereco = _posicaoDetalhes.Endereco;


                                    PainelResumoListViewSource = tempPosicao;





                                    PosicaoHistorico posicao = new PosicaoHistorico()
                                    {
                                        CorRegraPrioritaria = tempPosicao.CorRegraPrioritaria,
                                        DataEvento = tempPosicao.DataEvento.ToLocalTime(),
                                        Identificacao = tempPosicao.Identificacao,
                                        IdTipoUnidadeRastreada = tempPosicao.IdTipoUnidadeRastreada,
                                        //Ignicao = tempPosicao.Ignicao,
                                        Latitude = tempPosicao.Latitude,
                                        Longitude = tempPosicao.Longitude,
                                        NomeRegraViolada = tempPosicao.NomeRegraViolada,
                                        Velocidade = tempPosicao.Velocidade,
                                        ResponsavelUnidadeRastreada = tempPosicao.ResponsavelUnidadeRastreada,
                                        Endereco = _posicaoDetalhes.Endereco

                                    };

                                    if (tempPosicao.Ignicao != null)
                                        posicao.Ignicao = tempPosicao.Ignicao;


                                    //Monta StreetView
                                    MontaStreetView(posicao.Latitude.Value, posicao.Longitude.Value);



                                    posicao.ExibeUltimaPosicao = _posicaoDetalhes.ExibeUltimaPosicao;
                                    posicao.IdUnidadeRastreada = _posicaoDetalhes.IdUnidadeRastreada;

                                    if (tempPosicao.Ancora_Latitude != null)
                                    {
                                        posicao.Ancora_Latitude = tempPosicao.Ancora_Latitude;
                                        posicao.Ancora_Longitude = tempPosicao.Ancora_Longitude;
                                        posicao.Ancora_Tolerancia = tempPosicao.Ancora_Tolerancia;
                                    }

                                    posicao.OrdemRastreador = _posicaoDetalhes.OrdemRastreador;

                                    if (_painelDto.Id == 2 && _posicaoDetalhes.CorRegraPrioritaria != null) //Fixa Cor Alerta
                                    {
                                        posicao.CorRegraPrioritaria = _posicaoDetalhes.CorRegraPrioritaria;
                                        posicao.NomeRegraViolada = _posicaoDetalhes.NomeRegraViolada;
                                    }


                                    _view.MontaDetalheTopoPosicao(posicao);


                                    _posicaoHistorico = posicao;

                                    PainelSensoresListViewSource = tempPosicao.Sensores;
                                    PainelAtuadoresListViewSource = tempPosicao.Atuadores;
                                    PainelTelemetriaListViewSource = tempPosicao.Telemetrias;
                                    PainelInformacoesListViewSource = tempPosicao.Informacoes;


                                    //Monta Mapa da Posicao
                                    MontaMapa(posicao);


                                    if (posicao.Ancora_Tolerancia != null)
                                        _bllMapa.MontaMapaAncora(posicao, posicao.Ancora_Tolerancia.Value);

                                }
                                else
                                {
                                    this._messageService.ShowAlertAsync(AppResources.NaoHaDetalhesPosicao);
                                }
                            }
                            else
                            {
                                ShowErrorAlert(resultPainel.MessageError);
                            }

                            _view.EscondeLoad();
                            this.PainelDetalhes_IsRefreshing = false;
                            BtnTopIsEnabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorAlert("Exception");
                    }
                }, _tokensource.Token);

            }
            catch (Exception ex)
            {
                ShowErrorAlert("Exception");
            }
        }

        public void NavigateToComandos()
        {
            _navigationService.NavigateToComandos(PainelResumoListViewSource);
        }

        public async Task<String> FindAddressByPosition(Double paramLatitude, Double paramLongitude)
        {
            String ret = null;
            try
            {
                Geocoder coder = new Geocoder();

                Position pos = new Position(paramLatitude, paramLongitude);

                IEnumerable<string> lstPosition = await coder.GetAddressesForPositionAsync(pos);

                if (lstPosition != null)
                {
                    ret = lstPosition.First();

                    if (ret.Length > 0)
                        ret = ret.Replace("\n", "-");
                }
                else
                {
                    ret = AppResources.AddressNotFound;
                }
            }
            catch (Exception)
            {
                ret = AppResources.AddressNotFound;
            }

            return ret;
        }


    }

}
