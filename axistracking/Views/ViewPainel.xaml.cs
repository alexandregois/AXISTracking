using axistracking.ViewModels;
using axistracking.ViewModels.Services;
using axistracking.Views.Template;
using Xamarin.Forms;

namespace axistracking.Views
{
	public partial class ViewPainel : ContentPage
    {
		private ViewModelPainel _viewModelPainel;

        public ViewPainel()
        {
            InitializeComponent();

            PanelGeral.ControlTemplate = new ControlTemplate(typeof(DefaultPageTemplate));

            ListPainel.ItemTemplate = new DataTemplate(() =>
            {
				return new ListPainel_ViewCell((ViewModelPainel)this.BindingContext);
            });

			_viewModelPainel = new ViewModelPainel();

			this.BindingContext = _viewModelPainel;
			
        }

        protected override void OnAppearing()
        {
            var lifecycleHandler = (ViewModelPainel)this.BindingContext;
			lifecycleHandler.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelPainel)this.BindingContext;
			lifecycleHandler.OnDisappearing();
        }

		protected override bool OnBackButtonPressed()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_viewModelPainel
					.Deslogar();
			});
			base.OnBackButtonPressed();
			return true;
		}

    }

}