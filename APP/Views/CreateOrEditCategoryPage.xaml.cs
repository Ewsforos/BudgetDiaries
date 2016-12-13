using AVL.Controls;
using BLL;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrEditCategoryPage : Page
    {
        public CreateOrEditCategoryPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var serializationService = Template10.Services.SerializationService.SerializationService.Json;

            base.OnNavigatedTo(e);

            var service = IoC.Resolve<ICategoryService>();
            CreateOrEditCategoryViewModel CoECVM = null;

            if (e.Parameter != null)
            {
                var accId = Guid.Parse(serializationService.Deserialize(e.Parameter?.ToString()).ToString());

                var acc = service.GetCategoryById(accId);

                if (acc != null)
                {
                    CoECVM = new CreateOrEditCategoryViewModel(service, acc);
                }
            }

            if (CoECVM == null)
            {
                CoECVM = new CreateOrEditCategoryViewModel(service);
            }

            CoECVM.OperationDone += CoEAVM_OperationDone;

            CreateOrEditControl.SetContent(CoECVM);
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
