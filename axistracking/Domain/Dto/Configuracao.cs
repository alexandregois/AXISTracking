using System;

namespace axistracking.Domain.Dto
{
    public class Configuracao
    {
        public String CodigoEmpresa { get; }
        public String URLStringWS { get; }

        public Boolean UseMockDataStore; //PARA MOCAR = true

        public TimeSpan RefreshTime { get; }

        public Configuracao()
        {

#if DEBUG

            //UseMockDataStore = true;

            //URLStringWS = "http://10.1.3.124:81"; // Homologação
            //URLStringWS = "http://10.10.3.13:2826"; // Desenv

            //URLStringWS = "http://10.10.6.166:10000"; // Desenv PC Pombo//
            //URLStringWS = "http://10.10.3.160:2826"; // Desenv PC José Carlos//

            //URLStringWS = "http://10.10.3.209:2826"; // Desenv PC Pombo//
           
            URLStringWS = "http://service.systemsatx.com.br"; //IP Externo

            //URLStringWS = "http://200.152.54.164:81"; //Ip Homologação Externo

            //URLStringWS = "http://service.systemsatx.com.br:94"; //IP Externo

#else

            UseMockDataStore = false;
            URLStringWS = "http://service.systemsatx.com.br"; //IP Externo

            //URLStringWS = "http://200.152.54.164:81"; //Ip Homologação Externo
            //URLStringWS = "http://10.10.6.166:2826"; // Desenv PC Pombo//

#endif

            RefreshTime = TimeSpan.FromMinutes(1);

        }
    }
}
