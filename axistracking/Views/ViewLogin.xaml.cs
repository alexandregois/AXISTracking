using axistracking.Domain.Realm;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using axistracking.Views.Interface;
using axistracking.ViewModels;
using axistracking.CustomElements;

namespace axistracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewLogin : ContentPage, IViewLogin
    {
        public Realm _realm;
		private App _app => (Application.Current as App);
		private CustomDialogAlert _painelTopLoad { get; set; }
		private ViewModelLogin _viewModelLogin { get; set; }

        public ViewLogin(Boolean paramIsPersonalizado, String paramNameProject)
        {
			InitializeComponent();

			Panel.Margin = _app.DefaultTemplateMargin;

			Panel.HeightRequest = _app.ScreenHeight;
			Panel.WidthRequest = _app.ScreenWidth;

			Grid.HeightRequest = Panel.HeightRequest;
			Grid.WidthRequest = Panel.WidthRequest;

			MontaLoad();

			_viewModelLogin = new ViewModelLogin(paramIsPersonalizado, paramNameProject);
			_viewModelLogin._view = this as IViewLogin;
			this.BindingContext = _viewModelLogin;
            
		}

		protected override void OnAppearing ()
		{
			var lifecycleHandler = (ViewModelLogin) this.BindingContext;
			lifecycleHandler.OnAppearing();
		}

		protected override void OnDisappearing ()
		{
			var lifecycleHandler = (ViewModelLogin) this.BindingContext;
			lifecycleHandler.OnDisappearing();
		}

		private void MontaLoad()
		{
			_painelTopLoad = new CustomDialogAlert(
				Panel
				, Color.White.MultiplyAlpha(0.8)
				, false
			);
			ActivityIndicator activity = _painelTopLoad.RequireActivityIndicator();
			activity.Color = Color.Gray;
			_painelTopLoad.AddChildren(activity);
		}


		#region interface
		public void ExibirLoad()
		{
            Device.BeginInvokeOnMainThread(() => {
                _painelTopLoad.ShowAlert();
            });
        }

		public void EscondeLoad()
		{
            Device.BeginInvokeOnMainThread(() => {
                _painelTopLoad.HideAlert();
            });
        }
		#endregion
    }
}