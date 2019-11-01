using System;
using System.Threading.Tasks;
using axistracking.ViewModels.Services;
using Xamarin.Forms;

namespace axistracking.Views.Services
{
	public class MessageService : IMessageService
	{

		public async Task ShowAlertAsync (
			String paramMessage
			, String paramTitulo = "Ssx Tracking"
			, String paramOK = "OK"
		)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await Application
					.Current.MainPage.DisplayAlert
					(
						paramTitulo
						, paramMessage
						, paramOK
					);
			});
		}

		public async Task<Boolean> ShowAlertChooseAsync (
			String paramMessage
			, String paramCancel
			, String paramOK
			, String paramTitulo = "Ssx Tracking"
		)
		{
			TaskCompletionSource<bool> ret = new TaskCompletionSource<bool>();
			Device.BeginInvokeOnMainThread(async () =>
			{
				Boolean alertRet = await Application
					.Current.MainPage.DisplayAlert
					(
						paramTitulo
						, paramMessage
						, paramOK
						, paramCancel
					);
				ret.SetResult(alertRet);
			});

			return await ret.Task;
		}

        public async Task<String> ShowMessageAsync(
			string titulo
			, string paramCancel
			, string paramDestruction
			, string[] paramButtons
		)
		{

			TaskCompletionSource<String> ret = new TaskCompletionSource<String>();
			Device.BeginInvokeOnMainThread(async () => 
			{
				String alertRet = await Application
					.Current.MainPage.DisplayActionSheet
					(
						titulo
						, paramCancel
						, paramDestruction
						, paramButtons
					);
				ret.SetResult(alertRet);
			});
			return await ret.Task;
        }
	}
}

