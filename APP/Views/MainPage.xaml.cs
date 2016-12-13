using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace APP.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(AccountsPage));
            Shell.HamburgerMenu.NavigationService.ClearHistory();
        }
    }
}
