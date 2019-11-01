using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelComando: ViewModelBase
	{
		public Posicao PosicaoUnidadeRastreada { get; set; }
		public ComandoDto ComandoGlobal { get; set; }
		public CancellationTokenSource _tokensourceAction { get; set; }

		private ModelComando _model;
		public ModelComando Model
		{
			get
			{
				if(_model == null)
				{
					_model = new ModelComando();
				}
				return _model;
			}
		}

		public IViewComando _view { get; set; }

		public ICommand VoltarCommand { get; set; }

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


		List<ComandoDto> _commandPickerSource = new List<ComandoDto>();
		public List<ComandoDto> CommandPickerSource
		{
			get
			{
				return _commandPickerSource;
			}
			set
			{
				_commandPickerSource = value;
				this.Notify("CommandPickerSource");
			}
		}

		String _commandPickerTitle;
		public String CommandPickerTitle
		{
			get
			{
				return _commandPickerTitle;
			}
			set
			{
				_commandPickerTitle = value;
				this.Notify("CommandPickerTitle");
			}
		}

		Double _commandPickerHeight;
		public Double CommandPickerHeight
		{
			get
			{
				return _commandPickerHeight;
			}
			set
			{
				_commandPickerHeight = value;
				this.Notify("CommandPickerHeight");
			}
		}

		Double _scrollParameterHeight;
		public Double ScrollParameterHeight
		{
			get
			{
				return _scrollParameterHeight;
			}
			set
			{
				_scrollParameterHeight = value;
				this.Notify("ScrollParameterHeight");
			}
		}

		List<ComandoParametroDto> _parameterBoxBindingContext;
		public List<ComandoParametroDto> ParameterBoxBindingContext
		{
			get
			{
				return _parameterBoxBindingContext;
			}
			set
			{
				_parameterBoxBindingContext = value;
				this.Notify("ParameterBoxBindingContext");
			}
		}

		Color _parameterBoxBackgroundColor;
		public Color ParameterBoxBackgroundColor
		{
			get
			{
				return _parameterBoxBackgroundColor;
			}
			set
			{
				_parameterBoxBackgroundColor = value;
				this.Notify("ParameterBoxBackgroundColor");
			}
		}

		Thickness _commandMarginDefault;
		public Thickness CommandMarginDefault
		{
			get
			{
				return _commandMarginDefault;
			}
			set
			{
				_commandMarginDefault = value;
				this.Notify("CommandMarginDefault");
			}
		}

		Thickness _scrollParameterMargin;
		public Thickness ScrollParameterMargin
		{
			get
			{
				return _scrollParameterMargin;
			}
			set
			{
				_scrollParameterMargin = value;
				this.Notify("ScrollParameterMargin");
			}
		}

		Double _commandWidthDefault;
		public Double CommandWidthDefault
		{
			get
			{
				return _commandWidthDefault;
			}
			set
			{
				_commandWidthDefault = value;
				this.Notify("CommandWidthDefault");
			}
		}

		Boolean _commandPickerIsEnabled;
		public Boolean CommandPickerIsEnabled
		{
			get
			{
				return _commandPickerIsEnabled;
			}
			set
			{
				_commandPickerIsEnabled = value;
				this.Notify("CommandPickerIsEnabled");
			}
		}

		String _btnSendText;
		public String BtnSendText
		{
			get
			{
				return _btnSendText;
			}
			set
			{
				_btnSendText = value;
				this.Notify("BtnSendText");
			}
		}

		Boolean _btnSendIsEnabled;
		public Boolean BtnSendIsEnabled
		{
			get
			{
				return _btnSendIsEnabled;
			}
			set
			{
				_btnSendIsEnabled = value;
				this.Notify("BtnSendIsEnabled");
			}
		}

		public ICommand BtnSendCommand { get; set; }

		String _txtSubTitulo;
		public String TxtSubTitulo
		{
			get
			{
				return _txtSubTitulo;
			}
			set
			{
				_txtSubTitulo = value;
				this.Notify("TxtSubTitulo");
			}
		}

		public ViewModelComando(
			object obj
		)
		{
			PosicaoUnidadeRastreada = (Posicao)obj;
			HeightRequestDefault = _app.DefaultTemplateHeightContent;
			BtnSendCommand = new Command(BtnSendAction);

			BtnSendText = AppResources.SendCommand.ToUpper();
			BtnSendIsEnabled = false;
			if(Device.RuntimePlatform == Device.Android){
				ParameterBoxBackgroundColor =
					Color.FromRgba(
					239
					, 239
					, 244
					, 255
				);
			}

			ExpecificaTamanhos();
		}

		public override void OnAppearing()
		{
			this.DefaultTemplateBuild();


			TxtSubTitulo = PosicaoUnidadeRastreada.IdentificacaoUnidadeRastreada;

			_view.ExibirLoad();
			ComandoGlobal = new ComandoDto();
			BuscarComando();
		}

		public override void OnDisappearing()
		{
			if(_tokensourceAction != null)
				_tokensourceAction.Cancel();
		}

		public override void OnLayoutChanged()
		{
		}

		public override void DefaultTemplateBuild()
		{

			BoxLeftContent = VoltarButtonDefault();

			StackLayout stackTopo = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
				HeightRequest = _app.DefaultTemplateHeightNavegationBar,
				Margin = 0,
				Spacing = 0
			};

			Label labelTitulo = PanelTituloLabel_Titulo();
			Label labelSubTitulo = PanelTituloLabel_SubTitulo();

			labelTitulo.HeightRequest = labelSubTitulo.HeightRequest;
			labelTitulo.Text = AppResources.SendCommand;
			labelTitulo.VerticalTextAlignment = TextAlignment.End;
			stackTopo.Children.Add(labelTitulo);

			labelSubTitulo.SetBinding(
				Label.TextProperty
				, new TemplateBinding ("Parent.BindingContext.TxtSubTitulo")
			);

			stackTopo.Children.Add(labelSubTitulo);

			BoxMiddleContent = stackTopo;

		}

		private void ExpecificaTamanhos()
		{
			CommandPickerHeight = 53;

			CommandMarginDefault = new Thickness(
				20
				, 10
			);
			ScrollParameterMargin = new Thickness(
				CommandMarginDefault.Left
				, 0
				, CommandMarginDefault.Right
				, 0
			);
			CommandWidthDefault = DefaultWidth - (
				CommandMarginDefault.Left
				+ CommandMarginDefault.Right
			);

			ScrollParameterHeight =  HeightRequestDefault - (
				CommandPickerHeight
				+ CommandMarginDefault.Top
				+ CommandMarginDefault.Bottom
			);
		}

		private void BuscarComando()
		{
			ServiceResult<List<ComandoDto>> result = new ServiceResult<List<ComandoDto>>();
			try
			{
				_tokensourceAction = new CancellationTokenSource();
				Task.Run(async () => 
				{
					try
					{
						result = await Model.ListComandoRastreador(
							PosicaoUnidadeRastreada.IdRastreador
							, _tokensourceAction.Token
						);

						BuscarComando_Finish(result);
					}
					catch
					{
						result.IsValid = false;
						result.MessageError = "ErroInesperado";

						BuscarComando_Finish(result);

					}
				}, _tokensourceAction.Token);
			}
			catch
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";

				BuscarComando_Finish(result);

			}
		}

		private void BuscarComando_Finish(
			ServiceResult<List<ComandoDto>> paramResult 
		)
		{
			if(!_tokensourceAction.IsCancellationRequested)
			{

				UpdateToken(paramResult.RefreshToken);

				if(String.IsNullOrWhiteSpace(paramResult.MessageError))
				{
					CommandPickerSource = paramResult.Data;

					if(CommandPickerSource.Count() == 0)
					{
						CommandPickerIsEnabled = false;
						CommandPickerTitle = AppResources.NoCommandsFound;
						_messageService.ShowAlertAsync(
							AppResources.NoCommandsFound
						);
					}
					else
					{
						CommandPickerTitle = AppResources.SelectCommand;
						CommandPickerIsEnabled = true;
					}

				}
				else
				{
					_messageService.ShowAlertAsync(
						AppResources.ResourceManager.GetString(paramResult.MessageError)
					);
				}

				_view.EscondeLoad();
			}
		}

		public void PickerSelectedIndexChanged(
			object sender
			, System.EventArgs e
		)
		{
			_view.ExibirLoad();
			Picker temp = (Picker)sender;

			ComandoDto comando = (ComandoDto)temp.SelectedItem;

			if(comando != null)
			{
				if(comando.IdObjeto != ComandoGlobal.IdObjeto)
				{
					ComandoGlobal = new ComandoDto();
					ComandoGlobal.Clone(comando);

					if(ComandoGlobal.TipoAtuador == (Int32)EnumTipoAtuador.ComandoComParametro)
					{
						BuscaParametros(ComandoGlobal);
					}
					else
					{
						ServiceResult<List<ComandoParametroDto>> result 
							= new ServiceResult<List<ComandoParametroDto>>();

						List<ComandoParametroDto> lst = new List<ComandoParametroDto>();
						lst.Add(new ComandoParametroDto()
						{
							IdParametro = 0,
							Label = AppResources.StatusOutput,
							IdTipoParametro = (int)EnumTipoParametro.AtivacaoSaida,
							Valor = "1",
							Dominio =  String.Format(
								"0|{0};1|{1}"
								, AppResources.Desativar
								, AppResources.Ativar
							),
							Ordem = 0
						});

						result.Data = lst;

						BuscaParametros_Finish(result);
					}
				}
				else
				{
					_view.EscondeLoad();
				}
			}
			else
			{
				_view.EscondeLoad();
				ParameterBoxBindingContext = null;
			}

		}

		private void BuscaParametros(
			ComandoDto paramComando
		)
		{
			ServiceResult<List<ComandoParametroDto>> result = new ServiceResult<List<ComandoParametroDto>>();
			try
			{
				_tokensourceAction = new CancellationTokenSource();
				Task.Run(async () => 
				{
					try
					{
						result = await Model.ListParametros(
							PosicaoUnidadeRastreada.IdRastreador
							, paramComando.IdObjeto
							, _tokensourceAction.Token
						);
					}
					catch
					{
						result.IsValid = false;
						result.MessageError = "ErroInesperado";
					}
					finally
					{
						BuscaParametros_Finish(result);
					}
				}, _tokensourceAction.Token);
			}
			catch
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";

				BuscaParametros_Finish(result);

			}
		}

		private void BuscaParametros_Finish(
			ServiceResult<List<ComandoParametroDto>> paramResult 
		)
		{
			if(!_tokensourceAction.IsCancellationRequested)
			{

				UpdateToken(paramResult.RefreshToken);

				if(String.IsNullOrWhiteSpace(paramResult.MessageError))
				{
					ParameterBoxBindingContext = paramResult.Data;
				}
				else
				{
					_messageService.ShowAlertAsync(
						AppResources.ResourceManager.GetString(paramResult.MessageError)
					);
				}

				_view.EscondeLoad();
			}
		}

		//TODO:Tradução erros
		private void BtnSendAction(object obj)
		{
			try
			{
				_view.ExibirLoad();
				_tokensourceAction = new CancellationTokenSource();
				Task.Run(async () => 
				{
					ServiceResult<StatusComandoDto> comando = new ServiceResult<StatusComandoDto>();
					try
					{

						List<ComandoParametroDto> lst = _view.RecuperaValor();

						ServiceResult<List<ComandoParametroDto>> result 
							= new ServiceResult<List<ComandoParametroDto>>();

						if(lst.Count > 0)
						{
							foreach(ComandoParametroDto item in lst)
							{
								ComandoParametroDto paramDb = 
									ParameterBoxBindingContext
										.FirstOrDefault(
											x => x.IdParametro == item.IdParametro
										);

								if(paramDb != null)
								{
									if(!String.IsNullOrWhiteSpace(paramDb.Dominio))
									{

										if (paramDb.TamanhoMaximo.HasValue)
										{
											if(item.Valor.Length > paramDb.TamanhoMaximo.Value)
											{
												result.MessageError += String.Format(
													"{0}: {1} {2}"
													, paramDb.Label
													, "MuitoGrande"
													, Environment.NewLine
												);
											}
										}

										if(
											paramDb.IdTipoParametro == (Int32)EnumTipoParametro.TextboxLivre
											|| paramDb.IdTipoParametro == (Int32)EnumTipoParametro.TextboxNumeroInteiro 
											|| paramDb.IdTipoParametro == (Int32)EnumTipoParametro.TextboxNumeroDecimal
										)
										{

											if(!String.IsNullOrWhiteSpace(paramDb.Dominio))
											{
												String[] minMax = paramDb.Dominio.Split('>');

												Int32 min = 0;
												Int32 max = 0;

												if (minMax.Count() == 2) {
													min = Convert.ToInt32(minMax[0]);
													max = Convert.ToInt32(minMax[1]);
												} else {
													min = 0;
													max = Convert.ToInt32(minMax[0]);
												}

												Double valor = Double.Parse(item.Valor);
												// if Entry text is longer then valid length
												if (valor < min || valor > max)
												{
													result.MessageError += String.Format(
														"{0}: {1} {2}"
														, paramDb.Label
														, "ForaDaFaixa"
														, Environment.NewLine
													);
												}
											}
										}
									}
								}
								else
								{
									
								}
							}

							if(
								lst.Count() == 1 
								&& lst[0].IdTipoParametro == (int)EnumTipoParametro.AtivacaoSaida
							)
							{
								lst[0].Label = ComandoGlobal.ID; //SAI_33 || CSP_33
							}


							if(!_tokensourceAction.IsCancellationRequested)
							{
								comando = await Model.SendCommand(
									PosicaoUnidadeRastreada.IdRastreador
									, PosicaoUnidadeRastreada.OrdemRastreador
									, PosicaoUnidadeRastreada.IdRastreadorUnidadeRastreada
									, ComandoGlobal.IdObjeto
									, JsonConvert.SerializeObject(lst)
									, _tokensourceAction.Token
								);
							}
						}

					}
					catch
					{
						comando.MessageError = "Exception";
					}
		         	finally
					{
						FinalizaEnvioComando(comando);
					}
				}, _tokensourceAction.Token);

			}
			catch
			{
				_view.EscondeLoad();
				_messageService.ShowAlertAsync(
					AppResources.Exception
					, AppResources.Erro
				);
			}
		}
        
        private async Task FinalizaEnvioComando(
			ServiceResult<StatusComandoDto> paramComando
		)
		{

			if(!_tokensourceAction.IsCancellationRequested)
			{
				try
				{
					
					UpdateToken(paramComando.RefreshToken);
					
					if(String.IsNullOrWhiteSpace(paramComando.MessageError))
					{
						Boolean go = await _messageService.ShowAlertChooseAsync(
							AppResources.CommandSendSuccessWhereToGo
							, AppResources.Stay
							, AppResources.Go
						);

						if(go)
						{
							_navigationService.NavigateComandosParaNaoProcessado(
								PosicaoUnidadeRastreada.IdentificacaoUnidadeRastreada
							);
						}
						else
						{
							_view.LimpaComandos();
							BtnSendIsEnabled = false;
							ComandoGlobal.IdObjeto = -1000;
						}
					}
					else
					{
						ShowErrorAlert(paramComando.MessageError);
					}

				}
				catch
				{
					_messageService.ShowAlertAsync(
						AppResources.Exception
						, AppResources.Erro
					);
				}
				finally
				{
					_view.EscondeLoad();
				}
			}

		}

	}

	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}