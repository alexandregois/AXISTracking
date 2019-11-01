using System;
namespace axistracking.Domain.Dto
{
	public class ComandoDto
	{

		public Int32 TipoAtuador { get; set; }
		public String ID { get; set; }
		public Int32 IdObjeto { get; set; }
		public String Nome { get; set; }

		public void Clone(ComandoDto paramComando)
		{
			this.TipoAtuador = paramComando.TipoAtuador;
			this.ID = paramComando.ID;
			this.IdObjeto = paramComando.IdObjeto;
			this.Nome = paramComando.Nome;
		}
	}
}
