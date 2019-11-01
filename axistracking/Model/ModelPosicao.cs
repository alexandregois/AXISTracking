using System;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;

namespace axistracking.Model
{
	public class ModelPosicao : ModelBase
	{
		public async Task<ServiceResult<Posicao>> Get(
			Int64? paramIdPosicao
			, Int32 paramUnidadeRastreada
			, Byte? paramOrdemRastreador
			, CancellationToken paramToken
        )
		{
			ServiceResult<Posicao> result = new ServiceResult<Posicao>();

			try
			{
				if(!paramIdPosicao.HasValue)
					paramIdPosicao = 0;
				
				result = await DataStore.GetPosition(
					paramIdPosicao.Value
					, paramUnidadeRastreada
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

        public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
        )
        {
            ServiceResult<AncoraAtivacaoDto> result = new ServiceResult<AncoraAtivacaoDto>();

            try
            {
                result = await DataStore.AtivarAncora(
					paramPosicao
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

        public async Task<ServiceResult<Int32>> DesativarAncora(
			PosicaoHistorico paramPosicao
			, CancellationToken paramToken
        )
        {
            ServiceResult<Int32> result = new ServiceResult<Int32>();

            try
            {
                result = await DataStore.DesativarAncora(
					paramPosicao
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
