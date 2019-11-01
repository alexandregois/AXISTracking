using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Bll;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelHistorico : ViewModelBase
    {
        private Boolean _isMapaBig = false;

        public IViewHistorico _viewHistorico { get; set; }

        private CancellationTokenSource _tokensource { get; set; }

        private Bll_Mapa _bllMapa { get; set; }

        private PosicaoHistorico posicaoHistorico { get; set; }

        private PainelDto _painelDto { get; set; }

        private EnumPage _actualPage { get; set; }

        private EnumPage _returnPage { get; set; }

        private Int32 periodoBusca { get; set; }

        private List<Position> _lstPosition { get; set; }

        public object SelectedItem { get; set; }

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

        public ICommand ViewHistoricoCommand
        {
            get;
            set;
        }

        public ICommand RefreshCommand
        {
            get;
            set;
        }

        public ICommand BuscarMaisCommand
        {
            get;
            set;
        }

        public ICommand ListHistorico_RefreshCommand
        {
            get;
            set;
        }

        List<PosicaoHistorico> _listHistorico_Source = new List<PosicaoHistorico>();
        public List<PosicaoHistorico> ListHistorico_Source
        {
            get
            {
                return _listHistorico_Source;
            }
            set
            {
                _listHistorico_Source = value;
                //App.ListPainelSource = _listPainel_Source;
                this.Notify("ListHistorico_Source");
            }
        }

        Double _heightPainelMapa = new Double();
        public Double HeightPainelMapa
        {
            get
            {
                return _heightPainelMapa;
            }
            set
            {
                _heightPainelMapa = value;
                this.Notify("HeightPainelMapa");
            }
        }

        Double _heightPainelLista = new Double();
        public Double HeightPainelLista
        {
            get
            {
                return _heightPainelLista;
            }
            set
            {
                _heightPainelLista = value;
                this.Notify("HeightPainelLista");
            }
        }

        Double _heightPainelBuscarMais = new Double();
        public Double HeightPainelBuscarMais
        {
            get
            {
                return _heightPainelBuscarMais;
            }
            set
            {
                _heightPainelBuscarMais = value;
                this.Notify("HeightPainelBuscarMais");
            }
        }

        Boolean _listHistorico_IsRefreshing;
        public Boolean ListHistorico_IsRefreshing
        {
            get
            {
                return _listHistorico_IsRefreshing;
            }
            set
            {
                _listHistorico_IsRefreshing = value;
                this.Notify("ListHistorico_IsRefreshing");
            }
        }

        Boolean _maisCommandIsEnabled = false;
        public Boolean MaisCommandIsEnabled
        {
            get
            {
                return _maisCommandIsEnabled;
            }
            set
            {
                _maisCommandIsEnabled = value;
                this.Notify("MaisCommandIsEnabled");
            }
        }

        public DateTime? DataUltimaPosicao { get; set; }


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

        public ViewModelHistorico(
            PainelDto Painel
            , object obj
            , EnumPage Page
        )
        {
            posicaoHistorico = (PosicaoHistorico)obj;
            ExibeTitulo();
            DefaultTemplateBuild();
            _painelDto = Painel;

            _actualPage = Page;

            _returnPage = EnumPage.Detalhes;

            this.RefreshCommand = new Command(this.Refresh);
            this.ListHistorico_RefreshCommand = new Command(this.ListHistoricoRefresh);
            this.BuscarMaisCommand = new Command(this.BuscarMais);

            this.MapaCommand = new Command(this.MapaAction);
            this.MapaTypeCommand = new Command(this.MapaTypeAction);


            if (Painel.Id == 2) //Tela de Alertas
                periodoBusca = -2;
            else
                periodoBusca = -4;


            MapaTelaNormal();

            HeightPainelBuscarMais = _app.DefaultTemplateHeightNavegationBar;

            HeightPainelLista =
                DefaultHeightContent - (
                    HeightPainelMapa
                    + HeightPainelBuscarMais
                );
            BtnTopIsEnabled = false;
            MapaButtonBackgroundColor = _desactiveBackgroundColor;
        }

        public override void OnAppearing()
        {
            try
            {
                //ExibeTitulo();

                if (_bllMapa == null)
                    _bllMapa = new Bll_Mapa(this._viewHistorico.mapaHistorico);
                if (App.ListPainelTopUnidadesSource != null && App.ListPainelTopUnidadesSource.Count() > 0)
                {
                    try
                    {

                        PainelDto painelTopTemp = App.ListPainelTopUnidadesSource.FirstOrDefault();
                        BtnTopIsEnabled = true;
                        return;

                    }
                    catch (Exception ex)
                    {
                        this._messageService.ShowAlertAsync(
                            ex.Message
                            , "Erro"
                        );
                    }
                }

                this.ListHistoricoRefresh(null);

            }
            catch (Exception ex)
            {
                this._messageService.ShowAlertAsync(
                    ex.Message
                    , "Erro"
                );
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
                Label subTitulo = PanelTituloLabel_SubTitulo();
                titulo.HeightRequest = subTitulo.HeightRequest;

                subTitulo.SetBinding(
                    Label.TextProperty
                    , new TemplateBinding("Parent.BindingContext.TxtSubTitulo")
                );

                titulo.SetBinding(
                    Label.TextProperty
                    , new TemplateBinding("Parent.BindingContext.TxtTitulo")
                );

                if (posicaoHistorico != null)
                {
                    ExibeSubTitulo(posicaoHistorico.DataEvento.Value.ToLocalTime());
                }

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
                    VisualElement.IsEnabledProperty
                    , new TemplateBinding("Parent.BindingContext.BtnTopIsEnabled")
                );

                stackRight.Children.Add(mapaTypeViewDetalhes);

                BoxRightContent = stackRight;
            }
            catch
            {
            }
        }

        private void Refresh(object obj)
        {
            if (!this.ListHistorico_IsRefreshing)
            {
                if (_tokensource != null)
                    _tokensource.Cancel(false);

                _tokensource = new CancellationTokenSource();

                this.ListHistoricoRefresh(obj);
            }
        }

        private void ListHistoricoRefresh(object obj)
        {
            ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();
            try
            {
                _tokensource = new CancellationTokenSource();

                Task.Run(async () =>
                {

                    try
                    {
                        this.ListHistorico_IsRefreshing = true;
                        this._viewHistorico.ExibirLoad();
                        this.MaisCommandIsEnabled = false;
                        this._viewHistorico.mapaHistorico.CameraIdled += MapaHistorico_CameraChanged;
                        BtnTopIsEnabled = false;

                        ModelHistorico _modelHistorico = new ModelHistorico();

                        DateTime? dataPeriodo = null;

                        DateTime dataPeriodoAnterior = _app.DataHistoricoAnterior;

                        try
                        {
                            if (obj != null)
                            {
                                dataPeriodo = (DateTime)obj;

                                if (dataPeriodo.Value == dataPeriodoAnterior)
                                {
                                    dataPeriodo = dataPeriodo.Value.AddHours(-4);
                                }

                                _app.DataHistoricoAnterior = dataPeriodo.Value;

                            }

                        }
                        catch { }

                        result = await _modelHistorico
                            .ListHistoricoUnidadesAsync(
                                posicaoHistorico.IdUnidadeRastreada
                                , dataPeriodo
                                , periodoBusca
                                , posicaoHistorico.OrdemRastreador
                                , _tokensource.Token
                            );

                        if (result.Data.Count == 0)
                        {
                            DateTime dataResult = _app.DataHistoricoAnterior.AddHours(-4);
                            //_app.DataHistoricoAnterior = _app.DataHistoricoAnterior.AddHours(-4);

                            result = await _modelHistorico
                            .ListHistoricoUnidadesAsync(
                                posicaoHistorico.IdUnidadeRastreada
                                , dataResult
                                , periodoBusca
                                , posicaoHistorico.OrdemRastreador
                                , _tokensource.Token
                            );

                            if (result.Data.Count == 0)
                                DataUltimaPosicao = dataResult;

                            _app.DataHistoricoAnterior = dataResult.AddHours(-4);
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this._bllMapa.LimpaMapa();
                        });

                    }
                    catch
                    {
                        result.MessageError = "Exception";
                    }
                    finally
                    {
                        ListHistoricoRefresh_Finish(result);
                    }
                }, _tokensource.Token);

            }
            catch
            {
                result.MessageError = "Exception";
                ListHistoricoRefresh_Finish(result);
            }
        }

        private void ListHistoricoRefresh_Finish(
            ServiceResult<List<PosicaoHistorico>> paramResult
        )
        {
            if (!_tokensource.IsCancellationRequested)
            {

                UpdateToken(paramResult.RefreshToken);

                if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                {
                    if (paramResult.Data.Count > 0)
                    {

                        this.ListHistorico_Source = paramResult.Data;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.SelectedItem = paramResult.Data.FirstOrDefault();

                        });

                        MontaMapa();
                    }

                }
                else
                {
                    ShowErrorAlert(paramResult.MessageError);
                }

                BtnTopIsEnabled = true;
                this.ListHistorico_IsRefreshing = false;
                this._viewHistorico.ExibirLoad();
                this.MaisCommandIsEnabled = true;
                this._viewHistorico.EscondeLoad();
                this._viewHistorico.mapaHistorico.CameraIdled -= MapaHistorico_CameraChanged;
            }
        }

        private void MontaMapa()
        {
            try
            {

                this._viewHistorico.exibeBuscarMais = true;

                if (this.ListHistorico_Source != null)
                {
                    if (this.ListHistorico_Source.Count > 0)
                    {
                        try
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _lstPosition = _bllMapa.MontaMapaListaHistorico(
                                    this.ListHistorico_Source
                                );

                                CentralizarMapa();

                            });

                            DataUltimaPosicao = ListHistorico_Source
                                .LastOrDefault()
                                .DataEvento.Value.ToLocalTime();
                            //.AddHours(periodoBusca);

                        }
                        catch
                        {
                            _lstPosition = null;
                            ShowErrorAlert("Exception");
                        }

                    }
                    else
                    {
                        if (DataUltimaPosicao != null)
                        {
                            DataUltimaPosicao = DataUltimaPosicao
                                .Value
                                .AddHours(periodoBusca);
                        }
                        else
                        {
                            DataUltimaPosicao = DateTime.Now;
                            DataUltimaPosicao = DataUltimaPosicao
                                .Value
                                .AddHours(periodoBusca);
                        }

                        this._viewHistorico.EscondeLoad();
                        this._viewHistorico.mapaHistorico.CameraIdled -= MapaHistorico_CameraChanged;
                    }
                }
                else
                {
                    DataUltimaPosicao = DataUltimaPosicao
                        .Value
                        .AddHours(periodoBusca);

                    this._viewHistorico.EscondeLoad();
                    this._viewHistorico.mapaHistorico.CameraIdled -= MapaHistorico_CameraChanged;
                }

                //ExibeSubTitulo(DataUltimaPosicao.Value.ToLocalTime());
                ExibeSubTitulo(ListHistorico_Source.FirstOrDefault()
                                .DataEvento.Value.ToLocalTime());
            }
            catch
            {
                _lstPosition = null;
                ShowErrorAlert("Exception");
            }
        }

        private async Task Loop()
        {
            try
            {

                if (!this.ListHistorico_IsRefreshing)
                {
                    this.ListHistoricoRefresh(null);
                }

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }

        }

        #region Mapa
        public async void MapaAction()
        {
            if (_isMapaBig)
            {
                _isMapaBig = false;
                MapaButtonBackgroundColor = _desactiveBackgroundColor;
                MapaTelaNormal();
            }
            else
            {
                _isMapaBig = true;
                MapaButtonBackgroundColor = _activeBackgroundColor;
                MapaTelaCheia();
            }

            CentralizarMapa();

        }

        public async void MapaTypeAction()
        {
            String answer = await
                _messageService.ShowMessageAsync(
                    AppResources.MapType
                    , AppResources.cancelar
                    , null
                    , new string[]{
                    AppResources.Map
                    , AppResources.Satellite
                    , AppResources.Hybrid
                    , AppResources.Terrain
                }
                );

            if (answer == AppResources.Hybrid)
            {
                this._viewHistorico.mapaHistorico.MapType = MapType.Hybrid;
            }
            else if (answer == AppResources.Satellite)
            {
                this._viewHistorico.mapaHistorico.MapType = MapType.Satellite;
            }
            else if (answer == AppResources.Terrain)
            {
                this._viewHistorico.mapaHistorico.MapType = MapType.Terrain;
            }
        }

        public void CentralizarMapa()
        {
            if (_lstPosition != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _bllMapa.CentralizarMapa(Bounds.FromPositions(
                        _lstPosition
                    ));

                });
            }
        }
        #endregion

        public void BuscarMais(object obj)
        {
            Refresh(DataUltimaPosicao);
        }

        public void MapaHistorico_CameraChanged(
            object sender
            , CameraIdledEventArgs e
        )
        {
            if (!this.ListHistorico_IsRefreshing)
            {
                this._viewHistorico.EscondeLoad();
                this._viewHistorico.mapaHistorico.CameraIdled -= MapaHistorico_CameraChanged;
            }
        }

        public void MapaTelaNormal()
        {
            HeightPainelMapa = 36 * (DefaultHeightContent / 100);
        }

        public void MapaTelaCheia()
        {
            HeightPainelMapa = DefaultHeightContent;
        }

        public void ExibeTitulo()
        {
            TxtTitulo = posicaoHistorico.IdentificacaoUnidadeRastreada;
        }

        public void ExibeSubTitulo(DateTime? paramData)
        {
            String dataString = "";
            if (paramData.HasValue)
            {
                dataString = String.Format(
                    "{0:dd/MM/yyyy HH:mm:ss}"
                    , paramData.Value.ToLocalTime()
                );

            }

            TxtSubTitulo = dataString;
        }
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
