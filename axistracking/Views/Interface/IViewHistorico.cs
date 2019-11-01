using System;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Views.Interface
{
	public interface IViewHistorico
    {
        Map mapaHistorico { get; set; }
        Boolean exibeBuscarMais { get; set; }
		void EscondeLoad();
		void ExibirLoad();

    }
}
