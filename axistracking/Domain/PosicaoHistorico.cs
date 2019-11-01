using System;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Domain
{
    public class PosicaoHistorico
    {
        public Int64 IdPosicao { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longitude { get; set; }
        public Double? Velocidade { get; set; }
        public Byte? IdTipoUnidadeRastreada { get; set; }
        public DateTime? DataEvento { get; set; }
        public string NomeRegraViolada { get; set; }
        public string CorRegraPrioritaria { get; set; }
        public Boolean? Ignicao { get; set; }
        public Int32 IdUnidadeRastreada { get; set; }
        public String Identificacao { get; set; }
        public Byte Status { get; set; }
        public String ResponsavelUnidadeRastreada { get; set; }
        public string IconUrl { get; set; }
        public Byte? OrdemRastreador { get; set; }

        public Double? Ancora_Latitude { get; set; }
        public Double? Ancora_Longitude { get; set; }
        public Int32? Ancora_Tolerancia { get; set; }

        public String Endereco { get; set; }

        public BitmapDescriptor MontaIconPin()
        {
            //if(String.IsNullOrWhiteSpace(IconUrl))
            //{
            return GetDefaultIcon();
            //}
            //else
            //{
            //	BitmapDescriptor bmp = null;
            //	try
            //	{
            //		using (HttpClient client = new HttpClient())
            //		{
            //			Uri ur = new Uri(IconUrl);
        }

        private BitmapDescriptor GetDefaultIcon()
        {
            String iconePrincipal = "";
            switch (this.IdTipoUnidadeRastreada)
            {
                case 2:
                    iconePrincipal = "pin_ultima_posicao_cel.png";
                    break;
                default:
                    iconePrincipal = "pin_ultima_posicao_carro.png";
                    break;
            }
            return BitmapDescriptorFactory.FromBundle(iconePrincipal);
        }

        public Boolean ExibeUltimaPosicao { get; set; }

        public Byte? StatusRastreadorUnidadeRastreada { get; set; }


        private String _identificacaoUnidadeRastreada;
        public String IdentificacaoUnidadeRastreada
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_identificacaoUnidadeRastreada))
                {
                    _identificacaoUnidadeRastreada = Identificacao + " (" + (OrdemRastreador + 1).ToString() + ")";
                }
                return _identificacaoUnidadeRastreada;
            }
        }

        public string IconePadrao { get; set; }


    }
}
