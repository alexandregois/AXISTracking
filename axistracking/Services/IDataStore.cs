using axistracking.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using axistracking.Domain;
using System.Threading;

namespace axistracking.Services
{
    public interface IDataStore
    {
        Task<ServiceResult<Token>> Login(
            string paramUsuario
			, string paramSenha
			, string paramHash
            , string paramIdAplicacao
            , string paramIdentificacao
            , string paramIdSistemaOperacional
            , CancellationToken paramToken
		);
		
		#region Posição
		Task<ServiceResult<Posicao>> GetPosition(
			Int64 paramIdPosicao
			, Int32 paramUnidadeRatreada
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        );
        #endregion

        Task<ServiceResult<bool>> ComandoCancelar(int paramIdCommandLog, CancellationToken paramToken);

        Task<ServiceResult<Boolean?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        );

        Task<ServiceResult<List<PosicaoHistorico>>> ListTracedUnitsWithPosition(
			CancellationToken paramToken
		);

		Task<ServiceResult<List<PosicaoHistorico>>> ListTracedUnitsWithAlert(
			CancellationToken paramToken
		);

		Task<ServiceResult<bool>> Atualiza_PushKey(
			int paramIdAplicativo
			, String paramPushKey
			, CancellationToken paramToken
		);

		Task<ServiceResult<List<PosicaoHistorico>>> ListTracedUnitHistory(
			Int32 paramId
			, String paramData
			, Int32 paramPeriodo
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        );

		Task<ServiceResult<List<PainelDto>>> ListPainel(
			CancellationToken paramToken
		);

        Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
		);

        Task<ServiceResult<Int32>> DesativarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
		);


        Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken,
            int paramIdAplicativo
        );

        Task<ServiceResult<List<ComandoLog>>> ListCommandLog(
			CancellationToken paramToken
		);

		Task<ServiceResult<List<ComandoParametroDto>>> ListCommandParameter(
			Int32 paramIdRastreador
			, Int32 paramIdComando
			, CancellationToken paramToken
		);

		Task<ServiceResult<List<ComandoDto>>> ListCommand(
			Int32 paramIdRastreador
			, CancellationToken paramToken
		);

		Task<ServiceResult<StatusComandoDto>> SendCommand(
			int paramIdTracker
			, int paramOrder
			, int paramIdCommand
			, String paramLstParametresCommand
			, CancellationToken paramToken
		);

		Task<ServiceResult<HistoricoComandoRespost>> ListCommandHistory
		(
			int paramPage
			, int paramPageSize
			, CancellationToken paramToken
		);

    }
}
