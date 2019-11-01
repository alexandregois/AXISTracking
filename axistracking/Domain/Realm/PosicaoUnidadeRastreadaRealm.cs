using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Domain.Realm
{
    public class PosicaoUnidadeRastreadaRealm
    {

        #region Posicao
        public Int64 IdPosicao { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longitude { get; set; }
        public DateTime? DataEvento { get; set; }
        public String Endereco { get; set; }
        public Double? Velocidade { get; set; }
        public Boolean? Ignicao { get; set; }
        public Boolean? GPSValido { get; set; }
        public Int32? IdRegraPrioritaria { get; set; }
        public Double? BateriaPrincipal { get; set; } //Novo
        public Double? BateriaBackup { get; set; } //Novo
        public Double? Altitude { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public String Evento { get; set; }
        public string CorRegraPrioritaria { get; set; }
        public Double? SinalGPRS { get; set; }
        public Double? Odometro { get; set; }
        public Byte OrdemRastreador { get; set; }
        public Dictionary<string, bool> Sensores { get; set; }
        public Dictionary<string, bool> Atuadores { get; set; }
        public Dictionary<string, double> Telemetrias { get; set; }
        public Dictionary<string, bool> Informacoes { get; set; }
        public string NomeRegraViolada { get; set; }
        public DateTime? DataGPS { get; set; }
        public Byte StatusFiltro { get; set; }

        #endregion

        #region UnidadeRastreada
        public Int32? IdUnidadeRastreada { get; set; }
        public int IdRastreador { get; set; }
        public Int32? IdRastreadorUnidadeRastreada { get; set; }
        public Byte? IdTipoUnidadeRastreada { get; set; }
        public String Identificacao { get; set; }
        public String Placa { get; set; }
        public DateTime? DataEvento_UnidadeRastreada { get; set; }
        public string ModeloVeiculo { get; set; }
        public string CategoriaModelo { get; set; }
        public string GrupoUnidadeRastreada { get; set; }
        #endregion

        #region Ancora
        public Int32? Ancora_IdGeography { get; internal set; }
        public Double? Ancora_Latitude { get; set; }
        public Double? Ancora_Longitude { get; set; }
        public Int32? Ancora_Tolerancia { get; set; }
        #endregion

    }
}
