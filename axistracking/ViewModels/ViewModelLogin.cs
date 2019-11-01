using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Bll;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using axistracking.Model;
using axistracking.Resx;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using family.Domain.Enum;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
    public class ViewModelLogin : ViewModelBase
    {

        public IViewLogin _view { get; set; }

        private CancellationTokenSource _tokensource { get; set; }


        ModelLogin _modelLogin = null;

        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public ICommand LoginCommand
        {
            get;
            set;
        }

        private Color _corFundoBotao;
        public Color CorFundoBotao
        {
            get
            {
                return _corFundoBotao;
            }
            set
            {
                _corFundoBotao = value;
                this.Notify("CorFundoBotao");
            }
        }

        private Color _corFundoLogin;
        public Color CorFundoLogin
        {
            get
            {
                return _corFundoLogin;
            }
            set
            {
                _corFundoLogin = value;
                this.Notify("CorFundoLogin");
            }
        }

        string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                this.Notify("Email");
            }
        }

        string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.Notify("Password");
            }
        }

        public ImageSource ImageSource
        {
            get;
            set;
        }

        public Thickness ImageMargin
        {
            get;
            set;
        }

        public ViewModelLogin(Boolean paramIsPersonalizado, String paramNameProject)
        {

            GetPreferences();

            this.LoginCommand = new Command(this.Login);

            try
            {
                if (!paramIsPersonalizado)
                {

                    if (Application.Current.Properties.ContainsKey("UrlLogo"))
                    {
                        if (Application.Current.Properties["UrlLogo"] != null)
                            this.ImageSource = Xamarin.Forms.ImageSource.FromUri(new Uri((string)Application.Current.Properties["UrlLogo"]));
                        else
                            this.ImageSource = Xamarin.Forms.ImageSource.FromFile("splash_tracking.png");
                    }
                    else
                        this.ImageSource = Xamarin.Forms.ImageSource.FromFile("splash_tracking.png");

                }
                else
                    this.ImageSource = ImageSource.FromFile("splash.png");

            }
            catch (Exception)
            {
                this.ImageSource = ImageSource.FromFile("splash.png");
            }


            if (_app.nameProject == "agility")
            {
                CorFundoLogin = Color.FromHex("#810A9A");                
                CorFundoBotao = Color.FromHex("#FF8833");
            }


            //this.ImageSource = "LogoAxisTracking.png";            
            //this.ImageSource = "splash_tracking.png";
            this.ImageMargin = new Thickness(0, 0, 0, 0);
        }

        public override void OnAppearing()
        {
        }

        public override void OnDisappearing()
        {
            if (_tokensource != null)
                _tokensource = new CancellationTokenSource();
        }

        private void GetPreferences()
        {
            if (Application.Current.Properties.ContainsKey("User"))
                Email = (string)Application.Current.Properties["User"];

            if (Application.Current.Properties.ContainsKey("Pass"))
                Password = (string)Application.Current.Properties["Pass"];
        }

        public override void OnLayoutChanged()
        {
        }

        public override void DefaultTemplateBuild()
        {
        }

        private void SetPreferences(String User, String Pass)
        {
            Application.Current.Properties["User"] = User;
            Application.Current.Properties["Pass"] = Pass;
        }

        private async void Login()
        {
            try
            {

                _view.ExibirLoad();
                if (String.IsNullOrEmpty(this.Email) || String.IsNullOrEmpty(this.Password))
                {
                    await this._messageService.ShowAlertAsync(
                        "E-mail e/ou Senha inválidos."
                        , AppResources.Erro
                    );
                    _view.EscondeLoad();
                    return;
                }

                _modelLogin = new ModelLogin();
                _tokensource = new CancellationTokenSource();


                SetPreferences(Email, Password);


                Task.Run(async () =>
                {
                    try
                    {
                        String paramIdSistemaOperacional;

                        String strIndentificacao = String.Empty;

                        if (!String.IsNullOrEmpty(_app.Util.GetIdentifier()))
                            strIndentificacao = _app.Util.GetIdentifier();
                        else
                            strIndentificacao = CrossDeviceInfo.Current.Id;


                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            paramIdSistemaOperacional =
                                ((byte)EnumSistemaOperacional.iOS).ToString();
                        }
                        else
                        {
                            paramIdSistemaOperacional =
                                ((byte)EnumSistemaOperacional.Android).ToString();
                        }



                        ServiceResult<Token> result =
                            await _modelLogin.Login(
                                this.Email
                                , this.Password
                                , App.Configuracao.CodigoEmpresa
                                , "10"
                                , strIndentificacao
                                , paramIdSistemaOperacional
                                , _tokensource.Token
                            );

                        if (!_tokensource.IsCancellationRequested)
                        {
                            if (String.IsNullOrWhiteSpace(result.MessageError))
                            {
                                Application.Current.Properties["UrlLogo"] = result.Data.UrlLogo;

                                UpdateToken(result.Data);

                                App.ListPainelSource = result.Data.LstDashBoard;


                                Bll_PushNotification classPushNotification = new Bll_PushNotification();
                                classPushNotification.RegistraPushKey(_tokensource.Token);


                                ////Pagina apos o Login
                                _navigationService.NavigateToPainel();

                                //_navigationService.NavigateToModulos();


                            }
                            else
                            {
                                _view.EscondeLoad();
                                _messageService.ShowAlertAsync(
                                    result.MessageError
                                    , AppResources.Erro
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _view.EscondeLoad();

                        ShowErrorAlert("Exception");
                    }

                }, _tokensource.Token);

            }
            catch
            {
                _view.EscondeLoad();
                ShowErrorAlert("Exception");
            }

        }
    }
#pragma warning restore CS4014
#pragma warning restore RECS0022
}
