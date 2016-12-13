using BLL;
using System;
using VML;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.Extensions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoresPage : Page
    {
        public CategoresPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var ac = Resources["SystemControlBackgroundAccentBrush"] as SolidColorBrush;
            var bc = new SolidColorBrush(Colors.Gray);
            var fc = CategoresList.Foreground as SolidColorBrush;

            this.ViewModel.SetBrushes(ac, bc, fc);
        }

        private void showMenu(object sender)
        {
            var uiSender = sender as UIElement;
            var flyout = (FlyoutBase)uiSender.GetValue(FlyoutBase.AttachedFlyoutProperty);
            flyout.ShowAt(uiSender as FrameworkElement);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditCategoryPage));
        }

        private void EditAccountMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var cat = button.DataContext as CategoryViewModel;

            ViewModel.EditCategoryCommand.Execute(cat);
        }

        private void DeleteAccountMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var cat = button.DataContext as CategoryViewModel;

            ViewModel.DeleteCategoryCommand.Execute(cat);
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            showMenu(sender);
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            showMenu(sender);
        }

        private void AddNewCategory_Loaded(object sender, RoutedEventArgs e)
        {
            var pos = AddNewCategory.GetPosition();
            if (pos.Y < this.ActualHeight / 2)
            {
                AddNewCategory.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateSelectedCategory();
        }
    }
}
