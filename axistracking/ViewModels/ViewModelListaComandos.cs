using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Bll;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Model;
using axistracking.Resx;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelListaComandos : ViewModelBaseListPage
	{
        public IViewListaComandos _viewComandos;
        private EnumPage _page { get; set; }

        private CancellationTokenSource _tokensource { get; set; }

        private TimeSpan _refreshTime { get; set; }

        private PainelDto _painel { get; set; }

        private ComandoLog _comando { get; set; }

        private Bll_Mapa _bllMapa { get; set; }

		private String[] _arrayEventos { get; set; }

        public ICommand RefreshCommand
        {
            get;
            set;
        }

        public ICommand ListComandos_RefreshCommand
        {
            get;
            set;
        }

        public ICommand VoltarCommand
        {
            get;
            set;
        }

		public ICommand ListaEventoCommand
		{
			get;
			set;
		}

		public ICommand ComandoHistoricoCommand
		{
			get;
			set;
		}

        Boolean _painelDefaultIsVisible = true;
        public Boolean PanelDefaultIsVisible
        {
            get
            {
                return _painelDefaultIsVisible;
            }
            set
            {
                _painelDefaultIsVisible = value;
                this.Notify("PanelDefaultIsVisible");
            }
        }

		private string _txtTitulo;
		public string TxtTitulo
		{
			get
			{
				return _txtTitulo;
			}
			set
			{
				_txtTitulo = value;
				this.Notify("TxtTitulo");
			}
		}

		protected List<ComandoLog> _list_SourceCompleta = new List<ComandoLog>();
		protected List<ComandoLog> _list_Source = new List<ComandoLog>();
		public List<ComandoLog> List_Source
		{
			get
			{
				return _list_Source;
			}
			set
			{
				_list_Source = value;
				ExibeTitulo(_list_Source.Count);
				this.Notify("List_Source");
			}
		}

        public ViewModelListaComandos(
            object obj
            , EnumPage Page
			, String paramNomeUnidade
        )
        {
            _page = Page;
			TxtBuscar = paramNomeUnidade;
            _painel = (PainelDto)obj;

            this.RefreshCommand = new Command(this.Refresh);
            this.VoltarCommand = new Command(this.VoltarAction);
			this.ComandoHistoricoCommand = new Command(this.ComandoHistoricoAction);

            _refreshTime = App.Configuracao.RefreshTime;
            _tokensource = new CancellationTokenSource();

        }

		public override void OnDisappearing()
        {
            _tokensource.Cancel(false);
        }

        public override void OnAppearing()
        {
            try
            {
				ExibeTitulo(List_Source.Count());
                Double linhas = _painel.Grafico.Count / (double)2;
                if (linhas > 1)
                {
                    linhas = 2;
                }
                else
                {
                    linhas = 1;
                }
                ListPainelTop_Height = ListPainelTop_HeightPadrao * linhas;
                List_Height = DefaultHeightContent - ListPainelTop_Height;

                this.ListPainelTop_Source = _painel;

                if (App.ListPainelTopUnidadesSource != null && App.ListPainelTopUnidadesSource.Count() > 0)
                {
                    PainelDto painelTopTemp = App.ListPainelTopUnidadesSource.FirstOrDefault();
                    if ((DateTime.UtcNow - painelTopTemp.LastSearch).TotalSeconds
                        < _refreshTime.TotalSeconds)
                    {
						this.List_Source = App.ListComandosSource;
						List_IsRefreshing = false;
                        return;
                    }
                }

                Loop(TimeSpan.FromMilliseconds(500));

            }
            catch
            {
            }
        }

        public override void OnLayoutChanged()
        {
        }

		public override void DefaultTemplateBuild()
		{
			BoxLeftContent = VoltarButtonDefault();

			Label titulo = PanelTituloLabel_Titulo();
			titulo.FontSize = 20;
			titulo.Text = "0 " + AppResources.Comando;

			titulo.SetBinding(
				Label.TextProperty
				, new TemplateBinding ("Parent.BindingContext.TxtTitulo")
			);

			BoxMiddleContent = titulo;

			StackLayout stackRight = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HeightRequest = _app.DefaultTemplateHeightNavegationBar,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Margin = 0,
				Spacing = 0
			};


			Button comandoHistorico = new Button()
			{
				Image = "ic_historico.png",
				HeightRequest = stackRight.HeightRequest,
				BorderRadius = 0,
				BorderWidth = 0,
				BorderColor = Color.Transparent,
				BackgroundColor = Color.Transparent,
				IsVisible = true
			};

			comandoHistorico.SetBinding(
				Button.CommandProperty
				, new TemplateBinding ("Parent.BindingContext.ComandoHistoricoCommand")
			);

			stackRight.Children.Add(comandoHistorico);

			BoxRightContent = stackRight;
		}

        private void VoltarAction(object obj)
        {
            this._navigationService.Voltar();
        }

        private void ListPainelTopRefresh()
        {

            ModelPainel _modelPainel = new ModelPainel();
            _viewComandos.ExibirLoad();

            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        //ServiceResult<List<PainelDto>> resultPainel = await _modelPainel.ListPainelAsync();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                this.ListPainelTop_Source = _painel;
                                _viewComandos.EscondeLoad();

                            }
                            catch (Exception)
                            {
                            }
                        });


                    }
                    catch (Exception)
                    {
                    }
                }, _tokensource.Token);

            }
            catch (Exception)
            {
                _viewComandos.EscondeLoad();
            }
            finally
            {
            }
        }

		//TODO: Refazer Metodo
		public override void ListRefresh(object obj)
        {
            ModelComando _modelComandos = new ModelComando();
            this.List_IsRefreshing = true;

            try
            {
				_tokensource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    try
                    {
                        ListEndRefresh = false;

                        ServiceResult<List<ComandoLog>> resultComandos = 
							await _modelComandos.ListComandosEnviadosAsync(
								_tokensource.Token
							);

						if(!_tokensource.IsCancellationRequested)
						{
	                        App.ListComandosSource = resultComandos.Data;

							UpdateToken(resultComandos.RefreshToken);

	                        Device.BeginInvokeOnMainThread(() =>
	                        {
	                            try
	                            {
	                                if (!String.IsNullOrWhiteSpace(resultComandos.MessageError))
	                                {
										ShowErrorAlert(resultComandos.MessageError);
	                                }
	                                _list_SourceCompleta = resultComandos.Data;
									this.List_Source = _list_SourceCompleta;
	                                this.List_IsRefreshing = false;
									Buscar();
	                            }
	                            catch (Exception)
	                            {
									ShowErrorAlert("Exception");
									this.List_IsRefreshing = false;
	                            }
	                        });
						}
                    }
                    catch (Exception)
                    {
						ShowErrorAlert("Exception");
                        this.List_IsRefreshing = false;
                    }
                }, _tokensource.Token);

            }
            catch (Exception)
			{
				ShowErrorAlert("Exception");
				this.List_IsRefreshing = false;
            }
        }

        private async Task Loop(TimeSpan paramRefreshTime)
        {

            try
            {
                await Task.Delay(paramRefreshTime, _tokensource.Token);

                if (!this.List_IsRefreshing)
                {
                    this.ListPainelTopRefresh();
                    this.ListRefresh(null);
                }

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }

        }

        private void Refresh(object obj)
        {
            if (!this.List_IsRefreshing)
            {
                _tokensource.Cancel();
                _tokensource = new CancellationTokenSource();
                this.ListRefresh(null);
            }
        }

		public override void Buscar()
        {
            List_Source = _list_SourceCompleta.Where(
				x =>(StatusFiltro.HasValue ? x.Status == StatusFiltro.Value : true)
				&& (String.IsNullOrWhiteSpace(this.TxtBuscar) ? true : x.IdentificacaoUnidadeRastreada.Contains(this.TxtBuscar))
           ).ToList();
        }

		private void ComandoHistoricoAction(object obj)
		{
			this._navigationService.NavigateToComandoHistorico();
		}

		public override void ExibeTitulo(Int32? paramTotal)
		{
			String strTexto = "";
			if (paramTotal.HasValue)
			{
				strTexto = paramTotal.ToString() + " ";

				if (paramTotal < 2)
					strTexto += AppResources.Comando;
				else
					strTexto += AppResources.Comandos;
			}
			else
			{
				strTexto += AppResources.Comando;
			}

			TxtTitulo = strTexto;

		}

		public override void MudarTamanhoLoad()
		{
			if(_viewComandos != null)
				_viewComandos.MudarTamanhoLoad();
		}
	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}
