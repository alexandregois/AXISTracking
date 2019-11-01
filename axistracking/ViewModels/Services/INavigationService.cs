using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Enum;

namespace axistracking.ViewModels.Services
{
	public interface INavigationService
    {
		void Voltar();
		void NavigateToLogin();
		void NavigateToDetalhes(PainelDto painel, object obj, EnumPage Page);
		void NavigateToPainel();
        void NavigateToModulos();
        void NavigateToHistorico(
			PainelDto painel
			, object obj
			, EnumPage Page
		);

		void NavigateToPosicao(PainelDto Painel, object obj, EnumPage Page);

		void NavigateToComandos(Posicao paramIdRastreador);		

        void NavigateToListarComandos(
			object obj
			, EnumPage Page
			, string paramNomeUnidade = ""
		);

		void NavigateToComandoHistorico();

		void NavigateComandosParaNaoProcessado(string paramNomeUnidade);
    }
}
