using otp.Helpers;
using otp.Services;
using otp.ViewModels;
using FreshMvvm;
using System;
using System.Globalization;
using Xamarin.Forms;
using Akavache;
using Xamarin.Essentials;

namespace otp
{
    public partial class App : Application
    {
        public App()
        {
            LoadDate();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzMxNjIxMEAzMjM1MmUzMDJlMzBmeFZadGJOd0xVY0FvVEhzdk5WZ0twVG9WMXdpZms0OHpyem5vUnVnd293PQ==");


            InitializeComponent();
            Device.SetFlags(new[] { "MediaElement_Experimental,Brush_Experimental" });
            BlobCache.ApplicationName = "otp";
            CultureInfo cal = new CultureInfo(Helpers.Settings.Language);
            otp.Resources.AppResources.Culture = cal;
            FreshIOC.Container.Register<IOpenAIService, OpenAIService>();

         //   var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<ConnectionViewModel>();
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                var pageModel = FreshMvvm.FreshPageModelResolver.ResolvePageModel<ConnectionViewModel>();
           
                MainPage = new FreshNavigationContainer(pageModel);
            }
            else
            {
                var pageModel = FreshMvvm.FreshPageModelResolver.ResolvePageModel<IntroViewModel>();

                MainPage = new FreshNavigationContainer(pageModel);
            }
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;


        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            if (access == NetworkAccess.None)
            {
                var pageModel = FreshMvvm.FreshPageModelResolver.ResolvePageModel<ConnectionViewModel>();

                MainPage = new FreshNavigationContainer(pageModel);

            }
            else
            {
                var pageModel = FreshMvvm.FreshPageModelResolver.ResolvePageModel<IntroViewModel>();

                MainPage = new FreshNavigationContainer(pageModel);
            }

        }


        void LoadDate()
        {

            FreshIOC.Container.Register<IGenericRepository, GenericRepository>();
            FreshIOC.Container.Register<IServicesService, ServicesService>();
            
            //FreshIOC.Container.Register<IServicesProvidersService, ServicesProvidersService>();
        }

        public static void ChangeTheme(int option)
        {
            App.Current.Resources.MergedDictionaries.Clear();
            if (option == 1)
                App.Current.Resources.MergedDictionaries.Add(new Themes.Light());
            else
                App.Current.Resources.MergedDictionaries.Add(new Themes.Dark());

            App.Current.Resources.MergedDictionaries.Add(new Themes.GeneralStyle());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
