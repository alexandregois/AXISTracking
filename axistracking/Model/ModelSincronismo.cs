using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain.Dto;
using axistracking.Model;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace axistracking.Model
{
	public class ModelSincronismo : ModelBase
	{
		public ModelSincronismo()
			: base()
		{
		}

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(
            Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken,
            int paramIdAplicativo)
        {
            ServiceResult<String> result = new ServiceResult<String>();
            try
            {
                result = await DataStore.RetornaObjetoKeepAlive(paramIdObjetoKeepAlive, paramToken, paramIdAplicativo);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                result.MessageError = "Exception";
            }

            return result;

        }



    }
}
