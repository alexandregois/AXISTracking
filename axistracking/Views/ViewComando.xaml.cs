using System;
using System.Collections.Generic;
using axistracking.CustomElements;
using axistracking.CustomElements.Cell;
using axistracking.Domain.Dto;
using axistracking.ViewModels;
using axistracking.Views.Interface;
using axistracking.Views.Template;
using Xamarin.Forms;
using System.Linq;

namespace axistracking.Views
{
	public partial class ViewComando : ContentPage, IViewComando
	{
		private ViewModelComando _viewModel;
		private CustomDialogAlert _painelLoad { get; set; }

		private FormularioDinamico _form;
		public FormularioDinamico Form
		{
			get
			{	
				if(_form == null)
				{
					_form = new FormularioDinamico(_viewModel.CommandWidthDefault);
				}
				return _form;
			}
		}
		
		public ViewComando(object obj)
		{
			InitializeComponent();
			PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

			_viewModel = new ViewModelComando(
				obj
			);
			_viewModel._view = this as IViewComando;
			this.BindingContext = _viewModel;

			MontaLoad();
		}

		protected override void OnAppearing()
		{
			_viewModel.OnAppearing();

			CommandPicker.SelectedItem = null;

			if(ParameterBox.Root != null)
				ParameterBox.Root.Clear();
		}

		protected override void OnDisappearing()
		{
			_viewModel.OnDisappearing();
		}

		private void MontaLoad()
		{
			_painelLoad = new CustomDialogAlert(
				PageContent
				, Color.White.MultiplyAlpha(0.8)
				, false
			);
			ActivityIndicator activity = _painelLoad.RequireActivityIndicator();
			activity.Color = Color.Gray;
			_painelLoad.ShowAlert(activity);
		}

		public void Handle_BindingContextChanged(
			object sender
			, System.EventArgs e
		)
		{
			try
			{
				CustomTableView gridTemp = ((CustomTableView)sender);
				List<ComandoParametroDto> painel = (List<ComandoParametroDto>)gridTemp.BindingContext;
				gridTemp.Root.Clear();

				if(painel != null)
				{
					TableRoot temp = Form.BuildFormulario(painel);

					Device.BeginInvokeOnMainThread(() => gridTemp.Root = temp);
				}

			} catch { }

		}

		void Picker_Unfocused(
			object sender
			, Xamarin.Forms.FocusEventArgs e
		)
		{
			_viewModel.PickerSelectedIndexChanged(sender, e);
			_viewModel.BtnSendIsEnabled = true;
		}

		#region interface
		public void ExibirLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelLoad.ShowAlert();
			});
		}

		public void EscondeLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelLoad.HideAlert();
			});
		}

		public List<ComandoParametroDto> RecuperaValor()
		{
			List<ComandoParametroDto> lstReturn = new List<ComandoParametroDto>();
			try
			{

				Dictionary<Int32, ComandoParametroDto> dic 
				= new Dictionary<int, ComandoParametroDto>();

				foreach(KeyValuePair<String, Cell> item in Form.ListParametros)
				{
					ComandoParametroDto temp;
					String[] parts = item.Key.Split('-');
					Int32 idParametro = Convert.ToInt32(parts[0]);

					if(dic.ContainsKey(idParametro))
					{
						temp = dic[idParametro];
						dic.Remove(idParametro);
					}
					else
					{
						temp = new ComandoParametroDto()
						{
							IdParametro = idParametro
						};
					}

					String valor = "";

					Type tipo = item.Value.GetType();

					Layout<View> tempView;

					if(tipo == typeof(CustomSwitchCell))
					{
						CustomSwitchCell tempCell = (CustomSwitchCell)item.Value;
						tempView = (Grid)tempCell.View;
					}
					else if(tipo == typeof(CustomPickerCell))
					{
						CustomPickerCell tempCell = (CustomPickerCell)item.Value;
						tempView = (StackLayout)tempCell.View;
					}
					else
					{
						CustomEntryCell tempCell = (CustomEntryCell)item.Value;
						tempView = (StackLayout)tempCell.View;
					}

					foreach(View itemView in tempView.Children)
					{
						Type tipoChildren = itemView.GetType();
						if(tipoChildren != typeof(Label))
						{
							if(tipoChildren == typeof(Switch))
							{
								valor = Convert.ToInt32(((Switch)itemView).IsToggled).ToString();
							}
							else if(tipoChildren == typeof(Picker))
							{
								FormularioDinamicoElementDto tempPicker 
								= ((Picker)itemView).SelectedItem as FormularioDinamicoElementDto;

								if(tempPicker != null)
									valor = tempPicker.Id;
							}
							else
							{
								valor = ((Entry)itemView).Text;
							}
							break;
						}
					}

					temp.Valor += valor;

					dic.Add(temp.IdParametro, temp);
				}

				lstReturn = dic.Values.ToList();

			} catch { }

			return lstReturn;

		}

		public void LimpaComandos()
		{
			Device.BeginInvokeOnMainThread(() => {
				CommandPicker.SelectedItem = null;
				ParameterBox.Root.Clear();
			});
		}
		#endregion

	}
}
