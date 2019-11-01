using System;
using System.Linq;
using Xamarin.Forms;

namespace axistracking.CustomElements.Behaviors
{
	public class EntryMaxMinValidatorBehavior : Behavior<Entry>
	{
		public int MinNumber { get; set; }
		public int MaxNumber { get; set; }

		protected override void OnAttachedTo(Entry bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.TextChanged += OnEntryTextChanged;
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.TextChanged -= OnEntryTextChanged;
		}

		void OnEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			Double? valor = null;

			try
			{

				CustomEntry entry = (CustomEntry)sender;

				StackLayout temp = (StackLayout)entry.Parent;

				try
				{
					valor = Double.Parse(entry.Text);
					// if Entry text is longer then valid length
					if (valor < MinNumber || valor > MaxNumber)
					{
						entry.ErrorMessage = "ERROR";
					}
					else
					{
						entry.ErrorMessage = "";
					}
				} 
				catch
				{
					entry.ErrorMessage = "ERROR";
				}



			} catch { }
		}
	}
}
