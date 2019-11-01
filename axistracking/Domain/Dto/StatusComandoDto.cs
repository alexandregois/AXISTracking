using System;
namespace axistracking.Domain.Dto
{
	public class StatusComandoDto
	{
		public Int32? IdComandoLog { get; set; }
		public DateTime? DataFila { get; set; }
		public Byte? IdStatusComando { get; set; }
		public Int32? IdRastreador { get; set; }
		public Boolean IsBloqueado { get; set; }
	}
}
