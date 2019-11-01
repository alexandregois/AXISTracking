using System;
using Xamarin.Forms.GoogleMaps;

namespace axistracking.Views.Interface
{
	public interface IViewDetalhes
	{
		void EscondeLoad();
		void ExibirLoad();
		void MudarTamanhoLoad();
        void ExibeTitulo(Int32? paramTotal);
		Map MapaDetalhes { get; }
        void EscondePercentual();
    }
}
