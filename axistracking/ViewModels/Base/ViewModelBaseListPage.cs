using System;
using System.Windows.Input;
using axistracking.Domain.Dto;
using Xamarin.Forms;

namespace axistracking.ViewModels.Base
{
	public abstract class ViewModelBaseListPage : ViewModelBase
	{

		public ICommand BuscarCommand
		{
			get;
			set;
		}

		private string txtBuscar;
		public string TxtBuscar
		{
			get
			{
				return txtBuscar;
			}
			set
			{
				txtBuscar = value;
				this.Notify("TxtBuscar");
			}
		}



		private Boolean _listEndRefresh = false;
		public Boolean ListEndRefresh
		{
			get
			{
				return _listEndRefresh;
			}
			set
			{
				_listEndRefresh = value;
				this.Notify("ListEndRefresh");
			}
		}

		private Double _painelFiltroTextHeight = 57;
		public Double PainelFiltroTextHeight
		{
			get
			{
				return _painelFiltroTextHeight;
			}
			set
			{
				_painelFiltroTextHeight = value;
				this.Notify("PainelFiltroTextHeight");
			}
		}

		private Double _listPainelTop_HeightPadrao = 60;
		public Double ListPainelTop_HeightPadrao
		{
			get
			{
				return _listPainelTop_HeightPadrao;
			}
			set
			{
				_listPainelTop_HeightPadrao = value;
				this.Notify("ListPainelTop_HeightPadrao");
			}
		}

		private Double _listPainelTop_Height;
		public Double ListPainelTop_Height
		{
			get
			{
				if(_listPainelTop_Height > 0)
					return _listPainelTop_Height;
				else
					return ListPainelTop_HeightPadrao;
			}
			set
			{
				_listPainelTop_Height = value;
				MudarTamanhoLoad();
				this.Notify("ListPainelTop_Height");
			}
		}

		PainelDto _listPainelTop_Source;
		public PainelDto ListPainelTop_Source
		{
			get
			{
				return _listPainelTop_Source;
			}
			set
			{
				_listPainelTop_Source = value;
				this.Notify("ListPainelTop_Source");
			}
		}

		private Double _listPainelTopColumn_Width;
		public Double ListPainelTopColumn_Width
		{
			get
			{
				return _listPainelTopColumn_Width;
			}
			set
			{
				_listPainelTopColumn_Width = value;
				this.Notify("ListPainelTopColumn_Width");
			}
		}

		//List
		public ICommand List_RefreshCommand
		{
			get;
			set;
		}

		private Double _list_Height;
		public Double List_Height
		{
			get
			{
				return _list_Height;
			}
			set
			{
				_list_Height = value;
				this.Notify("List_Height");
			}
		}

		Boolean _list_IsRefreshing;
		public Boolean List_IsRefreshing
		{
			get
			{
				return _list_IsRefreshing;
			}
			set
			{
				_list_IsRefreshing = value;
				ListEndRefresh = !value;
				this.Notify("List_IsRefreshing");
			}
		}

		Boolean _list_IsVisible = true;
		public Boolean List_IsVisible
		{
			get
			{
				return _list_IsVisible;
			}
			set
			{
				_list_IsVisible = value;
				this.Notify("List_IsVisible");
			}
		}

		Boolean _listPainelTop_IsVisible = true;
		public Boolean ListPainelTop_IsVisible
		{
			get
			{
				return _listPainelTop_IsVisible;
			}
			set
			{
				_listPainelTop_IsVisible = value;
				this.Notify("ListPainelTop_IsVisible");
			}
		}

		#region Filtro itens
		public Byte? StatusFiltro { get; set; }
		#endregion

		public ViewModelBaseListPage()
		{
			ListPainelTop_Height = _listPainelTop_HeightPadrao;

			ListPainelTopColumn_Width = DefaultWidth / (double)2;
			
			this.List_RefreshCommand = new Command(this.ListRefresh);

			this.BuscarCommand = new Command(() => this.Buscar());
		}


		public abstract void ListRefresh(object obj);
		public abstract void Buscar();
		public abstract void MudarTamanhoLoad();
		public abstract void ExibeTitulo(Int32? paramTotal);

	}
}
