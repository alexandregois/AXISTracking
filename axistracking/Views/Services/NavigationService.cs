using System;
using System.Threading.Tasks;
using axistracking.ViewModels.Services;
using axistracking.Enum;
using axistracking.Domain.Dto;
using Xamarin.Forms;
using System.Threading;
using axistracking.Domain;
using System.Linq;

namespace axistracking.Views.Services
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	public class NavigationService : INavigationService
	{
		private App _app => (Application.Current as App);

		public void Voltar()
		{
			Device.BeginInvokeOnMainThread(() =>
			{				
				Application.Current.MainPage.Navigation.PopAsync();
			});
		}
		
		public void NavigateToLogin ()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
			});
		}

		public void NavigateToDetalhes(PainelDto painel, object obj, EnumPage Page)
        {
			Device.BeginInvokeOnMainThread(() =>
			{
				Application.Current.MainPage.Navigation.PushAsync(new ViewDetalhes(obj, Page));
			});
        }

		public void NavigateToListarComandos(
			object obj
			, EnumPage Page
			, string paramNomeUnidade = ""
		)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
				Application.Current.MainPage.Navigation.PushAsync(
					new ViewListaComandos(
						obj
						, Page
						, paramNomeUnidade
					)
				);
            });
        }

        public void NavigateToPainel()
   		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_app.MainPage = new NavigationPage(new ViewPainel());
			});
        }

        public void NavigateToModulos()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _app.MainPage = new NavigationPage(new ViewModulos());
            });
        }

        public void NavigateToHistorico(
			PainelDto painel
			, object obj
			, EnumPage Page
		)
        {
			Device.BeginInvokeOnMainThread(() =>
			{
				Application.Current.MainPage.Navigation.PushAsync(new ViewHistorico(painel, obj));
			});
        }

		public void NavigateToPosicao(PainelDto Painel, object obj, EnumPage Page)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Application.Current.MainPage.Navigation.PushAsync(new ViewPosicao(Painel, obj, Page));
			});
		}

		public void NavigateToComandos(Posicao paramIdRastreador)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Application.Current.MainPage.Navigation.PushAsync(new ViewComando(paramIdRastreador));
			});
		}

		public void NavigateToComandoHistorico()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Application.Current.MainPage.Navigation.PushAsync(new ViewHistoricoComando());
			});
		}

		public void NavigateComandosParaNaoProcessado(string obj)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
                for (int i = 0; i < Application.Current.MainPage.Navigation.NavigationStack.Count; i++)
                {
                    if (Application.Current.MainPage.Navigation.NavigationStack[i].GetType() != typeof(ViewPainel))
                    {
                        Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[i]);

                    }
                }

                //foreach (Page item in Application.Current.MainPage.Navigation.NavigationStack)
                //{
                //    if (item.GetType() != typeof(ViewPainel))
                //    {
                //        Application.Current.MainPage.Navigation.RemovePage(item);
                //        //break;
                //    }
                //}

                NavigateToListarComandos(
					App.ListPainelSource.FirstOrDefault(x => x.Id == 3)
					, EnumPage.ListaComandos
					, obj
				);
			});
		}

	}
	#pragma warning restore CS4014
	#pragma warning restore RECS0022
}

