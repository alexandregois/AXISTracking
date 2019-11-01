using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain.Dto;

namespace axistracking.Model
{
	public class ModelPainel : ModelBase
	{

		public async Task<ServiceResult<List<PainelDto>>> ListPainelAsync(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<PainelDto>> result = new ServiceResult<List<PainelDto>>();

			try
			{
				result = await DataStore.ListPainel(
					paramToken
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
