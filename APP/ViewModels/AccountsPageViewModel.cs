using BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using VML;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using Windows.UI.Popups;
using APP.Views;

namespace APP.ViewModels
{
    public class AccountsPageViewModel : BindableBase
    {
        private IAccountService _service { get; }

        private SolidColorBrush _accentBrush = null;
        private SolidColorBrush _backgroundBrush = null;
        private SolidColorBrush _foregroundBrush = null;

        public ObservableCollection<AccountViewModel> Accounts
        {
            get
            {
                var accs = new ObservableCollection<AccountViewModel>(_service.GetAccounts() ?? new List<AccountViewModel>());

                if (_accentBrush != null)
                {
                    var i = -1;
                    foreach (var acc in accs)
                    {
                        if (i > 0)
                        {
                            acc.BackgroundBrush = null;
                        }
                        else
                        {
                            acc.BackgroundBrush = _backgroundBrush;
                        }

                        if (!acc.IsDefault)
                        {
                            acc.ForegroundBrush = _foregroundBrush;
                        }
                        else
                        {
                            acc.ForegroundBrush = _accentBrush;

                            Selected = acc;
                        }

                        i *= -1;
                    }
                }

                return accs;
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

        private bool _isAccountsLoading;
        public bool IsAccountsLoading
        {
            get
            {
                return _isAccountsLoading;
            }
            set
            {
                if (!Equals(_isAccountsLoading, value))
                {
                    _isAccountsLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isAccountDetailsLoading;
        public bool IsAccountDetailsLoading
        {
            get
            {
                return _isAccountDetailsLoading;
            }
            set
            {
                if (!Equals(_isAccountDetailsLoading, value))
                {
                    _isAccountDetailsLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        DelegateCommand<AccountViewModel> _deleteAccountCommand;
        public DelegateCommand<AccountViewModel> DeleteAccountCommand
            => _deleteAccountCommand ?? (_deleteAccountCommand = new DelegateCommand<AccountViewModel>(async (AccountViewModel account) =>
            {
                await DeleteAccount(account);
            }, (AccountViewModel param) => true));
        
        DelegateCommand<AccountViewModel> _editAccountCommand;
        public DelegateCommand<AccountViewModel> EditAccountCommand
            => _editAccountCommand ?? (_editAccountCommand = new DelegateCommand<AccountViewModel>((AccountViewModel account) =>
            {
                EditAccount(account);
            }, (AccountViewModel param) => true));
        
        public AccountsPageViewModel()
        {
            _service = IoC.Resolve<IAccountService>();
        }

        public void SetBrushes(SolidColorBrush accentBrush, SolidColorBrush backgroundBrush, SolidColorBrush foregroundBrush)
        {
            _accentBrush = accentBrush;

            _backgroundBrush = backgroundBrush;
            _backgroundBrush.Opacity = 0.2;

            _foregroundBrush = foregroundBrush;

            RaisePropertyChanged("Accounts");
        }

        public void UpdateAccountsView()
        {
            RaisePropertyChanged("Accounts");
        }

        public void UpdateSelectedAccount()
        {
            RaisePropertyChanged("Selected");
        }

        private void EditAccount(AccountViewModel account)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditAccountPage), account.Id);
        }

        private async Task DeleteAccount(AccountViewModel account)
        {
            var dialog = new MessageDialog($"Вы действительно хотите удалить счёт {account.Name} и все операции, проведённые по нему?");
            dialog.Title = "Внимание";
            dialog.Commands.Add(new UICommand { Label = "Да", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Нет", Id = 1 });
            dialog.DefaultCommandIndex = 0;

            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                try
                {
                    IoC.Resolve<IAccountService>().Remove(account);

                    UpdateAccountsView();
                }
                catch (Exception ex) { }
            }
        }
    }
}
