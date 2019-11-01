using System.Collections.Generic;

namespace axistracking.Domain.Dto
{
	public class HistoricoComandoRespost
	{
		public PainelDto Painel { get; set; }
		public List<ComandoLog> ListComando { get; set; }
	}
}
