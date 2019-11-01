using System;
using Xamarin.Forms;
using System.Linq;

namespace axistracking.CustomElements.Behaviors
{
	public class EntryLengthValidatorBehavior : Behavior<Entry>
	{
		public int MaxLength { get; set; }

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
			try
			{
				CustomEntry entry = (CustomEntry)sender;

				StackLayout temp = (StackLayout)entry.Parent;

				// if Entry text is longer then valid length
				if (entry.Text.Length > this.MaxLength)
				{
					entry.ErrorMessage = "ERROR";
				}
				else
				{
					entry.ErrorMessage = "";
				}

			} catch { }
		}
	}
}
