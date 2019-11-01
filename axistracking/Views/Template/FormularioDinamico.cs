using System;
using System.Collections.Generic;
using System.Linq;
using axistracking.CustomElements.Behaviors;
using axistracking.CustomElements.Cell;
using axistracking.Domain.Dto;
using axistracking.Enum;
using Xamarin.Forms;

namespace axistracking.Views.Template
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class FormularioDinamico
	{
		private Thickness _defaultFormularioMargin;
		private Double _defaultFormularioWidth;
		public Dictionary<String, Cell> ListParametros;

		public FormularioDinamico(
			Double paramWidth
		)
		{
			_defaultFormularioMargin = new Thickness(0, 5);
			_defaultFormularioWidth = paramWidth;
			ListParametros = new Dictionary<String, Cell>();
		}

		//TODO: Verificar Permissão
		public TableRoot BuildFormulario(
			List<ComandoParametroDto> paramList
		)
		{
			TableRoot view = new TableRoot();

			try
			{
				ListParametros.Clear();
				foreach(ComandoParametroDto item in paramList)
				{
					EnumTipoParametro tipo = (EnumTipoParametro)
						System.Enum.Parse(
							typeof(EnumTipoParametro)
							, item.IdTipoParametro.ToString()
						);

					TableSection div = new TableSection();

					switch(tipo)
					{
						case EnumTipoParametro.TextboxLivre:
							BuildParametroTextboxLivre(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.TextboxNumeroInteiro:
							BuildParametroTextboxNumeroInteiro(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.TextboxNumeroDecimal:
							BuildParametroTextboxNumeroDecimal(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.DateEditboxPeriodo:
							//BuildParametroDateEditboxPeriodo(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.RadioButtonGenerico:
							BuildParametroRadioButtonGenerico(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.CheckboxGenerico:
							BuildParametroCheckboxGenerico(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ComboboxGenerico:
						case EnumTipoParametro.AtivacaoSaida:
							BuildParametroComboboxGenerico(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ListCheckboxGenerico:
							BuildParametroListCheckboxGenerico(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ListCheckboxUnidadeRastreada:
							//BuildParametroListCheckboxUnidadeRastreada(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ComboboxEntrada:
							BuildParametroComboboxEntrada(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ListCheckboxSaida:
							BuildParametroListCheckboxSaida(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ComboboxAPN:
							BuildParametroComboboxAPN(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.TextboxIP:
							BuildParametroTextboxIP(
								item
								, ref div
							);
							break;
						case EnumTipoParametro.ListCheckboxGrupoUnidadeRastreada:
							//BuildParametroListCheckboxGrupoUnidadeRastreada(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ListCheckboxUnidadeOrganizacional:
							//BuildParametroListCheckboxUnidadeOrganizacional(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ListCheckboxRegra:
							//BuildParametroListCheckboxRegra(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ListCheckboxStatusChamado:
							//BuildParametroListCheckboxStatusChamado(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ListCheckboxEventos:
							//BuildParametroListCheckboxEventos(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ComplexboxElemetosCondicionais:
							//BuildParametroComplexboxElemetosCondicionais(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ComplexboxCondicaoQuebraPeriodo:
							//BuildParametroComplexboxCondicaoQuebraPeriodo(
							//	item
							//	, ref div
							//);
							break;
						case EnumTipoParametro.ComplexboxAreas:
							//BuildParametroComplexboxAreas(
							//	item
							//	, ref div
							//);
							break;
					}

					view.Add(div);
				}
			}
			catch {}

			return view;
		}

		private void BuildParametroTextboxLivre (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				CustomEntryCell entry = new CustomEntryCell()
				{
					CellLabel = paramItem.Label,
					CellPlaceholder = paramItem.ToolTip
				};

				if(!String.IsNullOrWhiteSpace(paramItem.Valor))
					entry.CellText = paramItem.Valor;

				if (paramItem.TamanhoMaximo.HasValue)
				{
					EntryLengthValidatorBehavior maxLength = new EntryLengthValidatorBehavior();
					maxLength.MaxLength = paramItem.TamanhoMaximo.Value;
					entry.Behaviors.Add(maxLength);
				}

				if(!String.IsNullOrWhiteSpace(paramItem.Dominio))
				{
					String[] minMax = paramItem.Dominio.Split('>');

					Int32 min = 0;
					Int32 max = 0;

					if (minMax.Count() == 2) {
						min = Convert.ToInt32(minMax[0]);
						max = Convert.ToInt32(minMax[1]);
					} else {
						min = 0;
						max = Convert.ToInt32(minMax[0]);
					}

					entry.Keyboard = Keyboard.Numeric;

					EntryMaxMinValidatorBehavior maxLength = new EntryMaxMinValidatorBehavior();
					maxLength.MinNumber = min;
					maxLength.MaxNumber = max;
					entry.Behaviors.Add(maxLength);
				}

				ListParametros.Add(
					paramItem.IdParametro.ToString()
					, entry
				);
				entry.Tapped += Entry_Tapped;
				paramDiv.Add(entry);
			} catch {  }
		}

		private void BuildParametroTextboxNumeroInteiro (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				//Mudar se tiver alguma diferença
				BuildParametroTextboxLivre (
					paramItem
					, ref paramDiv
				);
			} catch { }

		}

		private void BuildParametroTextboxNumeroDecimal (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				BuildParametroTextboxLivre (
					paramItem
					, ref paramDiv
				);
			} catch { }
		}

		//private void BuildParametroDateEditboxPeriodo (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//
		//}

		private void BuildParametroRadioButtonGenerico (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				BuildParametroComboboxGenerico (
					paramItem
					, ref paramDiv
				);
			} catch { }

		}

		private void BuildParametroCheckboxGenerico (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				CustomSwitchCell entry = new CustomSwitchCell()
				{
					CellLabel = paramItem.Label,
					CellText = paramItem.Valor
				};

				ListParametros.Add(
					paramItem.IdParametro.ToString()
					, entry
				);

				entry.Tapped += Entry_Tapped;
				paramDiv.Add(entry);
			} catch { }
		}

		private void BuildParametroComboboxGenerico (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				String[] opt = paramItem.Dominio.Split(';');
				List<FormularioDinamicoElementDto> dataSource = new List<FormularioDinamicoElementDto>();

				foreach (String item in opt)
				{
					String[] dataTemp = item.Split('|');

					dataSource.Add(new FormularioDinamicoElementDto()
					{
						Id = dataTemp[0],
						Text = dataTemp[1]
					});
				}

				CustomPickerCell entry = new CustomPickerCell()
				{
					DataSource = dataSource,
					ItemDisplayBinding = new Binding("Text"),
					CellLabel = paramItem.Label
				};


				if(!String.IsNullOrWhiteSpace(paramItem.Valor))
					entry.CellText = paramItem.Valor;

				ListParametros.Add(
					paramItem.IdParametro.ToString()
					, entry
				);

				entry.Tapped += Entry_Tapped;
				paramDiv.Add(entry);
			} catch { }
		}

		private void BuildParametroListCheckboxGenerico (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				paramDiv.Title = paramItem.Label;
				List<String> dominio = paramItem.Dominio.Split(';').ToList();
				List<char> valorDefault = paramItem.Valor.ToCharArray().Reverse().ToList();

				for(Int32 i = 0; i < dominio.Count(); i++)
				{
					CustomSwitchCell entry = new CustomSwitchCell()
					{
						CellLabel = dominio[i],
						CellText = valorDefault[i].ToString()
					};

					ListParametros.Add(
						paramItem.IdParametro.ToString() + "-" + i.ToString()
						, entry
					);

					entry.Tapped += Entry_Tapped;
					paramDiv.Add(entry);
				}

			} catch 
			{ 
			}
		}

		//private void BuildParametroListCheckboxUnidadeRastreada (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		private void BuildParametroComboboxEntrada (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				BuildParametroComboboxGenerico (
					paramItem
					, ref paramDiv
				);
			} catch { }
		}

		private void BuildParametroListCheckboxSaida (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				BuildParametroListCheckboxGenerico (
					paramItem
					, ref paramDiv
				);
			} catch { }
		}

		private void BuildParametroComboboxAPN (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				BuildParametroComboboxGenerico (
					paramItem
					, ref paramDiv
				);
			} catch { }
		}

		//TODO: Mascara IP
		private void BuildParametroTextboxIP (
			ComandoParametroDto paramItem
			, ref TableSection paramDiv
		)
		{
			try
			{
				EntryCell entry = new EntryCell();

				if(!String.IsNullOrWhiteSpace(paramItem.Valor))
					entry.Text = paramItem.Valor;

				//if (paramItem.TamanhoMaximo.HasValue)
				//{
				//	EntryLengthValidatorBehavior maxLength = new EntryLengthValidatorBehavior();
				//	maxLength.MaxLength = paramItem.TamanhoMaximo.Value;
				//	entry.Behaviors.Add(maxLength);
				//}

				//if(!String.IsNullOrWhiteSpace(paramItem.Dominio))
				//{
				//	String[] minMax = paramItem.Dominio.Split('>');

				//	Int32 min = 0;
				//	Int32 max = 0;

				//	if (minMax.Count() == 2) {
				//		min = Convert.ToInt32(minMax[0]);
				//		max = Convert.ToInt32(minMax[1]);
				//	} else {
				//		min = 0;
				//		max = Convert.ToInt32(minMax[0]);
				//	}

				//	entry.Keyboard = Keyboard.Numeric;

				//	EntryMaxMinValidatorBehavior maxLength = new EntryMaxMinValidatorBehavior();
				//	maxLength.MinNumber = min;
				//	maxLength.MaxNumber = max;
				//	entry.Behaviors.Add(maxLength);
				//}

				ListParametros.Add(
					paramItem.IdParametro.ToString()
					, entry
				);

				entry.Tapped += Entry_Tapped;
				paramDiv.Add(entry);
			} catch {  }
		}

		//private void BuildParametroListCheckboxGrupoUnidadeRastreada (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroListCheckboxUnidadeOrganizacional (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroListCheckboxRegra (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroListCheckboxStatusChamado (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroListCheckboxEventos (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroComplexboxElemetosCondicionais (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroComplexboxCondicaoQuebraPeriodo (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		//private void BuildParametroComplexboxAreas (
		//	ComandoParametroDto paramItem
		//	, ref StackLayout paramDiv
		//)
		//{
		//	try
		//	{
		//
		//	} catch { }
		//}

		void Entry_Tapped(
			object sender
			, EventArgs e
		)
		{
			try
			{
				ViewCell viewCell = (ViewCell)sender;

				object view = viewCell.View;

				Layout<View> temp;

				if(view.GetType() == typeof(StackLayout))
				{
					temp = (StackLayout)view;
				}
				else
				{
					temp = (Grid)view;
				}

				foreach(View item in temp.Children)
				{
					Type tipo = item.GetType();
					if(tipo != typeof(Label))
					{
						if(tipo == typeof(Switch))
						{
							((Switch)item).IsToggled = !((Switch)item).IsToggled;
						}
						else
						{
							item.Focus();
						}
						break;
					}
				}


			} catch { }
		}
	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}