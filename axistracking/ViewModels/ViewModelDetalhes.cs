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
    public class ViewModelDetalhes : ViewModelBaseListPage
    {
        public IViewDetalhes _viewDetalhes;
        private EnumPage _page { get; set; }

        private CancellationTokenSource _tokensource { get; set; }
        private TimeSpan _refreshTime { get; set; }

        private PainelDto _painel { get; set; }

        private Posicao _unidade { get; set; }

        private Bll_Mapa _bllMapa { get; set; }

        private String[] _arrayEventos { get; set; }

        private String EventoFiltro { get; set; }

        public ICommand RefreshCommand
        {
            get;
            set;
        }

        public ICommand ListUnidades_RefreshCommand
        {
            get;
            set;
        }

        public ICommand ListaEventoCommand
        {
            get;
            set;
        }


        private string _txtTitulo;
        public string TxtTitulo
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

        Boolean _ativaControle = true;
        public Boolean AtivaControle
        {
            get
            {
                return _ativaControle;
            }
            set
            {
                _ativaControle = value;
                this.Notify("AtivaControle");
            }
        }

        Double _heightRequestDefault;
        public Double HeightRequestDefault
        {
            get
            {
                return _heightRequestDefault;
            }
            set
            {
                _heightRequestDefault = value;
                this.Notify("HeightRequestDefault");
            }
        }

        private List<PosicaoHistorico> _list_SourceCompleta = new List<PosicaoHistorico>();
        private List<PosicaoHistorico> _list_Source = new List<PosicaoHistorico>();
        public List<PosicaoHistorico> List_Source
        {
            get
            {
                return _list_Source;
            }
            set
            {
                _list_Source = value;
                if (_painel.Id == 2)// Chave == Alertas
                {
                    _viewDetalhes.ExibeTitulo(_list_Source.Count());
                }
                else
                {
                    //_viewDetalhes.ExibeTitulo(_list_Source.Select(p => p.IdUnidadeRastreada).Distinct().Count());
                    _viewDetalhes.ExibeTitulo(_list_Source.Select(p => p.IdUnidadeRastreada).Count());
                }

                //App.ListUnidadesSource = _listUnidades_Source;
                this.Notify("List_Source");
            }
        }

        Boolean _listUnidades_IsRefreshing;
        public Boolean ListUnidades_IsRefreshing
        {
            get
            {
                return _listUnidades_IsRefreshing;
            }
            set
            {
                _listUnidades_IsRefreshing = value;
                ListEndRefresh = !value;
                this.Notify("ListUnidades_IsRefreshing");
            }
        }

        Boolean _painelMapaBoxIsVisible = false;
        public Boolean PainelMapaBoxIsVisible
        {
            get
            {
                return _painelMapaBoxIsVisible;
            }
            set
            {
                _painelMapaBoxIsVisible = value;
                this.Notify("PainelMapaBoxIsVisible");
            }
        }

        Boolean _painelDefaultIsVisible = true;
        public Boolean PanelDefaultIsVisible
        {
            get
            {
                return _painelDefaultIsVisible;
            }
            set
            {
                _painelDefaultIsVisible = value;
                ListPainelTop_IsVisible = value;
                List_IsVisible = value;
            }
        }

        Color _filtroEventosBackgroundColor;
        public Color FiltroEventosBackgroundColor
        {
            get
            {
                return _filtroEventosBackgroundColor;
            }
            set
            {
                _filtroEventosBackgroundColor = value;
                this.Notify("FiltroEventosBackgroundColor");
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


        #region Mapa
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

        private Boolean _buttonMapaTypeVisible = false;
        public Boolean ButtonMapaTypeVisible
        {
            get
            {
                return _buttonMapaTypeVisible;
            }
            set
            {
                _buttonMapaTypeVisible = value;
                this.Notify("ButtonMapaTypeVisible");
            }
        }
        #endregion

        public ViewModelDetalhes(
            object obj
            , EnumPage Page
        )
        {
            DefaultTemplateBuild();
            _page = Page;

            _painel = (PainelDto)obj;

            this.RefreshCommand = new Command(this.Refresh);
            this.MapaCommand = new Command(this.MapaAction);
            this.MapaTypeCommand = new Command(this.MapaTypeAction);
            this.BuscarCommand = new Command(this.Buscar);

            this.ListaEventoCommand = new Command(this.ListaEvento);

            _refreshTime = App.Configuracao.RefreshTime;
            _tokensource = new CancellationTokenSource();

            _filtroEventosBackgroundColor = _desactiveBackgroundColor;
            _mapaButtonBackgroundColor = _desactiveBackgroundColor;

        }

        public override void OnDisappearing()
        {
            if (_tokensource != null)
                _tokensource.Cancel(false);
        }

        public override void OnAppearing()
        {
            try
            {
                DefaultTemplateBuild();

                if (_bllMapa == null)
                    _bllMapa = new Bll_Mapa(this._viewDetalhes.MapaDetalhes);

                this.ListPainelTop_Source = _painel;

                ListPainelTopColumn_Width = DefaultWidth / (double)2;
                if (App.ListPainelTopUnidadesSource != null && App.ListPainelTopUnidadesSource.Count() > 0)
                {
                    PainelDto painelTopTemp = App.ListPainelTopUnidadesSource.FirstOrDefault();
                    if ((DateTime.UtcNow - painelTopTemp.LastSearch).TotalSeconds
                        < _refreshTime.TotalSeconds)
                    {
                        this.List_Source = App.ListUnidadesSource;
                        _listUnidades_IsRefreshing = false;


                        return;
                    }
                }

                Loop(TimeSpan.FromMilliseconds(500));

            }
            catch
            {
            }
        }

        public override void OnLayoutChanged()
        {
        }

        public override void DefaultTemplateBuild()
        {
            try
            {
                BoxLeftContent = VoltarButtonDefault();

                Label titulo = PanelTituloLabel_Titulo();
                titulo.FontSize = 20;

                titulo.SetBinding(
                    Label.TextProperty
                    , new TemplateBinding("Parent.BindingContext.TxtTitulo")
                );

                BoxMiddleContent = titulo;

                StackLayout stackRight = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = 0,
                    Spacing = 0
                };

                Button filtroEventos = new Button()
                {
                    Image = "ic_filtro.png",
                    HeightRequest = stackRight.HeightRequest,
                    WidthRequest = 30,
                    Margin = 0,
                    BorderRadius = 0,
                    BorderWidth = 0,
                    BorderColor = Color.Transparent
                };

                filtroEventos.SetBinding(
                    Button.CommandProperty
                    , new TemplateBinding("Parent.BindingContext.ListaEventoCommand")
                );

                filtroEventos.SetBinding(
                    VisualElement.IsEnabledProperty
                    , new TemplateBinding("Parent.BindingContext.ListEndRefresh")
                );

                filtroEventos.SetBinding(
                    Button.BackgroundColorProperty
                    , new TemplateBinding("Parent.BindingContext.FiltroEventosBackgroundColor")
                );

                stackRight.Children.Add(filtroEventos);

                Button mapaViewDetalhes = new Button()
                {
                    Image = "ic_mapa.png",
                    HeightRequest = stackRight.HeightRequest,
                    WidthRequest = 30,
                    Margin = new Thickness(5, 0, 0, 0),
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
                    , new TemplateBinding("Parent.BindingContext.ListEndRefresh")
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
                    Margin = filtroEventos.Margin,
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
                    , new TemplateBinding("Parent.BindingContext.ListEndRefresh")
                );

                mapaTypeViewDetalhes.SetBinding(
                    VisualElement.IsVisibleProperty
                    , new TemplateBinding("Parent.BindingContext.ButtonMapaTypeVisible")
                );

                stackRight.Children.Add(mapaTypeViewDetalhes);

                BoxRightContent = stackRight;
            }
            catch
            {
            }
        }

        private void ListPainelTopRefresh()
        {

            ModelPainel _modelPainel = new ModelPainel();
            _viewDetalhes.ExibirLoad();

            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        //ServiceResult<List<PainelDto>> resultPainel = await _modelPainel.ListPainelAsync();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                this.ListPainelTop_Source = _painel;

                            }
                            catch (Exception)
                            {
                            }
                        });


                    }
                    catch (Exception)
                    {
                    }
                }, _tokensource.Token);

            }
            catch (Exception)
            {
                _viewDetalhes.EscondeLoad();
            }
            finally
            {
            }
        }

        public override void ListRefresh(object obj)
        {
            ModelDetalhes _modelUnidades = new ModelDetalhes();
            this.ListUnidades_IsRefreshing = true;

            try
            {
                _tokensource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    try
                    {
                        AtivaControle = false;
                        ListEndRefresh = false;

                        ServiceResult<List<PosicaoHistorico>> resultUnidades =
                            new ServiceResult<List<PosicaoHistorico>>();

                        Task<ServiceResult<List<PosicaoHistorico>>> taskUnidades;

                        if (_painel.Id == Convert.ToInt32(EnumPainelGrafico.Alertas))
                        {
                            taskUnidades = _modelUnidades.ListAlertasAsync(
                                _tokensource.Token
                            );

                            //_painel.Grafico = null;
                        }
                        else
                        {
                            taskUnidades = _modelUnidades.ListUnidadesAsync(
                                _tokensource.Token
                            );
                        }

                        resultUnidades = await taskUnidades;


                        if (!_tokensource.IsCancellationRequested)
                        {
                            App.ListUnidadesSource = resultUnidades.Data;
                            UpdateToken(resultUnidades.RefreshToken);

                            if (!String.IsNullOrWhiteSpace(resultUnidades.MessageError))
                            {
                                ShowErrorAlert(resultUnidades.MessageError);
                            }

                            _list_SourceCompleta = resultUnidades.Data;
                            List_Source = _list_SourceCompleta;
                            ListUnidades_IsRefreshing = false;

                            if (List_Source != null)
                            {
                                //Prenche a lista de eventos.
                                List<string> listEventos = this.List_Source.Select(
                                    x => x.NomeRegraViolada
                                ).Distinct().ToList();

                                _arrayEventos = listEventos.Where(x => x != null).ToArray();
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        ShowErrorAlert("Exception");
                    }
                    finally
                    {
                        _viewDetalhes.EscondeLoad();

                        _viewDetalhes.EscondePercentual();

                        ListEndRefresh = true;

                        AtivaControle = true;
                    }
                }, _tokensource.Token);

            }
            catch (Exception)
            {
                this._messageService.ShowAlertAsync(AppResources.Exception);
                this.ListUnidades_IsRefreshing = false;
            }
        }

        private async Task Loop(TimeSpan paramRefreshTime)
        {

            try
            {
                await Task.Delay(paramRefreshTime, _tokensource.Token);

                if (!this.ListUnidades_IsRefreshing)
                {
                    this.ListPainelTopRefresh();
                    this.ListRefresh(null);
                }

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }

        }

        private void Refresh(object obj)
        {
            if (!this.ListUnidades_IsRefreshing)
            {
                _tokensource.Cancel();
                _tokensource = new CancellationTokenSource();
                this.ListRefresh(null);
            }
        }

        public override void Buscar()
        {
            FilterUnidades(
                this.StatusFiltro
                , this.TxtBuscar
                , this.EventoFiltro
            );
        }

        public void FilterUnidades(
            Byte? paramStatus = null
            , String paramText = null
            , String paramTextEvento = null
        )
        {

            List<PosicaoHistorico> _list_SourceTratada = new List<PosicaoHistorico>();

            if (paramStatus != null)
            {
                if (paramStatus.Value == 0)
                {
                    List_Source = _list_SourceCompleta.Where(
                        x =>
                        (paramStatus.HasValue ? x.StatusRastreadorUnidadeRastreada == paramStatus.Value : true)
                        && (String.IsNullOrWhiteSpace(paramText) ? true : x.Identificacao.ToLower().Contains(paramText.ToLower()))
                        && (String.IsNullOrWhiteSpace(paramTextEvento) ? true : x.NomeRegraViolada.ToLower() == paramTextEvento.ToLower())
                    ).ToList();

                    //List_Source = _list_SourceTratada.ToList();

                }
                else
                {
                    List_Source = _list_SourceCompleta.Where(
                        x =>
                        (paramStatus.HasValue ? x.StatusRastreadorUnidadeRastreada == 1 : true) || (paramStatus.HasValue ? x.StatusRastreadorUnidadeRastreada == 2 : true)
                        && (String.IsNullOrWhiteSpace(paramText) ? true : x.Identificacao.ToLower().Contains(paramText.ToLower()))
                        && (String.IsNullOrWhiteSpace(paramTextEvento) ? true : x.NomeRegraViolada.ToLower() == paramTextEvento.ToLower())
                    ).ToList();

                    //List_Source = _list_SourceTratada.ToList();

                }
            }
            else
            {
                List_Source = _list_SourceCompleta.Where(
                    x =>
                    (paramStatus.HasValue ? x.Status == paramStatus.Value : true)
                    && (String.IsNullOrWhiteSpace(paramText) ? true : x.Identificacao.ToLower().Contains(paramText.ToLower()))
                    && (String.IsNullOrWhiteSpace(paramTextEvento) ? true : x.NomeRegraViolada.ToLower() == paramTextEvento.ToLower())
                ).ToList();
            }


            if (PainelMapaBoxIsVisible)
            {
                _bllMapa.LimpaMapa();
                MontaMapa();
            }

        }

        private void MapaAction(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (PainelMapaBoxIsVisible)
                {
                    PainelMapaBoxIsVisible = false;
                    PanelDefaultIsVisible = true;
                    ButtonMapaTypeVisible = false;
                    _bllMapa.LimpaMapa();
                    MapaButtonBackgroundColor = _desactiveBackgroundColor;
                }
                else
                {
                    PainelMapaBoxIsVisible = true;
                    PanelDefaultIsVisible = false;
                    ButtonMapaTypeVisible = true;
                    MapaButtonBackgroundColor = _activeBackgroundColor;
                    MontaMapa();
                }
            });
        }

        private async void MapaTypeAction(object obj)
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
                    , AppResources.Trafego
                }
                );

            if (answer == AppResources.Hybrid)
            {
                this._viewDetalhes.MapaDetalhes.MapType = MapType.Hybrid;
                this._viewDetalhes.MapaDetalhes.IsTrafficEnabled = false;

            }
            else if (answer == AppResources.Satellite)
            {
                this._viewDetalhes.MapaDetalhes.MapType = MapType.Satellite;
                this._viewDetalhes.MapaDetalhes.IsTrafficEnabled = false;
            }
            else if (answer == AppResources.Terrain)
            {
                this._viewDetalhes.MapaDetalhes.MapType = MapType.Terrain;
                this._viewDetalhes.MapaDetalhes.IsTrafficEnabled = false;
            }
            else if (answer == AppResources.Trafego)
            {
                this._viewDetalhes.MapaDetalhes.MapType = MapType.Street;
                this._viewDetalhes.MapaDetalhes.IsTrafficEnabled = true;
            }
            else
            {
                this._viewDetalhes.MapaDetalhes.MapType = MapType.Street;
                this._viewDetalhes.MapaDetalhes.IsTrafficEnabled = false;
            }

        }

        private void MontaMapa()
        {

            Device.BeginInvokeOnMainThread(() =>
            {

                try
                {

                    if (this.List_Source != null)
                    {
                        if (this.List_Source.Count > 0)
                        {
                            try
                            {
                                if (this.List_Source[0].Latitude != null)
                                {

                                    Position paramPosition = new Position(
                                        Convert.ToDouble(this.List_Source[0].Latitude)
                                        , Convert.ToDouble(this.List_Source[0].Longitude)
                                    );

                                    List<Position> lst = new List<Position>();

                                    var culture = System.Globalization.CultureInfo.CurrentCulture;

                                    foreach (var posicao in this.List_Source)
                                    {
                                        //if (culture.Name == "pt-BR" && posicao.Latitude < 0)
                                            lst.Add(_bllMapa.MontaMapaPosicao(posicao));
                                    }

                                    Bounds bound = Bounds.FromPositions(lst);

                                    //if (bound.Center.Latitude > 0)
                                    //{
                                    //    if (lst.Count > 1)
                                    //    {
                                    //        List<Position> lst2 = new List<Position>();
                                    //        foreach (var posicao2 in lst)
                                    //        {
                                    //            if (lst2.Count < 2)
                                    //                lst2.Add(posicao2);
                                    //        }
                                    //        bound = Bounds.FromPositions(lst2);
                                    //    }

                                    //}

                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        this._viewDetalhes.MapaDetalhes.MoveToRegion(MapSpan.FromBounds(bound), true);

                                    });


                                }

                            }
                            catch (Exception ex)
                            {
                                this._messageService.ShowAlertAsync(
                                    ex.Message
                                    , "Erro"
                                );
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    this._messageService.ShowAlertAsync(
                        ex.Message
                        , "Erro"
                    );
                }

            });

        }

        private async void ListaEvento()
        {
            try
            {
                if (_arrayEventos != null)
                {
                    String answer;

                    answer = await
                        this._messageService.ShowMessageAsync(
                            AppResources.FiltroEventos
                            , AppResources.cancelar
                            , null
                            , _arrayEventos
                        );


                    if (answer != AppResources.cancelar)
                    {
                        EventoFiltro = answer;
                        FiltroEventosBackgroundColor = _activeBackgroundColor;
                    }
                    else
                    {
                        EventoFiltro = null;
                        FiltroEventosBackgroundColor = _desactiveBackgroundColor;
                    }

                    Buscar();
                }

            }
            catch { }

        }

        public override void MudarTamanhoLoad()
        {
            if (_viewDetalhes != null)
                _viewDetalhes.MudarTamanhoLoad();
        }

        public override void ExibeTitulo(int? paramTotal)
        {
            _viewDetalhes.ExibeTitulo(paramTotal);
        }
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
