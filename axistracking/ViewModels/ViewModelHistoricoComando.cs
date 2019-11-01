using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using axistracking.Domain.Dto;
using axistracking.Model;
using axistracking.Resx;
using axistracking.ViewModels.Base;
using axistracking.Views.Interface;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
    public class ViewModelHistoricoComando : ViewModelBaseListPage
    {

        private CancellationTokenSource _tokensource { get; set; }
        public IViewListaComandos _viewComandos;

        private Int32 _actualPage { get; set; } = 0;
        private Int32 _pageSize { get; set; } = 100;


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
                this.Notify("List_Source");
            }
        }

        public ICommand BuscarMaisCommand { get; set; }

        private Boolean _maisCommandIsEnabled;
        public Boolean MaisCommandIsEnabled
        {
            get
            {
                return _maisCommandIsEnabled;
            }
            set
            {
                _maisCommandIsEnabled = value;
                this.Notify("MaisCommandIsEnabled");
            }
        }

        public ViewModelHistoricoComando()
        {
            DefaultTemplateBuild();
            BuscarMaisCommand = new Command(this.BuscarMaisAction);
        }

        public override void OnAppearing()
        {
            try
            {
                DefaultTemplateBuild();
                ListRefresh(null);
            }
            catch
            {
            }
        }

        public override void OnDisappearing()
        {
            _tokensource.Cancel();
        }

        public override void OnLayoutChanged()
        {
        }

        public override void DefaultTemplateBuild()
        {
            BoxLeftContent = VoltarButtonDefault();

            Label titulo = PanelTituloLabel_Titulo();
            titulo.FontSize = 20;
            titulo.Text = AppResources.CommandHistory;

            BoxMiddleContent = titulo;
        }

        public override void ListRefresh(object obj)
        {
            if (!this.List_IsRefreshing)
            {
                MaisCommandIsEnabled = false;
                _actualPage = 0;
                ListRefreshAction();
            }
        }

        public void ListRefreshAction()
        {
            ModelComando _modelComandos = new ModelComando();
            this.List_IsRefreshing = true;

            _viewComandos.ExibirLoad();

            ServiceResult<HistoricoComandoRespost> resultComandos = new ServiceResult<HistoricoComandoRespost>();

            try
            {
                _tokensource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    try
                    {
                        ListEndRefresh = false;

                        resultComandos = await _modelComandos.ListCommandHistory(
                            _actualPage
                            , _pageSize
                            , _tokensource.Token
                        );
                    }
                    catch (Exception)
                    {
                        resultComandos.MessageError = "Exception";
                        resultComandos.IsValid = false;
                    }

                    ListRefresh_Finish(resultComandos);
                }, _tokensource.Token);

            }
            catch (Exception)
            {
                resultComandos.MessageError = "Exception";
                resultComandos.IsValid = false;
                ListRefresh_Finish(resultComandos);
            }
        }

        private void ListRefresh_Finish(
            ServiceResult<HistoricoComandoRespost> paramResult
        )
        {
            try
            {
                if (!_tokensource.IsCancellationRequested)
                {

                    UpdateToken(paramResult.RefreshToken);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(paramResult.MessageError))
                            {
                                ShowErrorAlert(paramResult.MessageError);
                            }
                            else
                            {
                                _list_SourceCompleta = paramResult.Data.ListComando;
                                ListPainelTop_Source = paramResult.Data.Painel;
                                this.List_Source = _list_SourceCompleta;

                                if (_list_SourceCompleta.Count() != _pageSize)
                                {
                                    MaisCommandIsEnabled = false;
                                }
                                else
                                {
                                    MaisCommandIsEnabled = true;
                                }

                            }
                            this.List_IsRefreshing = false;
                            _viewComandos.EscondeLoad();
                        }
                        catch (Exception)
                        {
                            ShowErrorAlert("Exception");
                            this.List_IsRefreshing = false;
                            _viewComandos.EscondeLoad();
                        }
                    });
                }
            }
            catch (Exception)
            {
                ShowErrorAlert("Exception");
                this.List_IsRefreshing = false;
                _viewComandos.EscondeLoad();
            }
        }

        public override void Buscar()
        {
            List_Source = _list_SourceCompleta.Where(
                x => (StatusFiltro.HasValue ? x.Status == StatusFiltro.Value : true)
                && (String.IsNullOrWhiteSpace(this.TxtBuscar) ? true : x.IdentificacaoUnidadeRastreada.Contains(this.TxtBuscar))
            ).ToList();
        }

        public override void ExibeTitulo(Int32? paramTotal)
        {
        }

        public override void MudarTamanhoLoad()
        {
            if (_viewComandos != null)
                _viewComandos.MudarTamanhoLoad();
        }

        public async Task EnviarComandoCancelar(ComandoLog paramComando,
        Boolean paramEnvia
        )
        {
            ModelComando _modelComandos = new ModelComando();
            _tokensource = new CancellationTokenSource();

            if (!_tokensource.IsCancellationRequested)
            {
                ServiceResult<Boolean> comando = new ServiceResult<Boolean>();
                try
                {
                    if (paramEnvia)
                    {
                        comando = await _modelComandos.ComandoCancelar(
                            paramComando.IdComandoLog
                            , _tokensource.Token
                        );
                    }
                }
                catch
                {
                    comando.MessageError = "Exception";
                }
                finally
                {
                    await _messageService.ShowAlertAsync(AppResources.CancelarComandoSucesso);
                }
            }
        }


        private void BuscarMaisAction(
            object obj
        )
        {
            if (!this.List_IsRefreshing)
            {
                MaisCommandIsEnabled = false;
                _actualPage++;
                ListRefreshAction();
            }
        }
    }
}
