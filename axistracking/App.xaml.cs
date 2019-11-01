using System;
using System.Collections.Generic;
using System.Diagnostics;
using axistracking.CustomClass;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using axistracking.Services;
using axistracking.Services.ServiceRealm;
using axistracking.ViewModels.Services;
using axistracking.Views;
using axistracking.Views.Services;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace axistracking
{
    public partial class App : Application
    {


        public static Configuracao Configuracao { get; set; }

        public Boolean isPersonalizado { get; set; }
        public String nameProject { get; set; }

        public static List<PainelDto> ListPainelSource { get; set; }
        public static List<PainelDto> ListPainelTopUnidadesSource;

        public static List<PosicaoHistorico> ListUnidadesSource;

        public static List<PosicaoHistorico> ListHistoricoSource;

        public static List<ComandoLog> ListComandosSource;

        public TokenRealm _token;

        public DateTime DataHistoricoAnterior { get; set; }

        public Token Token { get; set; }


        private IUtilPlataform _util;
        public IUtilPlataform Util
        {
            get
            {
                if (_util == null)
                    _util = DependencyService.Get<IUtilPlataform>();

                return _util;
            }
            set
            {
                _util = value;
            }
        }

        #region Template
        private Double _screenWidth;
        public Double ScreenWidth
        {
            get
            {
                if (Math.Abs(_screenWidth) < Double.Epsilon)
                    _screenWidth = Util.GetScreenWidth();

                return _screenWidth;
            }
            set
            {
                _screenWidth = value;
            }
        }

        private Double _screenHeight;
        public Double ScreenHeight
        {
            get
            {
                if (Math.Abs(_screenHeight) < Double.Epsilon)
                    _screenHeight = Util.GetScreenHeight();

                return _screenHeight;
            }
            set
            {
                _screenHeight = value;
            }
        }

        private Double _reduceTop;
        public Double ReduceTop
        {
            get
            {
                if (Math.Abs(_reduceTop) < Double.Epsilon)
                    _reduceTop = Util.GetHeightStatusBar();

                return _reduceTop;
            }
            set
            {
                _reduceTop = value;
            }
        }
        public Thickness DefaultTemplateMargin
        {
            get
            {
                Double reduce = 0;
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    reduce = ReduceTop;
                }

                return new Thickness(
                    0
                    , reduce
                    , 0
                    , 0
                );
            }
        }

        public double DefaultTemplateHeightNavegationBar
        {
            get
            {
                return 53;
            }
        }

        public double DefaultTemplateHeightContent
        {
            get
            {
                return ScreenHeight
                    - (
                        DefaultTemplateHeightNavegationBar
                        + DefaultTemplateMargin.Top
                    );
            }
        }

        public static StackLayout PanelBarra { get; set; }

        #endregion

        public App(Boolean paramIsPersonalizado, String paramNameProject)
        {

            InitializeComponent();

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                StartAppCenter();

            LocalizeApp();

            isPersonalizado = paramIsPersonalizado;
            nameProject = paramNameProject;


            NavigationPage navigationPage;


            try
            {
                Configuracao = new Configuracao();
                GetDependency();



                //if (isPersonalizado && nameProject != "khronos")
                //    navigationPage = new NavigationPage(new SplashPersonalizado());
                //else
                //{

                    //if (Application.Current.Properties.ContainsKey("User") && Application.Current.Properties.ContainsKey("Pass"))
                    navigationPage = new NavigationPage(new ViewPainel());
                    //else
                    //navigationPage = new NavigationPage(new ViewLogin(isPersonalizado, nameProject));

                //}


                navigationPage.BarBackgroundColor = Color.FromHex("#FF2E2F3A");
                navigationPage.BarTextColor = Color.White;
                navigationPage.BackgroundColor = Color.White; //Color.FromHex("#FF2E2F3A");

                MainPage = navigationPage;

            }
            catch (Exception)
            {

                navigationPage = new NavigationPage(new ViewLogin(isPersonalizado, nameProject));

                navigationPage.BarBackgroundColor = Color.FromHex("#FF2E2F3A");
                navigationPage.BarTextColor = Color.White;
                navigationPage.BackgroundColor = Color.White; //Color.FromHex("#FF2E2F3A");

                MainPage = navigationPage;
            }


        }


        private void GetDependency()
        {
            if (Configuracao.UseMockDataStore)
            {
                DependencyService.Register<MockDataStore>();
            }
            else
            {
                DependencyService.Register<CloudDataStore>();
            }

            DependencyService
                .Register<IMessageService, MessageService>();

            DependencyService
                .Register<INavigationService, NavigationService>();

            _util = DependencyService.Get<IUtilPlataform>();

        }

        private void LocalizeApp()
        {
            //System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            //var assembly = typeof(App).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //System.Diagnostics.Debug.WriteLine("====================================");

            // determine the correct, supported .NET culture
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resx.AppResources.Culture = ci; // set the RESX for resource localization
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
        }

        protected override void OnStart()
        {

            StartAppCenter();

        }

        protected void StartAppCenter()
        {

            String mobileKey = String.Empty;

            mobileKey = "ios=8b9ba4c2-f4ec-417a-af3b-2f162072ce30;" +
                    "android=567f6f75-f529-4828-a023-287655052a02;";


            if (nameProject == "khronos")
            {
                mobileKey = "ios=9080a1f6-9273-4d9d-96cf-f059a1e52eb2;" +
                "android=19319985-2559-4c73-b9a0-2852649c7656";

            }

            if (nameProject == "maxima")
            {
                mobileKey = "ios=c257e245-e0ab-40d6-9d2a-c6d91fff1883;" +
                    "android=08c45e15-334c-4bd6-9a12-f90e3026b295";
            }

            if (nameProject == "gns")
            {
                mobileKey = "ios=6f04c133-f0f0-4b84-ab46-4990ea3401f7;" +
                    "android=6f04c133-f0f0-4b84-ab46-4990ea3401f7";
            }

            if (nameProject == "alltech")
            {
                mobileKey = "ios=89a546f6-a508-4b4c-9385-f44bbe855cd7;" +
                    "android=89a546f6-a508-4b4c-9385-f44bbe855cd7";
            }


            AppCenter.Start(
                mobileKey
                , typeof(Analytics)
                , typeof(Crashes)
                , typeof(Push)
            );


            AppCenter.SetEnabledAsync(true);

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }

}
