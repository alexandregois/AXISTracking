using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Bll;
using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels.Base;
using axistracking.ViewModels.Services;
using axistracking.Views;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelPainel : ViewModelBase
    {

        private CancellationTokenSource _tokensource { get; set; }
        private TimeSpan _refreshTime { get; set; }

        public ICommand ViewDetalhesCommand
        {
            get;
            set;
        }

        public ICommand ListPainel_RefreshCommand
        {
            get;
            set;
        }

        List<PainelDto> _listPainel_Source = new List<PainelDto>();
        public List<PainelDto> ListPainel_Source
        {
            get
            {
                return _listPainel_Source;
            }
            set
            {
                _listPainel_Source = value;
                App.ListPainelSource = _listPainel_Source;
                this.Notify("ListPainel_Source");
            }
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

        public ViewModelPainel()
        {
            DefaultTemplateBuild();
            this.ViewDetalhesCommand = new Command(this.ViewDetalhes);
            this.ListPainel_RefreshCommand = new Command(this.ListPainelRefresh);

            _refreshTime = App.Configuracao.RefreshTime;
            _tokensource = new CancellationTokenSource();


            TokenDataStore store = new TokenDataStore();
            TokenRealm _token = store.Get(1);


            if (_token == null)
            {
                _navigationService.NavigateToLogin();

                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });
            }


        }

        public override void OnAppearing()
        {
            try
            {
                DefaultTemplateBuild();

                if (App.ListPainelSource != null && App.ListPainelSource.Count() > 0)
                {
                    PainelDto painelTemp = App.ListPainelSource.FirstOrDefault();
                    if ((DateTime.UtcNow - painelTemp.LastSearch).TotalSeconds
                       < _refreshTime.TotalSeconds)
                    {
                        this.ListPainel_Source = App.ListPainelSource;
                        ListPainel_IsRefreshing = false;
                        return;
                    }
                }
                
                Loop(TimeSpan.FromMilliseconds(500));

                Refresh(null);

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
                if (_messageService == null)
                {
                    _messageService =
                            DependencyService.Get<IMessageService>();
                    //_navigationService =
                    //        DependencyService.Get<INavigationService>();
                }

                //_messageService.ShowAlertAsync(
                //AppResources.LoginNaoEncontrado
                //, AppResources.Erro
                //);

                //_navigationService.NavigateToLogin();

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));

                //});

            }
            else if (_token.IdAplicativo == 0)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));

                });

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

        private void Refresh(object obj)
        {
            if (!this.ListPainel_IsRefreshing)
            {
                _tokensource.Cancel();
                _tokensource = new CancellationTokenSource();
                this.ListPainel_IsRefreshing = true;
                this.ListPainelRefresh(null);
            }
        }

        private void ViewDetalhes(object obj)
        {
            PainelDto painel = (PainelDto)obj;

            EnumPage page;

            string traducao = AppResources.ResourceManager.GetString(painel.Chave);

            if (traducao != AppResources.Comandos)
            {
                if (traducao == AppResources.Alertas)
                {
                    page = EnumPage.DetalhesAlerta;
                }
                else
                {
                    page = EnumPage.DetalhesUnidade;
                }

                this._navigationService.NavigateToDetalhes(null, obj, page);
            }
            else
            {
                page = EnumPage.ListaComandos;
                this._navigationService.NavigateToListarComandos(obj, page);
            }


        }

        private void ListPainelRefresh(object obj)
        {
            ModelPainel _modelPainel = new ModelPainel();
            this.ListPainel_IsRefreshing = true;

            try
            {
                _tokensource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    ServiceResult<List<PainelDto>> result = await _modelPainel.ListPainelAsync(
                        _tokensource.Token
                    );

                    if (!_tokensource.IsCancellationRequested)
                    {
                        UpdateToken(result.RefreshToken);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (!String.IsNullOrWhiteSpace(result.MessageError))
                            {
                                _messageService.ShowAlertAsync(result.MessageError);
                            }

                            App.ListPainelSource = result.Data;
                            this.ListPainel_Source = App.ListPainelSource;
                            this.ListPainel_IsRefreshing = false;

                            Loop(_refreshTime);

                        });
                    }

                }, _tokensource.Token);

            }
            catch (Exception ex)
            {
                _messageService.ShowAlertAsync(ex.Message, AppResources.Erro);
                this._navigationService.NavigateToLogin();
            }
        }

        /*
                
        private async Task Loop(TimeSpan paramRefreshTime)
        {
            try
            {
                await Task.Delay(paramRefreshTime, _tokensource.Token);

                if (!this.ListPainel_IsRefreshing)
                    this.ListPainelRefresh(null);

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                _messageService.ShowAlertAsync(ex.Message, AppResources.Erro);
            }

        }

        */

        public async void ImageLogout(object sender)
        {
            await Deslogar();
        }

        public async Task Deslogar()
        {
            _tokensource = new CancellationTokenSource();

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

                    try
                    {

                        Bll_PushNotification classPushNotification = new Bll_PushNotification();
                        classPushNotification.DeletePushKey(_tokensource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        ModelLogin modelUsuario = new ModelLogin();
                        modelUsuario.Deslogar(_tokensource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }


                });

                this._navigationService.NavigateToLogin();

            }
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
