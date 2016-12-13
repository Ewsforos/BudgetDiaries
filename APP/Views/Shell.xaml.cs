using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace APP.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        public Shell()
        {
            Instance = this;
            InitializeComponent();

            var headerContent = new Grid();
            headerContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            headerContent.VerticalAlignment = VerticalAlignment.Stretch;
            headerContent.Background = HamburgerMenu.HamburgerBackground;
            headerContent.Width = HamburgerMenu.PaneWidth - 48;

            var headerTitle = new TextBlock()
            {
                Text = "Menu",
                Margin = new Thickness(15, 0, 0, 0),
                Foreground = HamburgerMenu.HamburgerForeground,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = HamburgerMenu.FontSize
            };

            headerContent.Children.Add(headerTitle);

            HamburgerMenu.HeaderContent = headerContent;
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }
    }
}

