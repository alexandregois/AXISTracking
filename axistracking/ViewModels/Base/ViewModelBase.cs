using System;
using System.ComponentModel;
using System.Linq;
using axistracking.Domain;
using axistracking.Domain.Realm;
using axistracking.Resx;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels.Services;
using axistracking.Views;
using Xamarin.Forms;

namespace axistracking.ViewModels.Base
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected App _app => (Application.Current as App);
        public IMessageService _messageService { get; set; }
        public INavigationService _navigationService { get; set; }

        public Color _activeBackgroundColor
        {
            get
            {
                return Color.Orange;
            }
        }

        public Color _desactiveBackgroundColor
        {
            get
            {
                return Color.Transparent;
            }
        }

        private Double _defaultWidth;
        public Double DefaultWidth
        {
            get
            {
                return _defaultWidth;
            }
            set
            {
                _defaultWidth = value;
                this.Notify("DefaultWidth");
            }
        }

        private Double _defaultHeight;
        public Double DefaultHeight
        {
            get
            {
                return _defaultHeight;
            }
            set
            {
                _defaultHeight = value;
                this.Notify("DefaultHeight");
            }
        }

        private Double _defaultHeightContent;
        public Double DefaultHeightContent
        {
            get
            {
                return _defaultHeightContent;
            }
            set
            {
                _defaultHeightContent = value;
                this.Notify("DefaultHeightContent");
            }
        }

        private Double _shadowHeight;
        public Double ShadowHeight
        {
            get
            {
                return _shadowHeight;
            }
            set
            {
                _shadowHeight = value;
                this.Notify("ShadowHeight");
            }
        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public ViewModelBase()
        {

            this.DefaultTemplateBuild();

            this._messageService =
                    DependencyService.Get<IMessageService>();
            this._navigationService =
                    DependencyService.Get<INavigationService>();


            DefaultWidth = _app.ScreenWidth;
            DefaultHeight = _app.ScreenHeight;
            DefaultHeightContent = _app.DefaultTemplateHeightContent;
            ShadowHeight = DefaultHeightContent;
        }

        public void Notify(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void OnAppearing();

        public abstract void OnDisappearing();

        public abstract void OnLayoutChanged();


        public String BuscaTraducaoError(
            String paramMessageError
        )
        {
            String erroMessage;
            try
            {
                erroMessage =
                    AppResources
                        .ResourceManager
                        .GetString(paramMessageError);

                if (String.IsNullOrWhiteSpace(erroMessage))
                    erroMessage = AppResources.Exception;

            }
            catch
            {
                erroMessage = AppResources.Exception;
            }
            return erroMessage;
        }

        public void ShowErrorAlert(
            String paramMessageError
        )
        {
            String erroMessage = BuscaTraducaoError(paramMessageError);

            _messageService.ShowAlertAsync(
                erroMessage
                , AppResources.Erro
            );
        }

        #region Template
        private ImageSource _imageSourceProperty;
        public ImageSource ImageSourceProperty
        {
            get
            {
                return _imageSourceProperty;
            }
            set
            {
                _imageSourceProperty = value;
                this.Notify("ImageSourceProperty");
            }
        }

        private Double _imageWidthProperty;
        public Double ImageWidthProperty
        {
            get
            {
                return _imageWidthProperty;
            }
            set
            {
                _imageWidthProperty = value;
                this.Notify("ImageWidthProperty");
            }
        }

        private View _boxLeftContent;
        public View BoxLeftContent
        {
            get
            {
                return _boxLeftContent;
            }
            set
            {
                _boxLeftContent = value;
                this.Notify("BoxLeftContent");
            }
        }
        private View _boxMiddleContent;
        public View BoxMiddleContent
        {
            get
            {
                return _boxMiddleContent;
            }
            set
            {
                _boxMiddleContent = value;
                this.Notify("BoxMiddleContent");
            }
        }

        private View _boxRightContent;
        public View BoxRightContent
        {
            get
            {
                return _boxRightContent;
            }
            set
            {
                _boxRightContent = value;
                this.Notify("BoxRightContent");
            }
        }

        public Label PanelTituloLabel_Titulo()
        {
            return new Label()
            {
                TextColor = Color.White,
                FontSize = 16,
                Margin = new Thickness(0),
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                LineBreakMode = LineBreakMode.TailTruncation
            };
        }

        public Label PanelTituloLabel_SubTitulo()
        {
            return new Label()
            {
                TextColor = Color.White,
                FontSize = 12,
                Margin = new Thickness(0),
                VerticalTextAlignment = TextAlignment.Start,
                HeightRequest = _app.DefaultTemplateHeightNavegationBar / (double)2,
                LineBreakMode = LineBreakMode.TailTruncation
            };
        }

        public Button VoltarButtonDefault()
        {
            return new Button()
            {
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Image = "ic_retornar.png",
                BackgroundColor = Color.Transparent,
                BorderRadius = 0,
                BorderWidth = 0,
                HeightRequest = _app.DefaultTemplateHeightNavegationBar,
                WidthRequest = 30,
                Command = new Command(() => this._navigationService.Voltar())
            };
        }

        public abstract void DefaultTemplateBuild();

        #endregion



        #region UpdateToken
        public void UpdateToken(Token paramRefreshToken)
        {
            try
            {
                if (paramRefreshToken != null)
                {

                    TokenRealm _tokenRealm = new TokenRealm();
                    _tokenRealm.Id = 1;
                    _tokenRealm.Access_Token = paramRefreshToken.Access_token;
                    _tokenRealm.UrlLogo = paramRefreshToken.UrlLogo;
                    _tokenRealm.LstFuncao = paramRefreshToken.LstFuncao;
                    _tokenRealm.NomeCliente = paramRefreshToken.NomeCliente;
                    _tokenRealm.NomeUsuario = paramRefreshToken.NomeUsuario;
                    _tokenRealm.IdAplicativo = paramRefreshToken.Aplicativo.IdAplicativo;

                    TokenDataStore store = new TokenDataStore();
                    store.CreateUpadate(_tokenRealm);
                    RefazPaginaLista();

                    Token _token = new Token();
                    _token.TransformFromRealm(_tokenRealm);
                    _app.Token = _token;

                }
            }
            catch (Exception)
            {
            }
        }

        private void RefazPaginaLista()
        {
            /*Page paginaAtual = Application.Current.MainPage.Navigation.NavigationStack.Last();
            if (paginaAtual.GetType() == typeof(ViewPainel))
            {
                ViewModelPainel atualPage = (ViewModelPainel)this;
                atualPage.DefaultTemplateBuild();
            }*/

            Page paginaAtual = Application.Current.MainPage.Navigation.NavigationStack.Last();
            if (paginaAtual.GetType() == typeof(ViewModulos))
            {
                ViewModelModulos atualPage = (ViewModelModulos)this;
                atualPage.DefaultTemplateBuild();
            }

        }

        #endregion

    }

#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}