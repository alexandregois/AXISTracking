using Xamarin.Forms;
using System;

namespace axistracking.CustomElements
{
	public class CustomEntry : Entry
	{
		public static readonly BindableProperty BorderStyleProperty 
		= BindableProperty.Create(
			"BorderStyle"
			, typeof(string)
			, typeof(CustomEntry)
			, ""
		);

		public string BorderStyle
		{
			get { return (string)GetValue(BorderStyleProperty); }
			set { SetValue(BorderStyleProperty, value); }
		}

		public static readonly BindableProperty ErrorMessageProperty 
		= BindableProperty.Create(
			"ErrorMessage"
			, typeof(string)
			, typeof(CustomEntry)
			, ""
		);

		public string ErrorMessage
		{
			get { return (string)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}

	}
}
