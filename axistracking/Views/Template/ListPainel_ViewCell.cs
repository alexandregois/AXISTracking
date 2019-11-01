using System;
using System.ComponentModel;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
    public class ListPainel_ViewCell : ViewCellBase
    {

        private ViewModelPainel _viewModelPainel;
        private readonly IMessageService _messageService;
        private Grid _gridTemp;

        protected App _app => (Application.Current as App);

        Double? alturaFinal;

        Label total;
        Button detalhes;
        Button cleanDetalhes;
        Image imageSetaDetalhes;
        Label chave;
        StackLayout GraficoDadosBox;
        StackLayout labelBox;

        public ListPainel_ViewCell(
            ViewModelPainel paramContext
        )
            : base(Color.White)
        {
            _viewModelPainel = paramContext;
            this._messageService =
                    DependencyService.Get<IMessageService>();
            MakeGrid();
        }

        private void MakeGrid()
        {
            _gridTemp = new Grid()
            {
                Margin = new Thickness(10, 10),
                Padding = 0,
                RowSpacing = 0,
                ColumnSpacing = 5,
                //BackgroundColor = Color.Black
            };

            _gridTemp.ColumnDefinitions = new ColumnDefinitionCollection();
            _gridTemp.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });

            _gridTemp.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Star
            });

            _gridTemp.RowDefinitions = new RowDefinitionCollection();
            _gridTemp.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            _gridTemp.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            #region  Linha 1
            total = new Label()
            {
                FontSize = 35,
                TextColor = Color.FromHex("#4b4e5e"),
                VerticalTextAlignment = TextAlignment.End,
                Margin = new Thickness(0, 0, 0, 0),
                VerticalOptions = LayoutOptions.End
                //,WidthRequest = 75
            };


            if (_app.ScreenHeight > 600)
            {
                total.FontSize = 39;
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    total.Margin = new Thickness(0, 3, 0, 0);
                }
            }


            _gridTemp.Children.Add(total, 0, 0);

            labelBox = new StackLayout()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Spacing = 0,
                Padding = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            StackLayout boxDetalhes = new StackLayout();
            boxDetalhes.Orientation = StackOrientation.Horizontal;
            boxDetalhes.HorizontalOptions = LayoutOptions.End;


            imageSetaDetalhes = new Image()
            {
                Source = "seta_direita.png",
                HeightRequest = 15,
                Margin = new Thickness(0, 0, 0, 0)

            };

            imageSetaDetalhes.GestureRecognizers.Add(new TapGestureRecognizer());

            imageSetaDetalhes.SetBinding(
                TapGestureRecognizer.CommandParameterProperty
                , new Binding(
                    "ViewDetalhesCommand"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , _viewModelPainel
                )
            );

            cleanDetalhes = new Button()
            {
                FontSize = 12,
                TextColor = Color.FromHex("#4482ff"),
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 25,
                WidthRequest = 25,
                BorderWidth = 0,
                BorderRadius = 0,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(9, 0, 0, 0),
                BorderColor = Color.Transparent
            };

            detalhes = new Button()
            {
                Text = AppResources.VerDetalhes,
                FontSize = 12,
                TextColor = Color.FromHex("#4482ff"),
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 25,
                WidthRequest = 120,
                BorderWidth = 0,
                BorderRadius = 0,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(15, 0, 0, 0),
                BorderColor = Color.Transparent
            };

            detalhes.SetBinding(
                Button.CommandProperty
                , new Binding(
                    "ViewDetalhesCommand"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , _viewModelPainel
                )
            );

            boxDetalhes.Children.Add(cleanDetalhes);

            boxDetalhes.Children.Add(detalhes);
            boxDetalhes.Children.Add(imageSetaDetalhes);

            labelBox.Children.Add(boxDetalhes);

            chave = new Label()
            {
                FontSize = 12,
                TextColor = Color.FromHex("#9b9eb0"),
                VerticalTextAlignment = TextAlignment.End,
                Margin = new Thickness(0, 3, 0, 0)
            };

            if (_app.ScreenHeight < 600)
                chave.Margin = new Thickness(0, 5, 0, 0);


            labelBox.Children.Add(chave);

            _gridTemp.Children.Add(labelBox, 1, 0);
            #endregion

            #region  Linha 2
            GraficoDadosBox = new StackLayout()
            {
                WidthRequest = _viewModelPainel.DefaultWidth -
                (
                    _gridTemp.Margin.Left
                    + _gridTemp.Margin.Right
                ),
                Orientation = StackOrientation.Horizontal,
                Margin = 0,
                Spacing = 0,
                Padding = 0
            };

            _gridTemp.Children.Add(GraficoDadosBox, 0, 1);

            Grid.SetColumnSpan(GraficoDadosBox, 2);
            #endregion

            _gridTemp.PropertyChanging += _gridTemp_PropertyChanging;

            View = _gridTemp;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            PainelDto painel = (PainelDto)BindingContext;

            if (painel != null)
            {
                total.Text = painel.Total.ToString();

                imageSetaDetalhes.IsVisible = painel.HasDetalhes;
                detalhes.IsVisible = painel.HasDetalhes;
                detalhes.CommandParameter = painel;

                if (!painel.HasDetalhes)
                    cleanDetalhes.IsVisible = true;
                else
                    cleanDetalhes.IsVisible = false;


                chave.Text = AppResources.ResourceManager.GetString(painel.Chave).ToUpper();

                if (painel.Chave == "UnidadesRastreadas")
                {
                    chave.Text = AppResources.RastreadoresDesatualizados.ToUpper();
                    Int32 intTotal = Convert.ToInt32(((double)painel.Total / 100) * painel.Grafico[0].porcento);
                    total.Text = intTotal.ToString();
                }

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    if (painel.Chave == "Alertas")
                    {
                        chave.Margin = new Thickness(-9, 0, 0, 0);
                    }
                }


                if (painel.Grafico != null && painel.Grafico.Count > 0)
                {
                    if (painel.Total > 0)
                    {
                        //int pos = 0;

                        foreach (GraficoDto objItem in painel.Grafico)
                        {
                            StackLayout stack = new StackLayout()
                            {
                                BackgroundColor = Color.FromHex(objItem.corBarra),
                                Padding = 0,
                                Margin = new Thickness(0, 10, 0, 0),
                                Spacing = 0
                            };

                            StackLayout stackVazio = new StackLayout()
                            {
                                BackgroundColor = Color.Transparent,
                                Padding = 0,
                                Margin = new Thickness(0, 15, 0, 0),
                                Spacing = 0
                            };

                            Label labelPercentual = new Label()
                            {
                                FontSize = 17,
                                TextColor = Color.White,
                                VerticalTextAlignment = TextAlignment.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Text = objItem.porcento.ToString("N0") + "%", //objItem.porcento.ToString() + "%", //objItem.porcento.ToString("N1") + "%",
                                Margin = new Thickness(0, 5),
                                LineBreakMode = LineBreakMode.TailTruncation
                            };

                            Double largura = 0;

                            if (objItem.porcento > 0)
                            {
                                largura = (
                                    (
                                        objItem.porcento
                                        * GraficoDadosBox.WidthRequest
                                    ) / 100
                                );
                            }

                            stack.WidthRequest = largura;

                            if (objItem.porcento < 25)
                                labelPercentual.TextColor = stack.BackgroundColor;

                            //if(Device.RuntimePlatform == Device.iOS)
                            stack.PropertyChanged += LabelPercentual_PropertyChanged;
                            stack.Children.Add(labelPercentual);


                            //Valor do grafico vem da procedure
                            //Correcao rápida
                            if (painel.Chave != "Alertas")
                            {
                                GraficoDadosBox.Children.Add(stack);
                            }
                            else
                            {
                                GraficoDadosBox.Children.Add(stackVazio);
                            }


                        }

                        GraficoDadosBox.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(GraficoDadosBox_Tap),
                            CommandParameter = painel,
                            NumberOfTapsRequired = 1
                        });

                    }
                    else
                    {
                        GraficoDadosBox.IsVisible = false;

                        if (Device.RuntimePlatform == Device.iOS)
                            total.PropertyChanged += Total_PropertyChanged;
                    }
                }
                else
                {
                    GraficoDadosBox.IsVisible = false;

                    if (Device.RuntimePlatform == Device.iOS)
                        total.PropertyChanged += Total_PropertyChanged;
                }


            }

        }

        private void GraficoDadosBox_Tap(object obj)
        {
            PainelDto painel = (PainelDto)obj;

            String message = Environment.NewLine;
            foreach (GraficoDto objItem in painel.Grafico)
            {
                message += String.Format(
                    "{0}: {1}% {2}"
                    , AppResources.ResourceManager.GetString(objItem.Identificacao)
                    , String.Format("{0:n0}", objItem.porcento)
                    //, String.Format("{0:n2}", objItem.porcento) //objItem.porcento
                    , Environment.NewLine
                );
            }

            _messageService.ShowAlertAsync(
                message
                , AppResources.ResourceManager.GetString(painel.Chave)
            );

        }

        void LabelPercentual_PropertyChanged(
            object sender
            , PropertyChangedEventArgs e
        )
        {
            if (e.PropertyName == "Height")
            {
                StackLayout temp = (StackLayout)sender;

                Double alturaLabel = temp.Height;

                if (alturaLabel < 20)
                {
                    alturaLabel = 20;
                    temp.HeightRequest = alturaLabel;
                }

                //StackLayout pai = ((StackLayout)temp.Parent);

                double altura =
                    alturaLabel + temp.Margin.VerticalThickness;

                double alturaLinha1 = total.Height;
                if (alturaLinha1 < labelBox.Height)
                    alturaLinha1 = labelBox.Height;

                altura += alturaLinha1;

                altura += _gridTemp.RowSpacing;

                altura += _gridTemp.Margin.Top;

                altura += _gridTemp.Margin.Bottom;

                _gridTemp.HeightRequest = altura;
                alturaFinal = altura;

                temp.PropertyChanged -= LabelPercentual_PropertyChanged;

                //if(Device.RuntimePlatform == Device.iOS)
                //temp.HeightRequest = alturaLabel;

            }
        }

        void Total_PropertyChanged(
            object sender
            , PropertyChangedEventArgs e
        )
        {
            if (e.PropertyName == "Height")
            {
                Label temp = (Label)sender;

                double altura = 0;

                double alturaLinha1 = total.Height;
                if (alturaLinha1 < labelBox.Height)
                    alturaLinha1 = labelBox.Height;

                altura += alturaLinha1;

                altura += _gridTemp.RowSpacing;

                altura += _gridTemp.Margin.Top;

                altura += _gridTemp.Margin.Bottom;

                _gridTemp.HeightRequest = altura;
                alturaFinal = altura;

                temp.PropertyChanged -= Total_PropertyChanged;

            }
        }

        void _gridTemp_PropertyChanging(
            object sender
            , PropertyChangingEventArgs e
        )
        {
            if (e.PropertyName == "Height")
            {
                Grid temp = (Grid)sender;
                if (alturaFinal.HasValue)
                    temp.HeightRequest = alturaFinal.Value;
            }
        }
    }
}
