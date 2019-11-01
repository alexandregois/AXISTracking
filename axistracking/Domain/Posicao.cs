using System;
using System.Collections.Generic;

namespace axistracking.Domain
{
    public class Posicao
    {
		public Int64? IdPosicao { get; set; }

		public Double? Latitude { get; set; }
		public Double? Longitude { get; set; }

		public String Endereco { get; set; }
		public Double? Velocidade { get; set; }
		public Boolean? Ignicao { get; set; }
		public Boolean? GPSValido { get; set; }
		public Int32? IdRegraPrioritaria { get; set; }
		public string CorRegraPrioritaria { get; set; }
		public String NomeRegraViolada { get; set; }
		public String Evento { get; set; }

		public Int32? IdUnidadeRastreada { get; set; }
		public Int32? IdRastreadorUnidadeRastreada { get; set; }
		public Byte? IdTipoUnidadeRastreada { get; set; }
		public String Identificacao { get; set; }
		public double? SinalGPRS { get; set; }
		public Byte? OrdemRastreador { get; set; }
		public int IdRastreador { get; set; }
		public Double? Ancora_Latitude { get; set; }
		public Double? Ancora_Longitude { get; set; }
		public Int32? Ancora_Tolerancia { get; set; }

		public string ModeloVeiculo { get; set; }
		public string CategoriaModelo { get; set; }
		public string GrupoUnidadeRastreada { get; set; }

		public DateTime? DataGPS { get; set; }
		public DateTime? DataAtualizacao { get; set; }
		public DateTime DataEvento { get; set; }
		public String ResponsavelUnidadeRastreada { get; set; }
		public String TempoIgnicao { get; set; }
		public Int32 IdStatusOperacional { get; set; }
		public Boolean? IsUltimaPosicao { get; set; }

		public Double? Odometro { get; set; }
		public Double? Horimetro { get; set; }
		public Double? TemperaturaInterna { get; set; }
		public Double? SensorTemperatura1 { get; set; }
		public Double? BateriaPrincipal { get; set; }
		public Double? BateriaBackup { get; set; }

		public Dictionary<string, bool> Sensores { get; set; }
		public Dictionary<string, bool> Atuadores { get; set; }
		public Dictionary<string, double> Telemetrias { get; set; }
		public Dictionary<string, bool> Informacoes { get; set; }

		private String _identificacaoUnidadeRastreada;
		public String IdentificacaoUnidadeRastreada
		{
			get
			{
				if(String.IsNullOrWhiteSpace(_identificacaoUnidadeRastreada))
				{
					_identificacaoUnidadeRastreada = Identificacao + " (" + (OrdemRastreador + 1).ToString() + ")";
				}
				return _identificacaoUnidadeRastreada;
			}
		}

    }
}
