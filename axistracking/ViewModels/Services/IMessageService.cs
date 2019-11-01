using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.ViewModels.Services
{
    public interface IMessageService
    {
		Task ShowAlertAsync (
			String paramMessage
			, String paramTitulo = "Ssx Tracking"
			, String paramOK = "OK"
		);

		Task<Boolean> ShowAlertChooseAsync (
			String paramMessage
			, String paramCancel
			, String paramOK
			, String paramTitulo = "Ssx Tracking"
		);

		Task<String> ShowMessageAsync(
			string titulo
			, string paramCancel
			, string paramDestruction
			, string[] paramButtons
		);
    }
}
