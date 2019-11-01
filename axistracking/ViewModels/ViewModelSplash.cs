using axistracking.ViewModels.Base;
using System;
using Xamarin.Forms;

namespace axistracking.ViewModels
{
    public class ViewModelSplash : ViewModelBase
    {
        protected App _app => (Application.Current as App);

        private Double _gridRowTop;
        public Double GridRowTop
        {
            get
            {
                return _gridRowTop;
            }
            set
            {
                _gridRowTop = value;
                this.Notify("GridRowTop");
            }
        }

        private Color _corFundoLogin;
        public Color CorFundoLogin
        {
            get
            {
                return _corFundoLogin;
            }
            set
            {
                _corFundoLogin = value;
                this.Notify("CorFundoLogin");
            }
        }

        private Double _gridRowBotton;
        public Double GridRowBotton
        {
            get
            {
                return _gridRowBotton;
            }
            set
            {
                _gridRowBotton = value;
                this.Notify("GridRowBotton");
            }
        }

        private Double _gridRowCenter;
        public Double GridRowCenter
        {
            get
            {
                return _gridRowCenter;
            }
            set
            {
                _gridRowCenter = value;
                this.Notify("GridRowCenter");
            }
        }

        private ImageSource _splashImage;
        public ImageSource SplashImage
        {
            get
            {
                return _splashImage;
            }
            set
            {
                _splashImage = value;
                this.Notify("SplashImage");
            }
        }

		private Double _splashImageWidth;
		public Double SplashImageWidth
		{
			get
			{
				return _splashImageWidth;
			}
			set
			{
				_splashImageWidth = value;
				this.Notify("SplashImageWidth");
			}
		}

		private Double _splashImageHeight;
		public Double SplashImageHeight
		{
			get
			{
				return _splashImageHeight;
			}
			set
			{
				_splashImageHeight = value;
				this.Notify("SplashImageHeight");
			}
		}

		public ViewModelSplash()
        {
            try
            {
                
                if (Application.Current.Properties.ContainsKey("UrlLogo"))
                {
                    if (Application.Current.Properties["UrlLogo"] != null)
                        SplashImage = ImageSource.FromUri(new Uri((string)Application.Current.Properties["UrlLogo"]));
                    else
                        SplashImage = ImageSource.FromFile("splash_tracking.png");
                }
                else
                    SplashImage = ImageSource.FromFile("splash_tracking.png");

            }
            catch (Exception)
            {

                SplashImage = ImageSource.FromFile("splash_tracking.png");
            }


            if (_app.nameProject == "agility")
            {
                CorFundoLogin = Color.FromHex("#810A9A");
            }


            GridRowTop = (_app.ScreenHeight / 100) * 40;
            GridRowCenter = (_app.ScreenHeight / 100) * 75;
            GridRowBotton = (_app.ScreenHeight / 100) * 10;

			SplashImageWidth = GridRowCenter / 100 * 75;
			SplashImageHeight = GridRowCenter / 100 * 75;

		}

        public override void OnAppearing()
        {

        }

        public override void OnDisappearing()
        {

        }

        public override void OnLayoutChanged()
        {

        }

        public override void DefaultTemplateBuild()
        {
            //throw new NotImplementedException();
        }
    }
}
