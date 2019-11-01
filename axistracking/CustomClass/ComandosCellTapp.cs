using System;
using axistracking.CustomElements;
using axistracking.Domain.Dto;
using axistracking.Resx;
using Xamarin.Forms;

namespace axistracking.CustomClass
{
	public class ComandosCellTapp
	{
		private CustomDialogAlert _painelDetalhes;
		private App _app => (Application.Current as App);
		
		public ComandosCellTapp(
			ComandoLog paramComando
			, CustomDialogAlert paramPainelDetalhes
		)
		{
			_painelDetalhes = paramPainelDetalhes;
			Color corTitulo = Color.Black;
			Color corTexto = Color.FromHex("#9b9eb0");
			Thickness marginBottom = new Thickness(0, 0, 0, 15);

			StackLayout boxPrincipal = new StackLayout();

			StackLayout boxDados = new StackLayout
			{
				Margin = 0,
				Padding = 0,
				Spacing = 0
			};


			#region Comando
			Label labelComando = new Label()
			{
				Text = paramComando.IdentificacaoComando,
				TextColor = corTitulo,
				FontAttributes = FontAttributes.Bold,
				FontSize = 15,
				Margin = 0,
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalTextAlignment = TextAlignment.Start
			};

			boxDados.Children.Add(labelComando);
			#endregion

			#region UnidadeRastreada
			if (!String.IsNullOrWhiteSpace(paramComando.UnidadeRastreada))
			{
				Label labelUnidadeRastreada = new Label()
				{
					Text = paramComando.IdentificacaoUnidadeRastreada,
					TextColor = corTexto,
					FontSize = 15,
					Margin = marginBottom,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelUnidadeRastreada);
			}
			#endregion

			#region StatusComando            

			if (!String.IsNullOrWhiteSpace(paramComando.StatusComando))
			{
				Label labelStatus = new Label()
				{
					Text = paramComando.StatusComando,
					TextColor = corTexto,
					FontSize = 15,
					FontAttributes = FontAttributes.Bold,
					Margin = marginBottom,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelStatus);
			}
			#endregion

			#region DataEnvio
			if (paramComando.DataEnvio != null)
			{
				Label labelTituloDataEnvio = new Label()
				{
					Text = AppResources.DataEnvio,
					TextColor = corTitulo,
					FontAttributes = FontAttributes.Bold,
					FontSize = 14,
					Margin = 0,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelTituloDataEnvio);

				Label labelDataEnvio = new Label()
				{
					Text = String.Format(
						"{0:dd/MM/yyyy HH:mm:ss}"
						, paramComando.DataEnvio.Value.ToLocalTime()
					),
					TextColor = corTexto,
					FontSize = 14,
					Margin = marginBottom,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelDataEnvio);
			}
			#endregion

			#region DataFila
			Label labelTituloDataFila = new Label()
			{
				Text = AppResources.DataFila,
				TextColor = corTitulo,
				FontAttributes = FontAttributes.Bold,
				FontSize = 14,
				Margin = 0,
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalTextAlignment = TextAlignment.Start
			};
			boxDados.Children.Add(labelTituloDataFila);

			Label labelDataFila = new Label()
			{
				Text = String.Format(
					"{0:dd/MM/yyyy HH:mm:ss}"
					, paramComando.DataFila.ToLocalTime()
				),
				TextColor = corTexto,
				FontSize = 14,
				Margin = marginBottom,
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalTextAlignment = TextAlignment.Start
			};
			boxDados.Children.Add(labelDataFila);
			#endregion

			#region DataFinalizacao Processamento
			if (paramComando.DataFinalizacao != null)
			{
				Label labelTituloDataProcessamento = new Label()
				{
					Text = AppResources.DataProcessamento,
					TextColor = corTitulo,
					FontAttributes = FontAttributes.Bold,
					FontSize = 14,
					Margin = 0,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelTituloDataProcessamento);

				Label labelDataProcessamento = new Label()
				{
					Text = String.Format(
						"{0:dd/MM/yyyy HH:mm:ss}"
						, paramComando.DataFinalizacao.Value.ToLocalTime()
					),
					TextColor = corTexto,
					FontSize = 14,
					Margin = marginBottom,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelDataProcessamento);
			}
			#endregion

			#region CriadoPor
			if (paramComando.UsuarioEnvio != null)
			{
				Label labelTituloCriadoPor = new Label()
				{
					Text = AppResources.CriadoPor,
					TextColor = corTitulo,
					FontAttributes = FontAttributes.Bold,
					FontSize = 14,
					Margin = 0,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelTituloCriadoPor);

				Label labelCriadoPor = new Label()
				{
					Text = paramComando.UsuarioEnvio,
					TextColor = corTexto,
					FontSize = 14,
					Margin = marginBottom,
					LineBreakMode = LineBreakMode.TailTruncation,
					HorizontalTextAlignment = TextAlignment.Start
				};
				boxDados.Children.Add(labelCriadoPor);
			}
			#endregion

			ScrollView scroll = new ScrollView
			{
				Content = boxDados
			};

			#region Botao Fechar
			//StackLayout boxFechar = new StackLayout();
			//boxFechar.HeightRequest = 30;
			//boxFechar.Orientation = StackOrientation.Horizontal;
			//boxFechar.VerticalOptions = LayoutOptions.End;
			//boxFechar.HorizontalOptions = LayoutOptions.End;

			Button botaoFecharDetalhes = new Button
			{
				Text = AppResources.Fechar,
				HorizontalOptions = LayoutOptions.End,
				BackgroundColor = Color.Transparent,
				TextColor = Color.Black,
				HeightRequest = 30
			};
			botaoFecharDetalhes.Clicked += BotaoFecharDetalhes_OnButtonClicked;

			//boxFechar.Children.Add(botaoFecharDetalhes);
			#endregion

			Frame activity = _painelDetalhes.RequireFramePadrao();

			scroll.HeightRequest = (_app.DefaultTemplateHeightContent * 0.8) - botaoFecharDetalhes.HeightRequest;
			boxPrincipal.Children.Add(scroll);
			boxPrincipal.Children.Add(botaoFecharDetalhes);

			activity.Content = boxPrincipal;

			Device.BeginInvokeOnMainThread(() =>
			{
				_painelDetalhes.ShowAlert(activity);
			});
		}

		private void BotaoFecharDetalhes_OnButtonClicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(_painelDetalhes.HideAndCleanAlert);
		}
	}
}
