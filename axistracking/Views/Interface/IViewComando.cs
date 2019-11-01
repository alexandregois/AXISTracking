using System.Collections.Generic;
using axistracking.Domain.Dto;

namespace axistracking.Views.Interface
{
	public interface IViewComando
	{
		void ExibirLoad();
		void EscondeLoad();
		List<ComandoParametroDto> RecuperaValor();
		void LimpaComandos();
	}
}
