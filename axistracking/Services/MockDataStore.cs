using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;
using axistracking.Resx;

namespace axistracking.Services
{
	public class MockDataStore: IDataStore
    {
        private App _app { get; set; }
                private Int32 delay => 2000;

		private List<PosicaoHistorico> _lstForListTracedUnits { get; set; }
		private List<PosicaoHistorico> _lstForListHistorico { get; set; }
        private List<PainelDto> _lstPainel { get; set; }
		private Posicao _detalhePosicao1 { get; set; }
        private List<ComandoLog> _lstCommand { get; set; }

        public async Task<ServiceResult<Boolean>> ComandoCancelar(
            int paramIdCommandLog
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();

            return result;
        }

        public MockDataStore()
		{
            #region _lstForListHistorico
			_lstForListHistorico = new List<PosicaoHistorico>();
            Int32 posHist = 0;
            for (Int32 i = 0; i < 1; i++)
            {
				_lstForListHistorico.Add(new PosicaoHistorico()
                {
                    IdPosicao = i + posHist,
                    DataEvento = new DateTime(2017, 05, 05, 17, 09, 29),
                    IdUnidadeRastreada = 5,
                    Identificacao = "Camila de Jesus Ferreira",
                    NomeRegraViolada = "Teste",
                    CorRegraPrioritaria = "#000000",                    
                    Latitude = -22.904725,
                    Longitude = -43.091967
                     
                });

                posHist++;
				_lstForListHistorico.Add(new PosicaoHistorico()
                {
                    IdPosicao = i + posHist,
                    DataEvento = new DateTime(2017, 05, 05, 17, 08, 29),
                    IdUnidadeRastreada = 5,
                    Identificacao = "Camila de Jesus Ferreira",
                    CorRegraPrioritaria = "#000000",
                    Latitude = -22.904562,
                    Longitude = -43.092332
                });

                posHist++;
				_lstForListHistorico.Add(new PosicaoHistorico()
                {
                    IdPosicao = i + posHist,
                    DataEvento = new DateTime(2017, 05, 05, 17, 07, 29),
                    IdUnidadeRastreada = 5,
                    Identificacao = "Camila de Jesus Ferreira",
                    NomeRegraViolada = "Teste",
                    Latitude = -22.904373,
                    Longitude = -43.092743
                });

                posHist++;
				_lstForListHistorico.Add(new PosicaoHistorico()
                {
                    IdPosicao = i + posHist,
                    DataEvento = new DateTime(2017, 05, 05, 17, 06, 29),
                    IdUnidadeRastreada = 5,
                    Identificacao = "Camila de Jesus Ferreira",
                    NomeRegraViolada = "Teste",
                    CorRegraPrioritaria = "#000000",
                    Latitude = - 22.904116,
                    Longitude  = - 43.093317

                });

                posHist++;
				_lstForListHistorico.Add(new PosicaoHistorico()
                {
                    IdPosicao = i + posHist,
                    DataEvento = new DateTime(2017, 05, 05, 17, 05, 29),
                    IdUnidadeRastreada = 5,
                    Identificacao = "Camila de Jesus Ferreira",
                    NomeRegraViolada = "Teste",
                    CorRegraPrioritaria = "#000000",
                    Latitude  = -22.903740,
                    Longitude  = -43.094420
                });

                posHist++;
            }
            #endregion

            #region _lstForListTracedUnits
			_lstForListTracedUnits = new List<PosicaoHistorico>();
			Int32 pos = 0;
			for (Int32 i = 0; i < 1; i++)
			{
				_lstForListTracedUnits.Add(new PosicaoHistorico()
				{
					IdPosicao = i + pos,
					DataEvento = new DateTime(2017, 05, 05, 17, 05, 29),
					IdUnidadeRastreada = 5,
					Identificacao = "Camila de Jesus Ferreira",
					Ignicao = true,
                    Latitude = -22.904725,
                    Longitude = -43.091967
                });

				pos++;
				_lstForListTracedUnits.Add(new PosicaoHistorico()
				{
					IdPosicao = i + pos,
					DataEvento = new DateTime(2017, 05, 19, 19, 41, 18),
					Ignicao = false,
					IdUnidadeRastreada = 9,
					Identificacao = "New Eco Sport",
					CorRegraPrioritaria = "#003300",
                    Latitude = -22.904562,
                    Longitude = -43.092332
                });

				pos++;
				_lstForListTracedUnits.Add(new PosicaoHistorico()
				{
					IdPosicao = i + pos,
					DataEvento = new DateTime(2017, 04, 25, 20, 08, 41),
					IdUnidadeRastreada = 2,
					Identificacao = "Uno Way",
					CorRegraPrioritaria = "#FFFFFF",
                    Latitude = -22.904373,
                    Longitude = -43.092743
                });

				pos++;
				_lstForListTracedUnits.Add(new PosicaoHistorico()
				{
					IdPosicao = i + pos,
					DataEvento = new DateTime(2017, 05, 29, 18, 43, 53),
					Ignicao = false,
					IdUnidadeRastreada = 1000078,
					Identificacao = "Marco Antonio José Nunes Malvessi",
					NomeRegraViolada = "Evento3",
					CorRegraPrioritaria = "#300300",
                    Latitude = -22.903740,
                    Longitude = -43.094420
                });

				pos++;
			}
			#endregion

			#region ListPainel
			_lstPainel = new List<PainelDto>();

			String cor1Barra = "#33343e";
			String cor1Fundo = "#181818";

			String cor2Barra = "#00ac99";
			String cor2Fundo = "#008387";

			String cor3Barra = "#ec6968";
			String cor3Fundo = "#C24A4A";

			String cor4Barra = "#E6A64E";
			String cor4Fundo = "#CC8739";

			#region UnidadesRastreadas
			PainelDto unidadesRastreadas = new PainelDto();
			unidadesRastreadas.Id = 1;
            unidadesRastreadas.Chave = "UnidadesRastreadas";
			unidadesRastreadas.HasDetalhes = true;
			unidadesRastreadas.Total = 100;
			unidadesRastreadas.LastSearch = DateTime.UtcNow;
			List<GraficoDto> lstUnidadesGrafico = new List<GraficoDto>();

			GraficoDto UnidadesRastreadasDesatualizadas = new GraficoDto()
			{
				porcento = 33.5,
				corBarra = cor3Barra,
				corFundo = cor3Fundo,
				Status = 0,
				Identificacao = "UnidadesRastreadasDesatualizadas",
				parent = 1
			};
			lstUnidadesGrafico.Add(UnidadesRastreadasDesatualizadas);

			GraficoDto UnidadesRastreadasParcialmenteAtualizadas = new GraficoDto()
			{
				porcento = 33.5,
				corBarra = cor4Barra,
				corFundo = cor4Fundo,
				Status = 2,
				Identificacao = "UnidadesRastreadasParcialmenteAtualizadas",
				parent = 1
			};
			lstUnidadesGrafico.Add(UnidadesRastreadasParcialmenteAtualizadas);

			GraficoDto atualizadas = new GraficoDto()
			{
				porcento = 33,
				corBarra = cor2Barra,
				corFundo = cor2Fundo,
				Status = 1,
				Identificacao = "UnidadesRastreadasAtualizadas",
				parent = 1
            };
			lstUnidadesGrafico.Add(atualizadas);

			unidadesRastreadas.Grafico = lstUnidadesGrafico;
			_lstPainel.Add(unidadesRastreadas);
			#endregion

			#region Alertas
			PainelDto painelalertas = new PainelDto();
			painelalertas.Id = 2;
			painelalertas.Chave = "Alertas";
			painelalertas.HasDetalhes = true;
			painelalertas.Total = 100;
			painelalertas.LastSearch = DateTime.UtcNow;
			List<GraficoDto> lstAlertasGrafico = new List<GraficoDto>();

			GraficoDto AlertasEmTratamento = new GraficoDto()
			{
				porcento = 44.5,
				corBarra = cor1Barra,
				corFundo = cor1Fundo,
				Status = 2,
				Identificacao = "EmTratamento",
				parent = 2
            };
			lstAlertasGrafico.Add(AlertasEmTratamento);

			GraficoDto AlertasNaoTratados = new GraficoDto()
			{
				porcento = 55.5,
				corBarra = cor2Barra,
				corFundo = cor2Fundo,
				Status = 1,
				Identificacao = "NaoTratados",
				parent = 2
			};
			lstAlertasGrafico.Add(AlertasNaoTratados);

			painelalertas.Grafico = lstAlertasGrafico;
			_lstPainel.Add(painelalertas);
			#endregion

			#region Comandos
			PainelDto painelcomandos = new PainelDto();
			painelcomandos.Id = 3;
			painelcomandos.Chave = "Comandos";
			painelcomandos.HasDetalhes = true;
			painelcomandos.Total = 100;
			painelcomandos.LastSearch = DateTime.UtcNow;
			List<GraficoDto> lstcomandosGrafico = new List<GraficoDto>();

			GraficoDto ComandoFalha = new GraficoDto()
			{
				porcento = 50,
				corBarra = cor1Barra,
				corFundo = cor1Fundo,
				Identificacao ="ComandoFalha",
				parent = 3,
                Status = 1

			};
			lstcomandosGrafico.Add(ComandoFalha);

			GraficoDto ComandoAguardando = new GraficoDto()
			{
				porcento = 50,
				corBarra = cor2Barra,
				corFundo = cor2Fundo,
				Identificacao ="ComandoAguardando",
				parent = 3,
                Status = 2
            };
			lstcomandosGrafico.Add(ComandoAguardando);

			painelcomandos.Grafico = lstcomandosGrafico;
			_lstPainel.Add(painelcomandos);
			#endregion

			#region Chamados
			PainelDto painelchamados = new PainelDto();
			painelchamados.Chave = "Chamados";
			painelchamados.HasDetalhes = false;
			painelchamados.Total = 0;
			painelchamados.Id=4;
			painelchamados.LastSearch = DateTime.UtcNow;
			List<GraficoDto> lstchamadosGrafico = new List<GraficoDto>();

			GraficoDto ChamadosNaoTratados = new GraficoDto()
			{
				porcento = 0,
				corBarra = cor1Barra,
				corFundo = cor1Fundo,
				Identificacao = "ChamadosNaoTratados",
				parent = 4
			};
			lstchamadosGrafico.Add(ChamadosNaoTratados);

			GraficoDto ChamadosEmTratamento = new GraficoDto()
			{
				porcento = 0,
				corBarra = cor2Barra,
				corFundo = cor2Fundo,
				Identificacao = "ChamadosEmTratamento",
				parent = 4
            };
			lstchamadosGrafico.Add(ChamadosEmTratamento);

			painelchamados.Grafico = lstchamadosGrafico;
			_lstPainel.Add(painelchamados);
			#endregion

			#region Viagens
			PainelDto painelviagens = new PainelDto();
			painelviagens.Chave = "Viagens";
			painelviagens.HasDetalhes = false;
			painelviagens.Total = 120;
			painelviagens.Id = 5;
			painelviagens.LastSearch = DateTime.UtcNow;
			List<GraficoDto> lstviagensGrafico = new List<GraficoDto>();

			GraficoDto ViagensNaoIniciadas = new GraficoDto()
			{
				porcento = 15,
				corBarra = cor1Barra,
				corFundo = cor1Fundo,
				Identificacao = "ViagensNaoIniciadas",
				parent = 5
            };
			lstviagensGrafico.Add(ViagensNaoIniciadas);

			GraficoDto ViagensNaoIniciadaAtrasadas = new GraficoDto()
			{
				porcento = 15,
				corBarra = cor2Barra,
				corFundo = cor2Fundo,
				Identificacao = "ViagensNaoIniciadaAtrasadas",
				parent = 5
            };
			lstviagensGrafico.Add(ViagensNaoIniciadaAtrasadas);

			GraficoDto ViagensEncerradas = new GraficoDto()
			{
				porcento = 30.4,
				corBarra = cor3Barra,
				corFundo = cor3Fundo,
				Identificacao = "ViagensEncerradas",
				parent = 5
			};
			lstviagensGrafico.Add(ViagensEncerradas);

			GraficoDto ViagensEmAndamento = new GraficoDto()
			{
				porcento = 59.6,
				corBarra = cor4Barra,
				corFundo = cor4Fundo,
				Identificacao = "ViagensEmAndamento",
				parent = 5
			};
			lstviagensGrafico.Add(ViagensEmAndamento);

			painelviagens.Grafico = lstviagensGrafico;
			_lstPainel.Add(painelviagens);
			#endregion

			#region pontosAtendimento
			PainelDto painelpontos = new PainelDto();
			painelpontos.Chave = "PontosAtendimento";
			painelpontos.HasDetalhes = false;
			painelpontos.Total = 0;
			painelpontos.Id = 6;
			painelpontos.LastSearch = DateTime.UtcNow;
			_lstPainel.Add(painelpontos);
			#endregion

			#region UnidadesEmServico
			PainelDto painelunidadesemservico = new PainelDto();
			painelunidadesemservico.Chave = "UnidadesEmServico";
			painelunidadesemservico.HasDetalhes = false;
			painelunidadesemservico.Total = 0;
			painelunidadesemservico.Id = 7;
			painelunidadesemservico.LastSearch = DateTime.UtcNow;
			_lstPainel.Add(painelunidadesemservico);
			#endregion

			#region CNH
			PainelDto painelcnh = new PainelDto();
			painelcnh.Chave = "cnh";
			painelcnh.HasDetalhes = false;
			painelcnh.Total = 100;
			painelcnh.Id = 8;
			painelcnh.LastSearch = DateTime.UtcNow;
			_lstPainel.Add(painelcnh);
			#endregion
			#endregion

			#region Detalhe posição
			_detalhePosicao1 = new Posicao();
			_detalhePosicao1.IdUnidadeRastreada = 1;
			_detalhePosicao1.Identificacao = "Teste";
			_detalhePosicao1.CorRegraPrioritaria = "#0f00f0";
			_detalhePosicao1.NomeRegraViolada = "Ancora";
			_detalhePosicao1.Ignicao = true;
			_detalhePosicao1.GPSValido = true;
			_detalhePosicao1.GrupoUnidadeRastreada = "Grupo teste";
			_detalhePosicao1.ModeloVeiculo = "Mock";
			_detalhePosicao1.Endereco = "Av. Roberto Silveira, 125 Niterói - RJ";
			_detalhePosicao1.DataEvento = DateTime.UtcNow;
			_detalhePosicao1.DataGPS = DateTime.UtcNow;
			_detalhePosicao1.Velocidade = 50;
			_detalhePosicao1.BateriaPrincipal = 50;
			_detalhePosicao1.BateriaBackup = 34;
			_detalhePosicao1.Evento = "Panico";
			_detalhePosicao1.IdRastreador = 0;

            _detalhePosicao1.Latitude = -22.903740;
            _detalhePosicao1.Longitude = -43.094420;


            _detalhePosicao1.Ancora_Latitude = -22.903740;
            _detalhePosicao1.Ancora_Longitude = -43.094420;
            _detalhePosicao1.Ancora_Tolerancia = 500;




            _detalhePosicao1.Sensores = new Dictionary<string, bool>();
			_detalhePosicao1.Sensores.Add("Sensores 1", true);
			_detalhePosicao1.Sensores.Add("Sensores 2", false);
			_detalhePosicao1.Sensores.Add("Sensores 3", true);
			               
			_detalhePosicao1.Atuadores = new Dictionary<string, bool>();
			_detalhePosicao1.Atuadores.Add("Atuadores 1", true);
			_detalhePosicao1.Atuadores.Add("Atuadores 2", true);
			_detalhePosicao1.Atuadores.Add("Atuadores 3", true);
			_detalhePosicao1.Atuadores.Add("Atuadores 4", true);

			_detalhePosicao1.Informacoes = new Dictionary<string, bool>();
			_detalhePosicao1.Informacoes.Add("Informacoes 1", true);
			_detalhePosicao1.Informacoes.Add("Informacoes 2", true);
			_detalhePosicao1.Informacoes.Add("Informacoes 3", true);
			_detalhePosicao1.Informacoes.Add("Informacoes 4", true);

			_detalhePosicao1.Telemetrias = new Dictionary<string, Double>();
			_detalhePosicao1.Telemetrias.Add("Telemetrias 1", 1);
			_detalhePosicao1.Telemetrias.Add("Telemetrias 2", 20.58);
			_detalhePosicao1.Telemetrias.Add("Telemetrias 3", 34);
			_detalhePosicao1.Telemetrias.Add("Telemetrias 4", 100);
            #endregion

            #region _lstCommand
            _lstCommand = new List<ComandoLog>();
            Int32 posCommand = 0;
            for (Int32 i = 0; i < 1; i++)
            {
                _lstCommand.Add(new ComandoLog()
                {
                    IdComandoLog = i + posHist,
                    DataFila = new DateTime(2017, 05, 05, 17, 09, 29),
                    DataEnvio = new DateTime(2017, 05, 05, 17, 09, 29),
                    DataFinalizacao = new DateTime(2017, 05, 05, 17, 09, 29),
                    IdSaida = (i + posHist) + 1,
                    NomeSaida = "Saida",
                    UnidadeRastreada = "Camila de Jesus Ferreira",
                    NomeComando = "Teste",
                    RefComando = AppResources.Ativar,
                    Status = 1,
                    UsuarioEnvio = "Pombo",
					StatusComando = "StatusComando"

                });

                posCommand++;
                _lstCommand.Add(new ComandoLog()
                {
                    IdComandoLog = i + posHist,
                    DataFila = new DateTime(2017, 05, 05, 17, 08, 29),
                    DataEnvio = new DateTime(2017, 05, 05, 17, 08, 29),
                    DataFinalizacao = new DateTime(2017, 05, 05, 17, 08, 29),
                    IdSaida = (i + posHist) + 1,
                    NomeSaida = "Saida",
                    UnidadeRastreada = "Camila de Jesus Ferreira",
                    NomeComando = "Saida",
                    RefComando = AppResources.Ativar,
                    Status = 1,
					UsuarioEnvio = "Bruno Daniel",
					StatusComando = "StatusComando"
                });

                posCommand++;
                _lstCommand.Add(new ComandoLog()
                {
                    IdComandoLog = i + posHist,
                    DataFila = new DateTime(2017, 05, 05, 17, 07, 29),
                    DataEnvio = new DateTime(2017, 05, 05, 17, 07, 29),
                    DataFinalizacao = new DateTime(2017, 05, 05, 17, 07, 29),
                    IdSaida = (i + posHist) + 1,
                    NomeSaida = "Porta Malas",
                    UnidadeRastreada = "New Eco Sport",
                    NomeComando = "Trava da Mala",
                    RefComando = AppResources.Ativar,
                    Status = 2,
					UsuarioEnvio = "Renan Fralda",
					StatusComando = "StatusComando"
                });

                posCommand++;
                _lstCommand.Add(new ComandoLog()
                {
                    IdComandoLog = i + posHist,
                    DataFila = new DateTime(2017, 05, 05, 17, 06, 29),
                    DataEnvio = new DateTime(2017, 05, 05, 17, 06, 29),
                    DataFinalizacao = new DateTime(2017, 05, 05, 17, 06, 29),
                    IdSaida = (i + posHist) + 1,
                    NomeSaida = "Saida",
                    UnidadeRastreada = "Uno Way",
                    NomeComando = "",
                    RefComando = AppResources.Ativar,
                    Status = 2,
					UsuarioEnvio = "Guigui",
					StatusComando = "StatusComando"

                });

                posCommand++;
                _lstCommand.Add(new ComandoLog()
                {
                    IdComandoLog = i + posHist,
                    DataFila = new DateTime(2017, 05, 05, 17, 05, 29),
                    DataEnvio = new DateTime(2017, 05, 05, 17, 05, 29),
                    DataFinalizacao = new DateTime(2017, 05, 05, 17, 05, 29),
                    IdSaida = (i + posHist) + 1,
                    NomeSaida = "Abertura Capô",
                    UnidadeRastreada = "Tiida Pombo",
                    NomeComando = "",
                    RefComando = AppResources.Ativar,
                    Status = 1,
					UsuarioEnvio = "Mychelle Filé",
					StatusComando = "StatusComando"
                });

                posHist++;
            }
            #endregion

        }

        public async Task<ServiceResult<Token>> Login(
			string paramUsuario
			, string paramSenha
			, string paramHash
            , string paramIdAplicacao
            , string paramIdentificacao
            , string paramIdSistemaOperacional
            , CancellationToken paramToken
		)
		{

			ServiceResult<Token> result = new ServiceResult<Token>();

			await Task.Delay(delay);
			try
			{
				result.Data = new Token()
				{
					Access_token = "Bl7Chqvyob2lAI8iAeuyiS+85pFwKTt/wl8ytytiS7eGrRp",
					LstFuncao = "|361|385|386|387|388|389|390|391" +
						"|392|393|394|395|396|397|",
					NomeCliente = "Systemsat",
					NomeUsuario = "Alexandre G."
				};
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

        public async Task<ServiceResult<bool?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<bool?> result = new ServiceResult<bool?>();

            try
            {
                result.Data = null;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception)
            {
                result.MessageError = "Exception";

            }
            return result;
        }

        public async Task<ServiceResult<Posicao>> GetPosition(
			long paramIdPosicao
			, Int32 paramUnidadeRatreada
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        )
		{
			ServiceResult<Posicao> result = 
				new ServiceResult<Posicao>();

			await Task.Delay(delay);
			try
			{
				result.Data = _detalhePosicao1;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<List<PainelDto>>> ListPainel(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<PainelDto>> result = 
				new ServiceResult<List<PainelDto>>();

			await Task.Delay(delay);
			try
			{
				result.Data = _lstPainel;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<List<PosicaoHistorico>>> 
		ListTracedUnitsWithPosition(CancellationToken paramToken)
		{
			ServiceResult<List<PosicaoHistorico>> result = 
				new ServiceResult<List<PosicaoHistorico>>();

			await Task.Delay(delay);
			try
			{
				result.Data = _lstForListTracedUnits;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

        public async Task<ServiceResult<List<PosicaoHistorico>>>
		ListTracedUnitsWithAlert(CancellationToken paramToken)
        {
            ServiceResult<List<PosicaoHistorico>> result =
                new ServiceResult<List<PosicaoHistorico>>();

            await Task.Delay(delay);
            try
            {
                result.Data = _lstForListTracedUnits;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception)
            {
                result.MessageError = "Exception";

            }

            return result;
        }


        public async Task<ServiceResult<List<PosicaoHistorico>>>
		ListTracedUnitHistory(
			Int32 paramId
			, String paramData
			, Int32 paramPeriodo
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        )
        {
			ServiceResult<List<PosicaoHistorico>> result =
				new ServiceResult<List<PosicaoHistorico>>();

            await Task.Delay(delay);
            try
            {
                result.Data = _lstForListHistorico;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception)
            {
                result.MessageError = "Exception";

            }

            return result;
        }

        public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
		)
        {
            ServiceResult<AncoraAtivacaoDto> result =
                new ServiceResult<AncoraAtivacaoDto>();

            await Task.Delay(delay);
            try
            {
                result.Data = new AncoraAtivacaoDto();
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception)
            {
                result.MessageError = "Exception";

            }

            return result;
        }

        public async Task<ServiceResult<Int32>> DesativarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
		)
        {
            ServiceResult<Int32> result =
                new ServiceResult<Int32>();

            await Task.Delay(delay);
            try
            {
                result.Data = new Int32();
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception)
            {
                result.MessageError = "Exception";

            }

            return result;
        }

		public async Task<ServiceResult<List<ComandoLog>>> ListCommandLog(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<ComandoLog>> result =
				new ServiceResult<List<ComandoLog>>();

			await Task.Delay(delay);
			try
			{
				result.Data = _lstCommand;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<List<ComandoParametroDto>>> ListCommandParameter(
			int paramIdRastreador
			, int paramIdComando
			, CancellationToken paramToken
		)
		{
			ServiceResult<List<ComandoParametroDto>> result =
				new ServiceResult<List<ComandoParametroDto>>();

			await Task.Delay(delay);
			try
			{
				List<ComandoParametroDto> lst = new List<ComandoParametroDto>();

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 33,
					Label = "Intervalo de transmissão em pânico (seg)",
					ToolTip = "Intervalo em segundos para transmissão da " +
						"posição com o pânico acionado",
					IdTipoParametro = (int)EnumTipoParametro.TextboxLivre,
					Valor = "",
					TamanhoMaximo = 5,
					Decimals = null,
					Dominio = null,
					Ordem = 0
				});

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 34,
					Label = "Tempo de espera pelo ACK (seg)",
					ToolTip = "Tempo máximo em segundos de espera pela " +
						"resposta de envio da posição",
					IdTipoParametro = (int)EnumTipoParametro.TextboxLivre,
					Valor = "",
					TamanhoMaximo = 5,
					Decimals = null,
					Dominio = "10>60",
					Ordem = 0
				});

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 35,
					Label = "Intervalo de transmissão em pânico (seg)",
					ToolTip = "Intervalo em segundos para transmissão " +
						"da posição com o pânico acionado",
					IdTipoParametro = (int)EnumTipoParametro.TextboxNumeroInteiro,
					Valor = "20",
					TamanhoMaximo = 5,
					Decimals = 3,
					Dominio = "0>65535",
					Ordem = 0
				});

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 36,
					Label = "Intervalo de transmissão ligado",
					ToolTip = "Intervalo de transmissão ligado",
					IdTipoParametro = (int)EnumTipoParametro.TextboxNumeroDecimal,
					Valor = "180",
					TamanhoMaximo = 5,
					Decimals = 0,
					Dominio = "10>500000",
					Ordem = 0
				});

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 37,
					Label = "Modo de Ignição",
					ToolTip = "",
					IdTipoParametro = (int)EnumTipoParametro.ComboboxGenerico,
					Valor = "0",
					TamanhoMaximo = null,
					Decimals = null,
					Dominio = "0|Ignição Virual;1|Ignição Física",
					Ordem = 0
				});

				lst.Add(new ComandoParametroDto()
				{
					IdParametro = 38,
					Label = "Configuração das informações que serão enviadas",
					ToolTip = "Configuração das informações que serão enviadas " +
						"na mensagem de posição",
					IdTipoParametro = (int)EnumTipoParametro.ListCheckboxGenerico,
					Valor = "10111110",
					TamanhoMaximo = null,
					Decimals = null,
					Dominio = "Habilitar Informações de Ponto Embarcado e " +
						"Acelerômetro;Habilitar pacote de transmissão de " +
						"acessórios sem fio;Habilitar informações gerais;" +
						"Habilitar Informações de hodômetro;Habilitar " +
						"Informações de horímetro;Habilitar motivo da geração " +
						"de pacotes de posição;Habilitar informações detalhadas " +
						"de tensão;Habilitar ID do condutor",
					Ordem = 0
				});
				
				result.Data = lst;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<List<ComandoDto>>> ListCommand(
			int paramIdRastreador
			, CancellationToken paramToken
		)
		{
			ServiceResult<List<ComandoDto>> result =
				new ServiceResult<List<ComandoDto>>();

			await Task.Delay(delay);
			try
			{
				List<ComandoDto> lst = new List<ComandoDto>();

				lst.Add(new ComandoDto()
				{
					ID = "SAI_1",
					IdObjeto = 1,
					Nome = "Saida Bloqueio",
					TipoAtuador = 1
				});

				lst.Add(new ComandoDto()
				{
					ID = "CSP_1",
					IdObjeto = 2,
					Nome = "Comando Sirene",
					TipoAtuador = 2
				});

				lst.Add(new ComandoDto()
				{
					ID = "CCP_1",
					IdObjeto = 3,
					Nome = "Comando Parametro",
					TipoAtuador = 3
				});
				
				result.Data = lst;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<StatusComandoDto>> SendCommand(
			int paramIdTracker
			, int paramOrder
			, int paramIdCommand
			, string paramLstParametresCommand
			, CancellationToken paramToken
		)
		{
			ServiceResult<StatusComandoDto> result =
				new ServiceResult<StatusComandoDto>();

			await Task.Delay(delay);
			try
			{
				result.Data = new StatusComandoDto()
				{
					IdStatusComando = 0
				};
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

		public async Task<ServiceResult<HistoricoComandoRespost>> ListCommandHistory
		(
			int paramPage
			, int paramPageSize
			, CancellationToken paramToken
		)
		{
			ServiceResult<HistoricoComandoRespost> result =
				new ServiceResult<HistoricoComandoRespost>();

			await Task.Delay(delay);
			try
			{
				PainelDto comandos = new PainelDto();
				comandos.Chave = "HistoricoComando";
				comandos.HasDetalhes = true;
				comandos.LastSearch = DateTime.UtcNow;
				comandos.Grafico = new List<GraficoDto>();
				comandos.Total = 10;

				double percentTotalComandoAguardando = 30;
				double percentTotalComandoFalha = 30;
				double percentTotalComandoSucesso = 40;

				String cor2Barra = "#00ac99";
				String cor2Fundo = "#008387";

				String cor3Barra = "#ec6968";
				String cor3Fundo = "#C24A4A";

				String cor4Barra = "#E6A64E";
				String cor4Fundo = "#CC8739";

				comandos.Grafico.Add(new GraficoDto()
				{
					porcento = Math.Round(percentTotalComandoSucesso, 2),
					corBarra = cor2Barra,
					corFundo = cor2Fundo,
					Identificacao = "ComandoSucesso",
					Status = 0,
					parent = 3
				});
				comandos.Grafico.Add(new GraficoDto()
				{
					porcento = Math.Round(percentTotalComandoFalha, 2),
					corBarra = cor3Barra,
					corFundo = cor3Fundo,
					Identificacao = "ComandoFalha",
					Status = 2,
					parent = 3
				});
				comandos.Grafico.Add(new GraficoDto()
				{
					porcento = Math.Round(percentTotalComandoAguardando, 2),
					corBarra = cor4Barra,
					corFundo = cor4Fundo,
					Identificacao = "ComandoAguardando",
					Status = 1,
					parent = 3
				});
				
				result.Data = new HistoricoComandoRespost()
				{
					Painel = comandos,
					ListComando = _lstCommand
				};
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken,
            int paramIdAplicativo
        )
        {
            ServiceResult<String> result = new ServiceResult<String>();

            return result;
        }

        public async Task<ServiceResult<bool>> Atualiza_PushKey(
			int paramIdAplicativo
			, string paramPushKey
			, CancellationToken paramToken
		)
		{
			await Task.Delay(delay);
			ServiceResult<bool> result = new ServiceResult<bool>();

			try
			{
				result.Data = true;
			}
			catch (HttpRequestException)
			{
				result.MessageError = "HttpRequestException";
			}
			catch (Exception)
			{
				result.MessageError = "Exception";

			}

			return result;
		}

	}
}
