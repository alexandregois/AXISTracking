using System;
namespace axistracking.Domain.Dto
{
	public class ComandoParametroDto
	{
		public Int32 IdParametro { get; set; }
		public Int32 IdTipoParametro { get; set; }
		public String Valor { get; set; }

		public String Label { get; set; }
		public String ToolTip { get; set; }
		public Int32? TamanhoMaximo { get; set; }
		public Byte? Decimals { get; set; }
		public String Dominio { get; set; }
		public Int32 Ordem { get; set; }
	}
}
