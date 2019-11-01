using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;

namespace axistracking.Model
{
	public class ModelDetalhes: ModelBase
    {

		public async Task<ServiceResult<List<PainelDto>>> ListPainelTopAsync(
			CancellationToken paramToken
		)
        {
            ServiceResult<List<PainelDto>> result = new ServiceResult<List<PainelDto>>();

            try
            {
				result = await DataStore.ListPainel(paramToken);
            }
            catch (Exception)
            {
                result.IsValid = false;
                result.MessageError = "ErroInesperado";
            }

            return result;
        }
        
        public async Task<ServiceResult<List<PosicaoHistorico>>> ListUnidadesAsync(
			CancellationToken paramToken
		)
        {
			ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();

            try
            {
				result = await DataStore.ListTracedUnitsWithPosition(paramToken);
            }
            catch (Exception)
            {
                result.IsValid = false;
                result.MessageError = "ErroInesperado";
            }

            return result;
        }

		public async Task<ServiceResult<List<PosicaoHistorico>>> ListAlertasAsync(
			CancellationToken paramToken
		)
        {
            ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();

            try
            {
				result = await DataStore.ListTracedUnitsWithAlert(paramToken);
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
