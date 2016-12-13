using AVL.Controls;
using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrEditAccountPage : Page
    {
        public CreateOrEditAccountPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var serializationService = Template10.Services.SerializationService.SerializationService.Json;

            base.OnNavigatedTo(e);

            var service = IoC.Resolve<IAccountService>();
            CreateOrEditAccountViewModel CoEAVM = null;

            if (e.Parameter != null)
            {
                var accId = Guid.Parse(serializationService.Deserialize(e.Parameter?.ToString()).ToString());

                var acc = service.GetAccountById(accId);

                if (acc != null)
                {
                    CoEAVM = new CreateOrEditAccountViewModel(service, acc);
                }
            }

            if (CoEAVM == null)
            {
                CoEAVM = new CreateOrEditAccountViewModel(service);
            }

            CoEAVM.OperationDone += CoEAVM_OperationDone;

            CreateOrEditControl.SetContent(CoEAVM);
        }

        private void CoEAVM_OperationDone(object sender, EventArgs e)
        {
            if (Shell.HamburgerMenu.NavigationService.CanGoBack)
            {
                Shell.HamburgerMenu.NavigationService.GoBack();
                return;
            }

            Shell.HamburgerMenu.NavigationService.Navigate(typeof(MainPage));
        }
    }
}
