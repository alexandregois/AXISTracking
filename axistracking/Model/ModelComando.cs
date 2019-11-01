using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain.Dto;

namespace axistracking.Model
{
	public class ModelComando: ModelBase
    {
		public async Task<ServiceResult<List<ComandoLog>>> ListComandosEnviadosAsync(
			CancellationToken paramToken
		)
        {
            ServiceResult<List<ComandoLog>> result = new ServiceResult<List<ComandoLog>>();

            try
            {
				result = await DataStore.ListCommandLog(paramToken);
            }
            catch (Exception)
            {
                result.IsValid = false;
                result.MessageError = "ErroInesperado";
            }

            return result;
        }

		public async Task<ServiceResult<List<ComandoDto>>> ListComandoRastreador(
			Int32 paramIdRastreador
			, CancellationToken paramToken
		)
		{
			ServiceResult<List<ComandoDto>> result = new ServiceResult<List<ComandoDto>>();

			try
			{
				result = await DataStore.ListCommand(
					paramIdRastreador
					, paramToken
				);
			}
			catch (Exception)
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";
			}

			return result;
		}

		public async Task<ServiceResult<List<ComandoParametroDto>>> ListParametros(
			int paramIdRastreador
			, int paramIdComando
			, CancellationToken paramToken
		)
		{
			ServiceResult<List<ComandoParametroDto>> result = new ServiceResult<List<ComandoParametroDto>>();

			try
			{
				result = await DataStore.ListCommandParameter(
					paramIdRastreador
					, paramIdComando
					, paramToken
				);

			}
			catch (Exception)
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";
			}

			return result;
		}

		public async Task<ServiceResult<StatusComandoDto>> SendCommand(
			int paramIdRastreador
			, byte? paramOrdemRastreador
			, int? paramIdRastreadorUnidadeRastreada
			, int paramIdComando
			, string paramLstParametro
			, CancellationToken paramToken
		)
		{
			ServiceResult<StatusComandoDto> result = new ServiceResult<StatusComandoDto>();

			try
			{
				result = await DataStore.SendCommand(
					paramIdRastreador
					, paramOrdemRastreador.Value
					, paramIdComando
					, paramLstParametro
					, paramToken
				);

			}
			catch (Exception ex)
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";
			}

			return result;
		}

        public async Task<ServiceResult<Boolean>> ComandoCancelar(Int32 paramIdCommandLog
    , CancellationToken paramToken
)
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            try
            {

                result = await DataStore.ComandoCancelar(
                    paramIdCommandLog
                    , paramToken
                );
            }
            catch
            {
                result.MessageError = "Exception";
            }

            return result;
        }


        public async Task<ServiceResult<HistoricoComandoRespost>> ListCommandHistory(
			int paramPage
			, int paramPageSize
			, CancellationToken paramToken
		)
		{
			ServiceResult<HistoricoComandoRespost> result = new ServiceResult<HistoricoComandoRespost>();

			try
			{
				result = await DataStore.ListCommandHistory(
					paramPage
					, paramPageSize
					, paramToken
				);
			}
			catch (Exception)
			{
				result.IsValid = false;
				result.MessageError = "ErroInesperado";
			}

			return result;
		}

	}
}
