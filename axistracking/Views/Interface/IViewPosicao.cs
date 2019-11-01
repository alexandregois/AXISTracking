using System;
using axistracking.Domain;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Views.Interface
{
    public interface IViewPosicao
    {
        Map mapaPosicao { get; set; }

        void EscondeLoad();
        void ExibirLoad();
        void MontaDetalheTopoPosicao(PosicaoHistorico paramPosicao);
        void MontaMapa();
        void ExibeTitulo();

		void MontaStreetView(Double paramLatitude, Double paramLongitude);

	}
}
