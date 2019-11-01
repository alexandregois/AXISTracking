using System;
using System.Collections.Generic;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
    public class ListComandos_ViewCell : ViewCellBase
    {
        protected App _app => (Application.Current as App);

        private readonly IMessageService _messageService;

        private ViewModelHistoricoComando _viewHistoricoComando;

        public EnumPage _actualPage { get; set; }

        public ListComandos_ViewCell(
            Object paramContext)
        : base(Color.White)
        {

            _viewHistoricoComando = new ViewModelHistoricoComando();

            this._messageService =
                DependencyService.Get<IMessageService>();
        }

        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();

                ComandoLog comandos = (ComandoLog)BindingContext;

                if (comandos != null)
                {

                    //Stack Principal
                    Grid boxPrincipal = this.BuildCell(comandos);

                    View = boxPrincipal;

                }
            }
            catch
            {

            }
        }

        public Grid BuildCell(
            ComandoLog paramComando
            , View paramView = null
        )
        {
            try
            {

                Double margin = 10;

                #region Grid
                Grid boxPrincipal = new Grid();
                //TODO: Remover
                boxPrincipal.WidthRequest = _app.ScreenWidth;
                boxPrincipal.ColumnSpacing = 0;
                boxPrincipal.RowSpacing = 0;
                boxPrincipal.VerticalOptions = LayoutOptions.FillAndExpand;

                boxPrincipal.ColumnDefinitions = new ColumnDefinitionCollection();
                ColumnDefinition col01 = new ColumnDefinition()
                {
                    Width = 37
                };


                ColumnDefinition col02 = new ColumnDefinition()
                {
                    Width = GridLength.Star
                };

                //boxPrincipal.ColumnDefinitions.Add(col01);
                boxPrincipal.ColumnDefinitions.Add(col02);

                boxPrincipal.RowDefinitions = new RowDefinitionCollection();
                boxPrincipal.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Star
                });
                #endregion

                #region Texto
                StackLayout boxTexto = new StackLayout()
                {
                    Margin = new Thickness(margin),
                    Spacing = 0,
                    Padding = 0,
                    Orientation = StackOrientation.Vertical,
                    WidthRequest = col02.Width.Value,
                    VerticalOptions = LayoutOptions.Center
                };

                Label labelComando = new Label()
                {
                    Text = paramComando.IdentificacaoComando,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 2),
                    LineBreakMode = LineBreakMode.TailTruncation,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                boxTexto.Children.Add(labelComando);

                Label labelDataEvento = new Label()
                {
                    Text = String.Format(
                        "{0:dd/MM/yyyy HH:mm:ss}"
                        , paramComando.DataFila.ToLocalTime()
                    ),
                    TextColor = Color.FromHex("#9b9eb0"),
                    FontSize = 14,
                    Margin = labelComando.Margin,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    HorizontalTextAlignment = TextAlignment.Start
                };

                if (!String.IsNullOrWhiteSpace(paramComando.StatusComando))
                    labelDataEvento.Text += " - " + paramComando.StatusComando;

                boxTexto.Children.Add(labelDataEvento);

                if (!String.IsNullOrWhiteSpace(paramComando.UnidadeRastreada))
                {
                    Label labelUnidadeRastreada = new Label()
                    {
                        Text = paramComando.IdentificacaoUnidadeRastreada,
                        TextColor = Color.Black,
                        FontSize = 14,
                        Margin = new Thickness(0),
                        LineBreakMode = LineBreakMode.TailTruncation,
                        HorizontalTextAlignment = TextAlignment.Start
                    };
                    boxTexto.Children.Add(labelUnidadeRastreada);
                }

                boxPrincipal.Children.Add(boxTexto, 0, 0);

                boxPrincipal.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(BoxPrincipal_Tap),
                    CommandParameter = paramComando,
                    NumberOfTapsRequired = 1
                });

                #endregion

                return boxPrincipal;

            }
            catch
            {
                throw;
            }
        }

        private async void BoxPrincipal_Tap(object obj)
        {
            String answer;

            ComandoLog paramComando = (ComandoLog)obj;

            List<String> respostas = new List<String>
                {
                    AppResources.DetalhesComando
                };

            if (paramComando.Status == 1 || paramComando.Status == 3)
            {
                respostas.Add(AppResources.CancelarComando);
            }

            answer = await
                this._messageService.ShowMessageAsync(
                    paramComando.IdentificacaoComando
                    , AppResources.cancelar
                    , null
                    , respostas.ToArray()
                );


            if (answer == AppResources.CancelarComando)
            {
                Boolean confirm = await this._messageService.ShowAlertChooseAsync(
                AppResources.CancelarComandoConfirma
                , AppResources.cancelar
                , AppResources.Ok
                , null);

                if (confirm == true)
                {
                    await _viewHistoricoComando.EnviarComandoCancelar(paramComando, true);
                }
            }


            if (answer == AppResources.DetalhesComando)
            {

                //String strDetalhes = "<strong>" + AppResources.Status + "</strong>" + "\n" + paramComando.StatusComando +
                //        "\n" +
                //        "\n" +
                //        "<strong>" + AppResources.DataFila + "</strong>" + "\n" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", paramComando.DataFila.ToLocalTime()) +
                //        "\n" +
                //        "\n" +
                //        "<strong>" + AppResources.CriadoPor + "</strong>" + "\n" + paramComando.ClienteEnvio + 
                //        "\n" +
                //        "\n";

                String strDetalhes = paramComando.IdentificacaoComando +
                        "\n" +
                        "\n" +
                        "\n" +
                    AppResources.Status + ":" + "\n" + paramComando.StatusComando +
                        "\n" +
                        "\n" +
                        AppResources.DataFila + ":" + "\n" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", paramComando.DataFila.ToLocalTime()) +
                        "\n" +
                        "\n" +
                        AppResources.CriadoPor + ":" + "\n" + paramComando.ClienteEnvio +
                        "\n" +
                        "\n";


                List<String> detalhes = new List<String>
                    {
                        AppResources.Status + "\n" + paramComando.StatusComando,
                        "\n",
                        "\n",
                        AppResources.DataFila + "\n" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", paramComando.DataFila.ToLocalTime()),
                        "\n",
                        "\n",
                        AppResources.CriadoPor + "\n" + paramComando.ClienteEnvio

                    };

                Boolean confirmComandoCancelar = false;

                if (paramComando.Status == 1 || paramComando.Status == 3)
                {
                    confirmComandoCancelar = await this._messageService.ShowAlertChooseAsync(
                    strDetalhes
                    , AppResources.Fechar
                    , AppResources.CancelarComando
                    , null);
                }
                else
                {
                    await this._messageService.ShowAlertChooseAsync(
                    strDetalhes
                    , AppResources.Fechar
                    , " "
                    , null);
                }


                if (confirmComandoCancelar == true)
                {
                    await _viewHistoricoComando.EnviarComandoCancelar(paramComando, true);
                }

            }

        }
    }
}
