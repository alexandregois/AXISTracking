using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;

namespace axistracking.Model
{
	public class ModelHistorico: ModelBase
    {

        public async Task<ServiceResult<List<PosicaoHistorico>>> ListHistoricoUnidadesAsync(
			Int32 paramId
			, DateTime? paramData
			, Int32 paramPeriodo
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        )
        {
			ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();

            try
            {
				String strData = null;

				if(paramData.HasValue)
				{
                    strData = paramData.Value.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss");
                    //strData = paramData.Value.ToString("yyyy/MM/dd HH:mm:ss");

                }

                result = await DataStore.ListTracedUnitHistory(
					paramId
					, strData
					, paramPeriodo
					, paramOrdemRastreador
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
