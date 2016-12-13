using AVL;
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
    public sealed partial class TransactionsPage : Page
    {
        public TransactionsPage()
        {
            this.InitializeComponent();
        }

        private void EditTransactionMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var transaction = button.DataContext as TransactionViewModel;

            ViewModel.EditTransactionCommand.Execute(transaction);
        }

        private void DeleteTransactionMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var transaction = button.DataContext as TransactionViewModel;

            ViewModel.DeleteTransactionCommand.Execute(transaction);
        }
    }
}
