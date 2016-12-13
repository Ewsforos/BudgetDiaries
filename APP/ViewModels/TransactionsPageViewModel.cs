using APP.Views;
using BLL;
using CDL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using VML;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace APP.ViewModels
{
    public class TransactionsPageViewModel : BindableBase
    {
        private ITransactionService _service { get; }

        private SolidColorBrush _incomeBrush, _expensesBrush;

        public ObservableCollection<TransactionViewModel> Transactions
        {
            get
            {
                var items = _service?.GetTransactions()?.OrderByDescending(t => t.Date)?.ToList();
                var trns = new ObservableCollection<TransactionViewModel>(items ?? new List<TransactionViewModel>());

                if (trns.Any())
                {
                    foreach (var transaction in trns)
                    {
                        if (transaction.Value > 0)
                        {
                            transaction.BackgroundBrush = _incomeBrush;
                        }
                        else
                        {
                            transaction.BackgroundBrush = _expensesBrush;
                        }
                    }
                }

                return trns;
            }
        }

        private Object _selected;
        public Object Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (!Equals(_selected, value))
                {
                    _selected = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isTransactionsLoading;
        public bool IsTransactionsLoading
        {
            get
            {
                return _isTransactionsLoading;
            }
            set
            {
                if (!Equals(_isTransactionsLoading, value))
                {
                    _isTransactionsLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isTransactionDetailsLoading;
        public bool IsTransactionDetailsLoading
        {
            get
            {
                return _isTransactionDetailsLoading;
            }
            set
            {
                if (!Equals(_isTransactionDetailsLoading, value))
                {
                    _isTransactionDetailsLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        DelegateCommand<TransactionViewModel> _deleteTransactionCommand;
        public DelegateCommand<TransactionViewModel> DeleteTransactionCommand
            => _deleteTransactionCommand ?? (_deleteTransactionCommand = new DelegateCommand<TransactionViewModel>(async (TransactionViewModel transaction) =>
            {
                await DeleteTransaction(transaction);
            }, (TransactionViewModel param) => true));

        DelegateCommand<TransactionViewModel> _editTransactionCommand;
        public DelegateCommand<TransactionViewModel> EditTransactionCommand
            => _editTransactionCommand ?? (_editTransactionCommand = new DelegateCommand<TransactionViewModel>((TransactionViewModel transaction) =>
            {
                EditTransaction(transaction);
            }, (TransactionViewModel param) => true));


        public TransactionsPageViewModel()
        {
            _service = IoC.Resolve<ITransactionService>();

            var resource = new ResourceDictionary() { Source = new Uri("ms-appx:///Styles/Custom.xaml") };
            if (resource.ContainsKey("IncomeColorBrush"))
            {
                _incomeBrush = resource["IncomeColorBrush"] as SolidColorBrush;
                _expensesBrush = resource["ExpensesColorBrush"] as SolidColorBrush;
            }
        }
        
        private void UpdateTransactionsView()
        {
            RaisePropertyChanged("Transactions");
        }

        public void AddNewIncomeTransactionBottomBar_Click(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditTransactionPage), TransactionType.Income);
        }

        public void AddNewExpenceTransactionBottomBar_Click(object sender, RoutedEventArgs e)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditTransactionPage), TransactionType.Expensess);
        }

        public void PeriodChooser_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task DeleteTransaction(TransactionViewModel transaction)
        {
            var dialog = new MessageDialog($"Вы действительно хотите удалить операцию {transaction.Title}?");
            dialog.Title = "Внимание";
            dialog.Commands.Add(new UICommand { Label = "Да", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Нет", Id = 1 });
            dialog.DefaultCommandIndex = 0;

            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                try
                {
                    var transactionService = IoC.Resolve<ITransactionService>();
                    var accountService = IoC.Resolve<IAccountService>();

                    var account = accountService.GetAccountById(transaction.AccountID);

                    account.Balance -= transaction.Value;

                    transactionService.Remove(transaction);

                    accountService.Update(account);

                    UpdateTransactionsView();
                }
                catch (Exception) { }
            }
        }

        private void EditTransaction(TransactionViewModel transaction)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditTransactionPage), transaction.ID);
        }
    }
}
