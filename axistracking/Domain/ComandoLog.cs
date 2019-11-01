using System;
using axistracking.Resx;

namespace axistracking.Domain.Dto
{
	public class ComandoLog
    {
        public Int32 IdComandoLog { get; set; }
        public String NomeComando { get; set; }
        public String RefComando { get; set; }
        public String NomeSaida { get; set; }
        public DateTime DataFila { get; set; }
        public String StatusComando { get; set; }
        public String UnidadeRastreada { get; set; }
        public Byte Ordem { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public String UsuarioEnvio { get; set; }
        public String ClienteEnvio { get; set; }
        public Int32? IdSaida { get; set; }
        public Byte? Status { get; set; }

		private String _identificacaoComando;
		public String IdentificacaoComando
		{
			get
			{
				if(String.IsNullOrWhiteSpace(_identificacaoComando))
				{
					_identificacaoComando = String.Empty;

					if (NomeComando != null)
						_identificacaoComando += NomeComando;
					else
						_identificacaoComando += AppResources.ResourceManager.GetString(RefComando);

					if(!String.IsNullOrWhiteSpace(NomeSaida))
						_identificacaoComando += "(" + NomeSaida + " " + IdSaida + ")";
				}

				return _identificacaoComando;
			}
		}

		private String _identificacaoUnidadeRastreada;
		public String IdentificacaoUnidadeRastreada
		{
			get
			{
				if(String.IsNullOrWhiteSpace(_identificacaoUnidadeRastreada))
				{
					_identificacaoUnidadeRastreada = UnidadeRastreada + " (" + (Ordem+ 1).ToString() + ")";
				}
				return _identificacaoUnidadeRastreada;
			}
		}

    }
}
