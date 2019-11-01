using System;
using System.Linq;
using System.Windows.Input;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Resx;
using axistracking.ViewModels.Base;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ListPagePadrao : AbsoluteLayout
    {
        private ViewModelBaseListPage _context { get; set; }

        public StackLayout PageContent { get; set; }

        public AbsoluteLayout PainelFiltro { get; set; }

        public Grid ListViewPanelTop { get; set; }

        private Color _colorPanelDesativado { get; set; }

        public ListView ListComandos { get; set; }

        private Double _alturaInicialLabelTotal { get; set; }

        private Int32 _totalGrafico { get; set; }

        private Int32 _totalAltura { get; set; }

        private Int32 _totalLinhas { get; set; }

        private PainelDto painel { get; set; }

        public ListPagePadrao(ViewModelBaseListPage paramBindingContext)
        {
            _context = paramBindingContext;
            this.BindingContext = paramBindingContext;

            _colorPanelDesativado = Color.FromHex("#FF2E2F3A");

            Margin = 0;
            Padding = 0;
            BackgroundColor = Color.Gray;

            SetBinding(
                VisualElement.HeightRequestProperty
                , new Binding(
                    "DefaultHeightContent"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            PageContent = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Margin = 0,
                Padding = 0,
                Spacing = 0
            };

            PageContent.SetBinding(
                VisualElement.HeightRequestProperty
                , new Binding(
                    "DefaultHeightContent"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            PageContent.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            CreateFiltro();
            CreateListView();

            Children.Add(PageContent);
        }

        #region Filtro
        private void CreateFiltro()
        {
            PainelFiltro = new AbsoluteLayout()
            {
                BackgroundColor = Color.FromHex("#2E2F3A"),
                Margin = 0,
                Padding = 0
            };

            PainelFiltro.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            PainelFiltro.SetBinding(
                VisualElement.IsVisibleProperty
                , new Binding(
                    "ListPainelTop_IsVisible"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            Grid filtroGrid = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Margin = 0,
                Padding = 0
            };

            filtroGrid.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            filtroGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Star
            });

            filtroGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            filtroGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Star
            });

            #region Filtro Texto
            Grid filtroTexto = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Margin = 10,
                Padding = 0,
                BackgroundColor = Color.White
            };

            filtroTexto.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Star
            });

            filtroTexto.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = 30
            });

            CustomEntry TxtBuscar = new CustomEntry()
            {
                Margin = new Thickness(5, 0),
                BackgroundColor = Color.Transparent,
                BorderStyle = "Hide"
            };

            TxtBuscar.SetBinding(
                Entry.TextProperty
                , new Binding(
                    "TxtBuscar"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );
            filtroTexto.Children.Add(TxtBuscar, 0, 0);

            Button imageBuscar = new Button()
            {
                Image = "ic_busca.png",
                BackgroundColor = Color.Transparent,
                Margin = 0
            };

            imageBuscar.SetBinding(
                Button.CommandProperty
                , new Binding(
                    "BuscarCommand"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );
            filtroTexto.Children.Add(imageBuscar, 1, 0);

            StackLayout panelFiltroText = new StackLayout()
            {
                BackgroundColor = Color.FromHex("#d2d8e2"),
                Margin = 0,
                Padding = 0,
                Spacing = 0,
            };

            panelFiltroText.SetBinding(
                VisualElement.HeightRequestProperty
                , new Binding(
                    "PainelFiltroTextHeight"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );
            panelFiltroText.Children.Add(filtroTexto);

            filtroGrid.Children.Add(panelFiltroText, 0, 0);
            #endregion

            #region Filtro Status
            ListViewPanelTop = new Grid()
            {
                BackgroundColor = Color.FromHex("#2E2F3A"),
                RowSpacing = 0,
                ColumnSpacing = 0,
                Margin = 0
            };

            ListViewPanelTop.BindingContextChanged += Handle_BindingContextChanged;

            #region ListViewPanelTop Binding
            ListViewPanelTop.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListViewPanelTop.SetBinding(
                VisualElement.HeightRequestProperty
                , new Binding(
                    "ListPainelTop_Height"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListViewPanelTop.SetBinding(
                BindableObject.BindingContextProperty
                , new Binding(
                    "ListPainelTop_Source"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListViewPanelTop.SetBinding(
                VisualElement.IsEnabledProperty
                , new Binding(
                    "ListEndRefresh"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );
            #endregion


            filtroGrid.Children.Add(ListViewPanelTop, 0, 1);
            #endregion

            PainelFiltro.Children.Add(filtroGrid);
            PageContent.Children.Add(PainelFiltro);
        }

        private void Handle_BindingContextChanged(
            object sender
            , System.EventArgs e
        )
        {
            Grid gridTemp = ((Grid)sender);
            painel = (PainelDto)gridTemp.BindingContext;

            if (painel != null)
            {
                gridTemp.Children.Clear();

                Int32 linha = 0;
                Int32 coluna = 0;

                Double barraPercentualHeight = 3;
                Thickness barraPercentualMargin = new Thickness(0, 5, 0, 0);
                Thickness boxContentMargin = 10;


                if (painel.Grafico != null)
                {

                    _alturaInicialLabelTotal = ((ViewModelBaseListPage)this.BindingContext).ListPainelTop_HeightPadrao - (
                   barraPercentualHeight
                   + barraPercentualMargin.Top
                   + barraPercentualMargin.Bottom
                   + boxContentMargin.Top
                   + boxContentMargin.Bottom);


                    _totalGrafico = painel.Grafico.Count();

                    foreach (GraficoDto objItem in painel.Grafico)
                    {

                        if (coluna == 2)
                        {
                            coluna = 0;
                            linha = 1;
                        }

                        Frame frame;
                        StackLayout boxFundo;
                        FramePadrao(out frame, out boxFundo, objItem);

                        StackLayout boxContent = new StackLayout()
                        {
                            Margin = boxContentMargin,
                            Spacing = 0
                        };

                        StackLayout barraPercentual = new StackLayout()
                        {
                            BackgroundColor = Color.FromHex("#4b4e5e"),
                            HeightRequest = barraPercentualHeight,
                            WidthRequest = frame.WidthRequest - (
                                boxContentMargin.Right
                                + boxContentMargin.Left
                            ),
                            Orientation = StackOrientation.Horizontal,
                            Margin = barraPercentualMargin,
                            Spacing = 0
                        };

                        Double largura = 0;

                        if (objItem.porcento > 0)
                        {
                            largura = ((objItem.porcento * barraPercentual.WidthRequest)
                                       / 100);
                        }

                        StackLayout barraCor = new StackLayout()
                        {
                            BackgroundColor = Color.FromHex(objItem.corBarra),
                            WidthRequest = largura,
                            Margin = new Thickness(0),
                            Spacing = 0
                        };

                        barraPercentual.Children.Add(barraCor);

                        Label total = new Label()
                        {
                            FontSize = 13,
                            TextColor = Color.White,
                            Margin = 0,
                            VerticalTextAlignment = TextAlignment.Start
                        };

                        if (objItem.porcento != 0)
                        {

                            total.Text =
                                     String.Format(
                                         "{0}% {1}"
                                         , objItem.porcento.ToString()
                                         , AppResources.ResourceManager.GetString(objItem.Identificacao)
                                        );
                        }
                        else
                        {

                            total.Text =
                                     String.Format(
                                         "{0}% {1}"
                                         , "0,00"
                                         , AppResources.ResourceManager.GetString(objItem.Identificacao)
                                        );
                        }


                        total.PropertyChanged += Total_PropertyChanged;
                        boxContent.Children.Add(total);

                        boxContent.Children.Add(barraPercentual);

                        boxFundo.Children.Add(boxContent);

                        boxFundo.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(BoxFundo_Tap),
                            CommandParameter = boxFundo,
                            NumberOfTapsRequired = 1
                        });

                        frame.Content = boxFundo;
                        gridTemp.Children.Add(frame, coluna, linha);
                        coluna++;

                    }


                }
                else
                {
                    _totalGrafico = 0;
                }



                if ((_totalGrafico % 2) == 1)
                {
                    Frame frame;
                    StackLayout paramBox;
                    FramePadrao(out frame, out paramBox, null);
                    frame.Content = paramBox;

                    gridTemp.Children.Add(frame, coluna, linha);
                }

            }
        }

        private void FramePadrao(
            out Frame paramFrame
            , out StackLayout paramStackLayout
            , GraficoDto paramGrafico
        )
        {

            paramFrame = new Frame()
            {
                OutlineColor = Color.FromHex("#373845"),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent,
                CornerRadius = 0,
                Margin = 0,
                HasShadow = false,
                Padding = 1,
                Opacity = 1,
                IsClippedToBounds = true,
                HeightRequest = ((ViewModelBaseListPage)this.BindingContext).ListPainelTop_HeightPadrao,
                WidthRequest = ((ViewModelBaseListPage)this.BindingContext).ListPainelTopColumn_Width
            };

            paramStackLayout = new StackLayout()
            {
                Margin = 0,
                Spacing = 0,
                BackgroundColor = _colorPanelDesativado,
                BindingContext = paramGrafico,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

        }

        private void BoxFundo_Tap(object e)
        {
            try
            {

                StackLayout boxFundo = (StackLayout)e;
                GraficoDto graficoDto = (GraficoDto)boxFundo.BindingContext;
                ViewModelBaseListPage detalhes = (ViewModelBaseListPage)this.BindingContext;

                if (boxFundo.BackgroundColor == _colorPanelDesativado)
                {
                    DesativarPanel();

                    boxFundo.BackgroundColor = Color.FromHex(graficoDto.corFundo);

                    detalhes.StatusFiltro = graficoDto.Status;

                    detalhes.Buscar();

                }
                else
                {
                    DesativarPanel();

                    detalhes.StatusFiltro = null;

                    detalhes.Buscar();

                }

            }
            catch (Exception)
            {

            }
        }

        private void DesativarPanel()
        {
            foreach (Frame item in ListViewPanelTop.Children)
            {
                item.Content.BackgroundColor = _colorPanelDesativado;
            }
        }

        void Total_PropertyChanged(
            object sender
            , System.ComponentModel.PropertyChangedEventArgs e
        )
        {
            if (e.PropertyName == "Height")
            {
                Label tempLabel = (Label)sender;
                if (tempLabel.Height > _alturaInicialLabelTotal)
                {
                    _alturaInicialLabelTotal = tempLabel.Height;
                }

                tempLabel.PropertyChanged -= Total_PropertyChanged;
                _totalAltura++;
                if (_totalGrafico == _totalAltura)
                    MudaAltura(_alturaInicialLabelTotal);
            }
        }

        private void MudaAltura(
            Double paramNovaAltura
        )
        {
            Double frameNovaAltura = 0;
            foreach (View item in ListViewPanelTop.Children)
            {
                frameNovaAltura = paramNovaAltura;

                StackLayout boxFundo = (StackLayout)((Frame)item).Content;

                StackLayout boxContent = (StackLayout)boxFundo.Children[0];

                foreach (View item2 in boxContent.Children)
                {
                    if (item2.GetType() == typeof(Label))
                    {
                        item2.HeightRequest = paramNovaAltura;
                    }
                    else
                    {
                        frameNovaAltura += item2.HeightRequest;
                        frameNovaAltura += item2.Margin.Bottom;
                        frameNovaAltura += item2.Margin.Top;
                    }
                }

                frameNovaAltura += boxContent.Margin.Top;
                frameNovaAltura += boxContent.Margin.Bottom;

                item.HeightRequest = frameNovaAltura;

            }

            ViewModelBaseListPage temp = ((ViewModelBaseListPage)this.BindingContext);

            Double tempCount = temp.ListPainelTop_Source.Grafico.Count() / (double)2;

            int linha = 1;

            if (tempCount > (double)1)
            {
                linha = ((int)tempCount) + 1;
            }

            temp.ListPainelTop_Height = linha * frameNovaAltura;

        }

        #endregion

        #region ListView
        private void CreateListView()
        {
            ListComandos = new ListView()
            {
                SeparatorColor = Color.FromHex("#d2d8e2"),
                SeparatorVisibility = SeparatorVisibility.Default,
                Margin = 0,
                HasUnevenRows = true,
                RowHeight = -1,
                BackgroundColor = Color.White
            };


            #region Binding
            ListComandos.SetBinding(
                VisualElement.WidthRequestProperty
                , new Binding(
                    "DefaultWidth"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                VisualElement.HeightRequestProperty
                , new Binding(
                    "List_Height"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                ListView.RefreshCommandProperty
                , new Binding(
                    "List_RefreshCommand"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                VisualElement.IsEnabledProperty
                , new Binding(
                    "ListEndRefresh"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                ListView.IsRefreshingProperty
                , new Binding(
                    "List_IsRefreshing"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                ListView.ItemsSourceProperty
                , new Binding(
                    "List_Source"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            ListComandos.SetBinding(
                VisualElement.IsVisibleProperty
                , new Binding(
                    "List_IsVisible"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );
            #endregion

            PageContent.Children.Add(ListComandos);
        }
        #endregion

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}