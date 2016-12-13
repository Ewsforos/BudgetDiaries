using Windows.UI.Xaml;
using System.Threading.Tasks;
using APP.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BLL;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Media;

namespace APP
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            
            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion            
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            await IoC.InitializeAsync();

            SettingsService _settings = SettingsService.Instance;

            if (App.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                _settings.AppTheme = ApplicationTheme.Dark;
            }
            else
            {
                _settings.AppTheme = ApplicationTheme.Light;
            }

            _settings.AppTheme = ApplicationTheme.Dark;

            NavigationService.Navigate(typeof(Views.TransactionsPage));
            await Task.CompletedTask;
        }

        public override Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {
            var suspend = e.SuspendingOperation.GetDeferral();

            IoC.Dispose();

            suspend.Complete();

            return base.OnSuspendingAsync(s, e, prelaunchActivated);
        }
    }
}

