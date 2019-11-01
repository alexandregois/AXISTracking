using System;
using axistracking.Domain;
using axistracking.ViewModels.Services;
using Xamarin.Forms;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.Enum;

namespace axistracking.Views.Template
{
    public class BuildCellDetalhes
    {
        private readonly IMessageService _messageService;
        protected App _app => (Application.Current as App);

        public BuildCellDetalhes()
        {

            this._messageService =
                    DependencyService.Get<IMessageService>();

        }

        public Grid BuildCell(
            PosicaoHistorico paramUnidadeRastreada
            , View paramView = null
            , Boolean ShowStatusRastreadorUnidadeRastreada = false
        )
        {
            try
            {

                Double margin = 15;

                #region Grid
                Grid boxPrincipal = new Grid();
                boxPrincipal.WidthRequest = _app.ScreenWidth;
                boxPrincipal.ColumnSpacing = 0;
                boxPrincipal.RowSpacing = 0;
                boxPrincipal.VerticalOptions = LayoutOptions.FillAndExpand;

                boxPrincipal.ColumnDefinitions = new ColumnDefinitionCollection();
                ColumnDefinition col01 = new ColumnDefinition()
                {
                    Width = GridLength.Auto
                };

                ColumnDefinition col03 = new ColumnDefinition()
                {
                    Width = GridLength.Auto
                };


                ColumnDefinition col02;

                col02 = new ColumnDefinition()
                {
                    Width = GridLength.Star
                };

                boxPrincipal.ColumnDefinitions.Add(col01);
                boxPrincipal.ColumnDefinitions.Add(col02);
                boxPrincipal.ColumnDefinitions.Add(col03);


                boxPrincipal.RowDefinitions = new RowDefinitionCollection();
                boxPrincipal.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Star
                });
                #endregion

                #region Frame
                Frame boxRegra = new Frame()
                {
                    CornerRadius = 2,
                    Margin = new Thickness(margin, margin, 0, margin),
                    HasShadow = false,
                    Padding = 0,
                    Opacity = 1,
                    WidthRequest = 27,
                    BackgroundColor = Color.Transparent,
                    OutlineColor = Color.Transparent,
                    VerticalOptions = LayoutOptions.Fill
                };

                if (!String.IsNullOrEmpty(paramUnidadeRastreada.NomeRegraViolada))
                {
                    boxRegra.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(BoxRegra_Tap),
                        CommandParameter = paramUnidadeRastreada,
                        NumberOfTapsRequired = 1
                    });


                    if (String.IsNullOrEmpty(paramUnidadeRastreada.CorRegraPrioritaria))
                        boxRegra.BackgroundColor = Color.FromHex("#FF2E2F3A");
                    else
                        boxRegra.BackgroundColor = Color.FromHex(paramUnidadeRastreada.CorRegraPrioritaria);

                }
                boxPrincipal.Children.Add(boxRegra, 0, 0);
                #endregion

                #region Texto
                StackLayout boxTexto = new StackLayout()
                {
                    Margin = new Thickness(margin),
                    Spacing = 0,
                    Orientation = StackOrientation.Vertical,
                    WidthRequest = col02.Width.Value,
                    VerticalOptions = LayoutOptions.Center
                };


                if (paramUnidadeRastreada.OrdemRastreador == null)
                    paramUnidadeRastreada.OrdemRastreador = 0;


                Label labelUnidadeRastreada = new Label()
                {
                    Text = paramUnidadeRastreada.IdentificacaoUnidadeRastreada,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 2),
                    LineBreakMode = LineBreakMode.TailTruncation,
                    HorizontalTextAlignment = TextAlignment.Start
                };


                if (paramUnidadeRastreada.StatusRastreadorUnidadeRastreada == (byte)EnumStatusUnidadeRastreada.Atualizada)
                {
                    labelUnidadeRastreada.TextColor = Color.DarkGreen;
                }
                else
                {
                    labelUnidadeRastreada.TextColor = Color.DarkRed;
                }


                boxTexto.Children.Add(labelUnidadeRastreada);

                Label labelDataEvento = new Label()
                {
                    Text = String.Format(
                        "{0:dd/MM/yyyy HH:mm:ss}"
                        , paramUnidadeRastreada.DataEvento.Value.ToLocalTime()
                    ),
                    TextColor = Color.FromHex("#9b9eb0"),
                    FontSize = 14,
                    Margin = labelUnidadeRastreada.Margin,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                boxTexto.Children.Add(labelDataEvento);


                if (!String.IsNullOrWhiteSpace(paramUnidadeRastreada.ResponsavelUnidadeRastreada))
                {
                    Label labelMotorista = new Label()
                    {
                        Text = paramUnidadeRastreada.ResponsavelUnidadeRastreada,
                        TextColor = Color.FromHex("#9b9eb0"),
                        FontSize = 14,
                        Margin = new Thickness(0),
                        LineBreakMode = LineBreakMode.TailTruncation,
                        HorizontalTextAlignment = TextAlignment.Start
                    };
                    boxTexto.Children.Add(labelMotorista);
                }


                StackLayout boxImagemUnidade = new StackLayout()
                {
                    Margin = new Thickness(15, 0, 0, 0),
                    Spacing = 0,
                    Orientation = StackOrientation.Horizontal,
                    WidthRequest = col01.Width.Value,
                    VerticalOptions = LayoutOptions.Start
                };


                String sourceImageUnidade = String.Empty;

                if (!String.IsNullOrEmpty(paramUnidadeRastreada.IconePadrao))
                    sourceImageUnidade = paramUnidadeRastreada.IconePadrao;

                Image ImageUnidade = new Image()
                {
                    Source = sourceImageUnidade,
                    //BackgroundColor = Color.Black,
                    //HeightRequest = 15,
                    //WidthRequest = 16,
                    Margin = new Thickness(35, 0, margin, 0),
                    Opacity = 1,
                    VerticalOptions = LayoutOptions.Center,
                    Scale = 3.5
                };


                boxPrincipal.Children.Add(ImageUnidade, 0, 0);

                boxPrincipal.Children.Add(boxTexto, 1, 0);
                #endregion

                #region Image

                String sourceImage = String.Empty;


                //Muda icone de ignicao
                if (paramUnidadeRastreada.Ignicao == true)
                    sourceImage = "ic_ignition_on.png";

                if (paramUnidadeRastreada.Ignicao == false)
                    sourceImage = "ic_ignition_off.png";


                Image ImageIgnicao = new Image()
                {
                    Source = sourceImage,
                    HeightRequest = 15,
                    WidthRequest = 16,
                    Margin = new Thickness(0, 0, margin, 0),
                    Opacity = 1,
                    VerticalOptions = LayoutOptions.Center
                };

                if (paramUnidadeRastreada.Ignicao.HasValue)
                {
                    if (paramUnidadeRastreada.Ignicao.Value == false)
                    {
                        ImageIgnicao.Opacity = 0.5f;
                    }
                }
                else
                {
                    ImageIgnicao.Opacity = 0;
                }

                StackLayout stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                stack.HorizontalOptions = LayoutOptions.End;
                stack.Margin = new Thickness(0, 25, 0, 0);

                stack.Children.Add(ImageIgnicao);
                if (paramView != null)
                {
                    stack.Children.Add(paramView);
                }

                boxPrincipal.Children.Add(stack, 1, 0);

                #endregion

                return boxPrincipal;

            }
            catch
            {

                throw;
            }
        }

        private async void BoxRegra_Tap(object obj)
        {
            if (obj != null)
            {
                PosicaoHistorico tempPosicao = obj as PosicaoHistorico;
                await this._messageService.ShowAlertAsync(
                    tempPosicao.NomeRegraViolada
                    , tempPosicao.IdentificacaoUnidadeRastreada
                );
            }

        }

    }
}
