using APP.ViewModels;
using AVL.Controls;
using BLL;
using CDL.Classes;
using CDL.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using VML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit;
using WinRTXamlToolkit.Controls.Extensions;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountsPage : Page
    {
        public AccountsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                // Parameter is item ID
                var id = (Guid)e.Parameter;
                //_lastSelectedItem = items.Where((item) => item.ItemId == id).FirstOrDefault();
            }

            var ac = Resources["SystemControlBackgroundAccentBrush"] as SolidColorBrush;
            var bc = new SolidColorBrush(Colors.Gray);
            var fc = AccountsList.Foreground as SolidColorBrush;

            this.ViewModel.SetBrushes(ac, bc, fc);
        }

        private void AppBarPeriodButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarButton;
            var flyout = this.Resources["CreateOrEditFlyout"] as Flyout;
            flyout.ShowAt(button);
        }

        private void showMenu(object sender)
        {
            var uiSender = sender as UIElement;
            var flyout = (FlyoutBase)uiSender.GetValue(FlyoutBase.AttachedFlyoutProperty);
            flyout.ShowAt(uiSender as FrameworkElement);
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            showMenu(sender);
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            showMenu(sender);
        }

        /// <summary>
        /// Создание нового счёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditAccountPage));
        }

        /// <summary>
        /// Редактирование выбранного счёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAccountMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var acc = button.DataContext as AccountViewModel;

            ViewModel.EditAccountCommand.Execute(acc);
        }

        private void DeleteAccountMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var acc = button.DataContext as AccountViewModel;

            ViewModel.DeleteAccountCommand.Execute(acc);
        }

        private void AddNewAccount_Loaded(object sender, RoutedEventArgs e)
        {
            var pos = AddNewAccount.GetPosition();
            if (pos.Y < this.ActualHeight / 2)
            {
                AddNewAccount.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateSelectedAccount();
        }
    }
}
