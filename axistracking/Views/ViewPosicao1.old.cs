using System;
using System.Collections.Generic;
using axistracking.CustomElements;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;
using axistracking.ViewModels;
using axistracking.Views.Interface;
using axistracking.Views.Template;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using axistracking.ViewModels.Services;
using System.Threading.Tasks;
using System.Threading;

namespace axistracking.Views
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public partial class ViewPosicao1 : ContentPage, IViewPosicao
	{
		private Thickness marginListRow { get; set; }
		private CustomDialogAlert _painelTopLoad { get; set; }
		private ViewModelPosicao _viewModelPosicao { get; set; }
		public Map mapaPosicao { get; set; }

		private Button _mapa { get; set; }
		private Button _mapaType { get; set; }

		private EnumPage _pageAnterior { get; set; }

		private Double marginStacklayoutLeftRight = 10;
		private Double marginStacklayoutTopBottom = 5;

		private Double marginStacklayoutTopBottomAnother = 0;

		private PosicaoHistorico _posicaoHistorico { get; set; }

		private readonly IMessageService _messageService;
		protected App _app => (Application.Current as App);

		private CancellationTokenSource _tokensource { get; set; }

		public ViewPosicao1(PainelDto Painel, object obj, EnumPage Page)
		{
			InitializeComponent();

			PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

			_posicaoHistorico = (PosicaoHistorico)obj;

			_pageAnterior = Page;

			marginListRow = new Thickness(
				marginStacklayoutLeftRight
				, marginStacklayoutTopBottom
			);


			mapaPosicao = new Map();

			_viewModelPosicao = new ViewModelPosicao(
				Painel
				, obj
				, EnumPage.Posicao
				, Page
			);

			_viewModelPosicao._view = this as IViewPosicao;
			this.BindingContext = _viewModelPosicao;

			this._messageService =
					DependencyService.Get<IMessageService>();

			MontaLoad();

			ExibeTitulo();

		}

		protected override void OnAppearing()
		{

			if (mapaPosicao == null)
			{
				mapaPosicao = new Map();
				PainelMapa.Children.Add(mapaPosicao);
			}

			PainelDetalhes.Children.Clear();

			var lifecycleHandler = (ViewModelPosicao)this.BindingContext;
			lifecycleHandler.OnAppearing();


		}

		protected override void OnDisappearing()
		{
			var lifecycleHandler = (ViewModelPosicao)this.BindingContext;
			lifecycleHandler.OnDisappearing();
		}

		private void MontaLoad()
		{
			_painelTopLoad = new CustomDialogAlert(
				PageContent
				, Color.White.MultiplyAlpha(0.8)
				, false
			);
			ActivityIndicator activity = _painelTopLoad.RequireActivityIndicator();
			activity.Color = Color.Gray;
			_painelTopLoad.ShowAlert(activity);

		}

		public void ListDefault_BindingContextChanged(
			object sender
			, System.EventArgs e
		)
		{
			StackLayout gridTemp = ((StackLayout)sender);
			Dictionary<String, Boolean> dic = (Dictionary<String, Boolean>)gridTemp.BindingContext;
			gridTemp.Children.Clear();
			if (dic != null)
			{
				Int32 count = dic.Count;
				if (count == 0)
				{
					SemInfo(gridTemp);
				}
				else
				{
					Grid boxGrid = MontaGridTopo();

					Int32 posRow = 0;

					foreach (KeyValuePair<String, Boolean> item in dic)
					{
						Image image;
						Label label;

						PadraoLinhaDetalhePosicao(out image, out label);

						if (item.Value)
						{
							image.Source = "ic_on.png";
						}
						else
						{
							image.Source = "ic_off.png";
						}

						label.Text = item.Key;
						label.FontSize = 11;
						label.VerticalTextAlignment = TextAlignment.Center;

						boxGrid.Children.Add(image, 0, posRow);
						boxGrid.Children.Add(label, 1, posRow);

						posRow++;
					}

					gridTemp.Children.Add(boxGrid);
				}
			}
			else
			{
				SemInfo(gridTemp);
			}

		}

		//Painel Telemetria//
		public void PainelTelemetriaListView_BindingContextChanged(
			object sender
			, System.EventArgs e
		)
		{
			StackLayout gridTemp = ((StackLayout)sender);
			Dictionary<String, Double> dic = (Dictionary<String, Double>)gridTemp.BindingContext;
			gridTemp.Children.Clear();
			if (dic != null)
			{
				if (dic.Count == 0)
				{
					SemInfo(gridTemp);
				}
				else
				{
					Grid boxGrid = MontaGridPadraoTexto();

					Int32 posRow = 0;
					Int32 posRowTopo = 0;


					foreach (KeyValuePair<String, Double> item in dic)
					{

						Label dataItem;
						Label tituloDataItem;

						PadraoLinhaDetalhePosicaoTextoInformacoes(out tituloDataItem, out dataItem);

						tituloDataItem.Text = item.Key;
						dataItem.Text = item.Value.ToString("N2", Resx.AppResources.Culture);

						boxGrid.Children.Add(tituloDataItem, 0, posRow);
						boxGrid.Children.Add(dataItem, 1, posRow);
						posRow++;

						gridTemp.Children.Add(boxGrid);
					}
				}
			}
			else
			{
				SemInfo(gridTemp);
			}

		}

		public void PainelResumoBindingContextChanged(
			object sender
			, System.EventArgs e
		)
		{

			StackLayout gridTemp = ((StackLayout)sender);
			Posicao _posicao = (Posicao)gridTemp.BindingContext;
			gridTemp.Children.Clear();

			if (_posicao != null)
			{

				Grid boxGridTopo = MontaGridTopo();
				Grid boxGrid = MontaGridPadrao();

				Int32 posRow = 0;
				Int32 posRowTopo = 0;

				#region GrupoModelo

				Image image;
				Label grupo;

				PadraoLinhaDetalhePosicao(out image, out grupo);

				grupo.FontAttributes = FontAttributes.Bold;
				grupo.Margin = new Thickness(
					0
					, marginStacklayoutTopBottom
					, 0
					, marginStacklayoutTopBottom
				);

				if (String.IsNullOrWhiteSpace(_posicao.GrupoUnidadeRastreada))
				{
					grupo.Text = AppResources.SemGrupo;
				}
				else
				{
					grupo.Text = _posicao.GrupoUnidadeRastreada;
				}

				Label descricao = new Label()
				{
					Text = "",
					FontSize = 15,
					VerticalTextAlignment = TextAlignment.Start,
					Margin = new Thickness(
						0
						, marginStacklayoutTopBottom
						, marginStacklayoutLeftRight
						, marginStacklayoutTopBottom
					),
					HorizontalTextAlignment = TextAlignment.Start
				};

				if (String.IsNullOrWhiteSpace(_posicao.ModeloVeiculo))
				{
					descricao.Text = AppResources.SemModelo;
				}
				else
				{
					descricao.Text = "(" + _posicao.ModeloVeiculo + ")";
				}

				StackLayout boxGrupoModelo = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					VerticalOptions = LayoutOptions.Center,
					Margin = marginListRow
				};

				image.Source = "ic_veiculo.png";

				boxGrupoModelo.Children.Add(grupo);
				boxGrupoModelo.Children.Add(descricao);

				boxGridTopo.Children.Add(image, 0, posRowTopo);
				boxGridTopo.Children.Add(boxGrupoModelo, 1, posRowTopo);

				posRowTopo++;
				#endregion

				#region Endereco
				if (!String.IsNullOrWhiteSpace(_posicao.Endereco))
				{
					Image imageEndereco;

					Label endereco;

					PadraoLinhaDetalhePosicao(out imageEndereco, out endereco);

					imageEndereco.Source = "ic_localizacao.png";

					endereco.Text = _posicao.Endereco;

					boxGridTopo.Children.Add(imageEndereco, 0, posRowTopo);
					boxGridTopo.Children.Add(endereco, 1, posRowTopo);
					posRowTopo++;
				}
				#endregion

				#region DataTransmissao/Atualizacao
				if (_posicao.DataAtualizacao.HasValue)
				{
					Image imageDataTransmissao;

					Label dataTransmissao;
					Label tituloDataTransmissao;

					PadraoLinhaDetalhePosicaoTexto(out tituloDataTransmissao, out dataTransmissao);

					tituloDataTransmissao.Text = AppResources.DataAtualizacao;

					//imageDataTransmissao.Source = "ic_data_transmissao.png";
					dataTransmissao.Text = FormatarData(_posicao.DataAtualizacao.Value);

					//boxGrid.Children.Add(imageDataTransmissao, 0, posRow);
					boxGrid.Children.Add(tituloDataTransmissao, 0, posRow);
					boxGrid.Children.Add(dataTransmissao, 1, posRow);
					posRow++;
				}
				#endregion

				#region DataGPS
				if (_posicao.DataGPS.HasValue)
				{
					Image imageDataGPS;

					Label dataGPS;
					Label tituloDataGPS;

					PadraoLinhaDetalhePosicaoTexto(out tituloDataGPS, out dataGPS);

					tituloDataGPS.Text = AppResources.DataGps;
					dataGPS.Text = FormatarData(_posicao.DataGPS.Value);

					boxGrid.Children.Add(tituloDataGPS, 0, posRow);
					boxGrid.Children.Add(dataGPS, 1, posRow);
					posRow++;
				}
				#endregion

				#region DataEvento               
				Image imageDataEvento;

				Label dataEvento;
				Label tituloDataEvento;

				PadraoLinhaDetalhePosicaoTexto(out tituloDataEvento, out dataEvento);

				tituloDataEvento.Text = AppResources.DataEvento;

				dataEvento.Text = FormatarData(_posicao.DataEvento);

				boxGrid.Children.Add(tituloDataEvento, 0, posRow);
				boxGrid.Children.Add(dataEvento, 1, posRow);
				posRow++;
				#endregion

				#region Velocidade
				if (_posicao.Velocidade.HasValue)
				{
					Image imageVelocidade;

					Label velocidade;
					Label tituloVelocidade;

					PadraoLinhaDetalhePosicaoTexto(out tituloVelocidade, out velocidade);

					tituloVelocidade.Text = AppResources.Velocidade;

					velocidade.Text = String.Format(
							"{0} Km/h"
						, _posicao.Velocidade.Value.ToString(
							"N2"
							, Resx.AppResources.Culture
						)
						);

					boxGrid.Children.Add(tituloVelocidade, 0, posRow);
					boxGrid.Children.Add(velocidade, 1, posRow);
					posRow++;
				}
				#endregion

				#region Odometro
				if (_posicao.Odometro.HasValue)
				{
					Image imageOdometro;

					Label odometro;
					Label tituloOdometro;

					PadraoLinhaDetalhePosicaoTexto(out tituloOdometro, out odometro);

					tituloOdometro.Text = AppResources.Odometro;

					odometro.Text = String.Format(
						"{0} Km"
						, _posicao.Odometro.Value.ToString(
							"N2"
							, Resx.AppResources.Culture
						)
					);

					boxGrid.Children.Add(tituloOdometro, 0, posRow);
					boxGrid.Children.Add(odometro, 1, posRow);
					posRow++;
				}
				#endregion

				#region Horimetro
				if (_posicao.Horimetro.HasValue)
				{

					Label horimetro;
					Label tituloHorimetro;

					PadraoLinhaDetalhePosicaoTexto(out tituloHorimetro, out horimetro);

					tituloHorimetro.Text = AppResources.Horimetro;

					horimetro.Text = String.Format(
							"{0} h"
						, _posicao.Horimetro.Value.ToString(
							"N2"
							, Resx.AppResources.Culture
						)
						);

					boxGrid.Children.Add(tituloHorimetro, 0, posRow);
					boxGrid.Children.Add(horimetro, 1, posRow);
					posRow++;
				}
				#endregion

				#region Temperatura
				if (_posicao.TemperaturaInterna.HasValue)
				{

					Label temperaturaInterna;
					Label TituloTemperaturaInterna;

					PadraoLinhaDetalhePosicaoTexto(out TituloTemperaturaInterna, out temperaturaInterna);

					TituloTemperaturaInterna.Text = AppResources.Temperatura;

					temperaturaInterna.Text =
						String.Format(
							"{0} &deg C"
						  , _posicao.TemperaturaInterna.Value.ToString(
							  "N2"
							  , Resx.AppResources.Culture
							 )
						);


					boxGrid.Children.Add(TituloTemperaturaInterna, 0, posRow);
					boxGrid.Children.Add(temperaturaInterna, 1, posRow);
					posRow++;
				}
				#endregion

				#region BateriaBackup
				if (_posicao.BateriaBackup.HasValue)
				{

					Label bateriaBackup;
					Label tituloBateriaBackup;

					PadraoLinhaDetalhePosicaoTexto(out tituloBateriaBackup, out bateriaBackup);

					tituloBateriaBackup.Text = AppResources.BateriaBackup;

					bateriaBackup.Text = _posicao.BateriaBackup.Value.ToString(
						"N2"
						, Resx.AppResources.Culture
					);

					boxGrid.Children.Add(tituloBateriaBackup, 0, posRow);
					boxGrid.Children.Add(bateriaBackup, 1, posRow);
					posRow++;
				}
				#endregion

				#region StatusGps NomeEvento
				if (!String.IsNullOrEmpty(_posicao.Evento))
				{

					Image imageEvento;

					Label bateriaEvento;
					Label titulobateriaEvento;

					PadraoLinhaDetalhePosicaoTexto(out titulobateriaEvento, out bateriaEvento);

					titulobateriaEvento.Text = AppResources.Evento;

					bateriaEvento.Text = _posicao.Evento;
					bateriaEvento.TextColor = Color.Red;

					boxGrid.Children.Add(titulobateriaEvento, 0, posRow);
					boxGrid.Children.Add(bateriaEvento, 1, posRow);
					posRow++;
				}
				#endregion

				for (Int32 i = 0; i < posRow; i++)
				{
					boxGrid.RowDefinitions.Add(new RowDefinition()
					{
						Height = GridLength.Star
					});
				}

				for (Int32 i = 0; i < posRowTopo; i++)
				{
					boxGridTopo.RowDefinitions.Add(new RowDefinition()
					{
						Height = GridLength.Star
					});
				}

				gridTemp.Children.Add(boxGridTopo);
				gridTemp.Children.Add(boxGrid);
			}
			else
			{
				SemInfo(gridTemp);
			}

		}

		private void SemInfo(StackLayout paramStackLayout)
		{
			Label label = new Label()
			{
				Text = AppResources.SemInformacao,
				TextColor = Color.LightGray,
				HeightRequest = 25,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(marginStacklayoutLeftRight)
			};
			paramStackLayout.Children.Add(label);
		}

		public void PainelDetalhesPropertyChanged(
			object sender
			, PropertyChangingEventArgs e
		)
		{
			if (e.PropertyName == "Height")
			{
				_viewModelPosicao.PainelDetalhesHeightRequest = ((StackLayout)sender).Height;
			}
		}

		public void MontaMapa()
		{
			PainelMapa.Children.Clear();
			PainelMapa.Children.Add(mapaPosicao);
		}

		public void MontaStreetView(Double paramLatitude, Double paramLongitude)
		{

			WebView WVStreet = new WebView();

			Device.BeginInvokeOnMainThread(() =>
			{


				//WVStreet = _viewModelPosicao.StreetView(paramLatitude, paramLongitude);

				//WebView WVStreet = new WebView();

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



					WVStreet.HeightRequest = PainelMapa.Height;
					WVStreet.WidthRequest = PainelMapa.Width;

					PainelMapa.Children.Clear();
					PainelMapa.Children.Add(WVStreet);

				}
				catch
				{
					//ShowErrorAlert("Exception");
				}

			});


		}

		public Grid MontaGridTopo()
		{
			Grid boxGrid = new Grid();
			boxGrid.Margin = new Thickness(0, 0, 0, 0);
			boxGrid.WidthRequest = _viewModelPosicao.DefaultWidth;
			boxGrid.ColumnSpacing = 0;
			boxGrid.RowSpacing = 0;
			boxGrid.VerticalOptions = LayoutOptions.FillAndExpand;

			boxGrid.RowDefinitions = new RowDefinitionCollection();

			boxGrid.ColumnDefinitions = new ColumnDefinitionCollection();
			ColumnDefinition col01 = new ColumnDefinition()
			{
				Width = 37
			};

			ColumnDefinition col02 = new ColumnDefinition()
			{
				Width = GridLength.Star
			};

			boxGrid.ColumnDefinitions.Add(col01);
			boxGrid.ColumnDefinitions.Add(col02);

			return boxGrid;
		}

		public Grid MontaGridPadrao()
		{
			Grid boxGrid = new Grid();
			boxGrid.Margin = new Thickness(0, 17, 0, 0);
			boxGrid.WidthRequest = _viewModelPosicao.DefaultWidth;
			boxGrid.ColumnSpacing = 0;
			boxGrid.RowSpacing = 0;
			boxGrid.VerticalOptions = LayoutOptions.FillAndExpand;

			boxGrid.RowDefinitions = new RowDefinitionCollection();

			boxGrid.ColumnDefinitions = new ColumnDefinitionCollection();
			ColumnDefinition col01 = new ColumnDefinition()
			{
				Width = 150 //37
			};

			ColumnDefinition col02 = new ColumnDefinition()
			{
				Width = GridLength.Star
			};

			boxGrid.ColumnDefinitions.Add(col01);
			boxGrid.ColumnDefinitions.Add(col02);

			return boxGrid;
		}

		public Grid MontaGridPadraoTexto()
		{
			Grid boxGrid = new Grid();
			boxGrid.Margin = new Thickness(0, 3, 0, 0);
			boxGrid.WidthRequest = _viewModelPosicao.DefaultWidth;
			boxGrid.ColumnSpacing = 0;
			boxGrid.RowSpacing = 5;
			boxGrid.VerticalOptions = LayoutOptions.FillAndExpand;

			boxGrid.RowDefinitions = new RowDefinitionCollection();

			boxGrid.ColumnDefinitions = new ColumnDefinitionCollection();
			ColumnDefinition col01 = new ColumnDefinition()
			{
				Width = 250
			};

			ColumnDefinition col02 = new ColumnDefinition()
			{
				Width = GridLength.Star
			};

			boxGrid.ColumnDefinitions.Add(col01);
			boxGrid.ColumnDefinitions.Add(col02);

			return boxGrid;
		}

		public void PadraoLinhaDetalhePosicaoTexto(out Label paramLabelTitulo, out Label paramLabel)
		{
			paramLabelTitulo = new Label()
			{
				FontSize = 13,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottom
				),
				HorizontalTextAlignment = TextAlignment.Start
			};

			paramLabel = new Label()
			{
				FontSize = 13,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottom
				),
				HorizontalTextAlignment = TextAlignment.Start
			};
		}

		public void PadraoLinhaDetalhePosicaoTextoInformacoes(out Label paramLabelTitulo, out Label paramLabel)
		{
			paramLabelTitulo = new Label()
			{
				FontSize = 13,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottomAnother
				),
				HorizontalTextAlignment = TextAlignment.Start
			};

			paramLabel = new Label()
			{
				FontSize = 13,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottomAnother
				),
				HorizontalTextAlignment = TextAlignment.Start
			};
		}


		public void PadraoLinhaDetalhePosicao(out Image paramImage, out Label paramLabel)
		{

			paramImage = new Image()
			{
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottom
					, 0
					, marginStacklayoutTopBottom
				),
				HorizontalOptions = LayoutOptions.Start
			};

			paramLabel = new Label()
			{
				FontSize = 13,
				VerticalTextAlignment = TextAlignment.Center,
				Margin = new Thickness(
					marginStacklayoutLeftRight
					, marginStacklayoutTopBottom
				),
				HorizontalTextAlignment = TextAlignment.Start
			};

		}

		public String FormatarData(DateTime paramData)
		{
			return String.Format(
				"{0:dd/MM/yyyy HH:mm:ss}"
				, paramData.ToLocalTime()
			);
		}

		#region interface
		public void ExibirLoad()
		{
			Device.BeginInvokeOnMainThread(() => _painelTopLoad.ShowAlert());
		}

		public void EscondeLoad()
		{
			Device.BeginInvokeOnMainThread(() => _painelTopLoad.HideAlert());
		}

		public void MontaDetalheTopoPosicao(PosicaoHistorico paramPosicao)
		{
			BuildCellDetalhes build = new BuildCellDetalhes();
			PainelDetalhes.Children.Clear();

			Image ImageAncora = null;

			if (paramPosicao.ExibeUltimaPosicao)
			{
				ImageAncora = new Image()
				{
					Source = "ic_menu_escuro.png",
					HeightRequest = 15,
					WidthRequest = 16,
					Margin = new Thickness(20, 0),
					Opacity = 1,
					VerticalOptions = LayoutOptions.Center
				};

				ImageAncora.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(ImageAncora_Tap),
					CommandParameter = paramPosicao,
					NumberOfTapsRequired = 1
				});
			}

			Grid tempGrid = build.BuildCell(paramPosicao, ImageAncora);

			Device.BeginInvokeOnMainThread(() =>
			{
				PainelDetalhes.Children.Add(tempGrid);
			});
		}

		private async void ImageAncora_Tap(object obj)
		{

			_tokensource = new CancellationTokenSource();

			try
			{

				PosicaoHistorico _posicao = (PosicaoHistorico)obj;

				String answer;
				String ancora;

				if (_posicao.Ancora_Tolerancia == null)
				{
					ancora = AppResources.AtivarAncora;
				}
				else
				{
					ancora = AppResources.DesativarAncora;
				}

				Boolean result = await _viewModelPosicao.ExibeMenuComandos();
				//await Task.Delay(3000);


				if (result == true)
				{
					answer = await
					_messageService.ShowMessageAsync(
					AppResources.ChooseAction
					, AppResources.cancelar
					, null
					, new string[] {
						ancora
						, AppResources.EnviarComandos
						}
					);
				}
				else
				{
					answer = await
					_messageService.ShowMessageAsync(
					  AppResources.ChooseAction
					  , AppResources.cancelar
					  , null
					  , new string[] {
						ancora
						}
					);

				}

				if (answer == AppResources.EnviarComandos)
				{
					this._viewModelPosicao.NavigateToComandos();
				}
				else if (answer == AppResources.AtivarAncora)
				{
					this._viewModelPosicao.AtivarAncora(_posicao);
				}
				else if (answer == AppResources.DesativarAncora)
				{
					this._viewModelPosicao.DesativarAncora(_posicao);
				}

			}
			catch (Exception ex)
			{

			}

		}

		public void ExibeTitulo()
		{
			PosicaoHistorico posicaoHistorico;

			if (_viewModelPosicao._posicaoHistorico != null)
				posicaoHistorico = _viewModelPosicao._posicaoHistorico;
			else
				posicaoHistorico = _posicaoHistorico;

			_viewModelPosicao.TxtTitulo = AppResources.PositionDetails;

			if (posicaoHistorico.OrdemRastreador == null)
				posicaoHistorico.OrdemRastreador = 0;

			_viewModelPosicao.TxtSubTitulo = posicaoHistorico.IdentificacaoUnidadeRastreada;

		}

		#endregion

	}
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}