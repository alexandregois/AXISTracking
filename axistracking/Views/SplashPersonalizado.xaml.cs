using axistracking.Domain;
using axistracking.Domain.Realm;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace axistracking.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPersonalizado : ContentPage
    {
        public App _app => (Application.Current as App);

        private ViewModelSplash _viewModel { get; set; }

        public TokenRealm _token;

        public Token Token { get; set; }

        public SplashPersonalizado()
        {
            InitializeComponent();

            try
            {

                imageLogo.SetBinding(Image.SourceProperty, "SplashImage");

				if(Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
				{
					imageLogo.HeightRequest = (_app.ScreenHeight / 100) * 13;
                    imageLogo.IsVisible = false;
                }


                Task.Delay(7000);

                _viewModel = new ViewModelSplash();
                this.BindingContext = _viewModel;


                TokenDataStore store = new TokenDataStore();
                _token = store.Get(1);

                if (_token != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new ViewPainel());
                        });
                }
                else if (_token != null && _token.IdAplicativo == 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                        });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                           Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                        });
                }

            }
            catch (Exception)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });

            }

        }
    }
}