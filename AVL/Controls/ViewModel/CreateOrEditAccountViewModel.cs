using BLL;
using CDL.Classes;
using CDL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Mvvm;
using VML;
using Windows.UI.Xaml;

namespace AVL.Controls
{
    public class CreateOrEditAccountViewModel : BindableBase
    {
        private IAccountService _accountService;

        private AccountViewModel _account;
        public event EventHandler OperationDone;

        private string _accountName;
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                if (!Equals(_accountName, value))
                {
                    _accountName = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        private string _accountCurrency;
        public string AccountCurrency
        {
            get
            {
                return _accountCurrency;
            }
            set
            {
                if (!Equals(_accountCurrency, value))
                {
                    _accountCurrency = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        private string _accountBalance;
        public string AccountBalance
        {
            get
            {
                return _accountBalance;
            }
            set
            {
                if (!Equals(_accountBalance, value))
                {
                    _accountBalance = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        private string _accountNumber;
        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                if (!Equals(_accountNumber, value))
                {
                    _accountNumber = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        public Visibility AccountNumberVisibility
        {
            get
            {
                if (AccountType == AccountType.BankAccount || AccountType == AccountType.Card)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        private AccountType _accountType;
        public AccountType AccountType
        {
            get
            {
                return _accountType;
            }
            set
            {
                if (!Equals(_accountType, value))
                {
                    _accountType = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                    RaisePropertyChanged(nameof(AccountNumberVisibility));
                }
            }
        }

        private bool _accountIsDefault;
        public bool AccountIsDefault
        {
            get
            {
                return _accountIsDefault;
            }
            set
            {
                if (!Equals(_accountIsDefault, value))
                {
                    _accountIsDefault = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        public List<Currency> CurrencyList { get; set; } = new List<Currency>();
        public Currency CurrencySelectedItem
        {
            get
            {
                if (!string.IsNullOrEmpty(AccountCurrency))
                {
                    return CurrencyList.FirstOrDefault(c => c.Value == AccountCurrency);
                }

                return CurrencyList.FirstOrDefault();
            }
            set
            {
                if (value != null && !Equals(AccountCurrency, value.Value))
                {
                    AccountCurrency = value.Value;
                    RaisePropertyChanged(nameof(AccountCurrency));
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        public List<AccountTypes> Types { get; set; } = new List<AccountTypes>();
        public AccountTypes AccountTypeSelectedItem
        {
            get
            {
                var type = AccountType.ToString();
                return Types.FirstOrDefault(t => t.Name == type);
            }
            set
            {
                var type = AccountType.ToString();
                if (value != null && !Equals(type, value.Value))
                {
                    switch (value?.Name)
                    {
                        case "BankAccount":
                            AccountType = AccountType.BankAccount;
                            break;
                        case "Card":
                            AccountType = AccountType.Card;
                            break;
                        case "Cash":
                            AccountType = AccountType.Cash;
                            break;
                    }

                    RaisePropertyChanged(nameof(AccountType));
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(AccountName))
                {
                    return false;
                }
                if (AccountType != AccountType.Cash && string.IsNullOrEmpty(AccountNumber))
                {
                    return false;
                }

                double balance;
                if (!double.TryParse(AccountBalance, out balance))
                {
                    return false;
                }



                return true;
            }
        }

        private DelegateCommand _createOrEditCommand;
        public DelegateCommand CreateOrEditCommand
        {
            get
            {
                return
                    _createOrEditCommand ?? (_createOrEditCommand = new DelegateCommand(() => CreateOrEdit(), () => IsValid));
            }
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                return
                    _cancelCommand ?? (_cancelCommand = new DelegateCommand(() => Cancel(), () => true));
            }
        }

        private string _headerTitle;
        public string HeaderTitle
        {
            get
            {
                return _headerTitle;
            }
        }

        public CreateOrEditAccountViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            _account = null;

            SetConstantLists();

            _accountName = "";
            _accountType = AccountType.Cash;
            _accountNumber = "";
            _accountCurrency = "RUR";
            _accountBalance = "";
            _accountIsDefault = false;
            _headerTitle = "Новый счёт";
        }

        public CreateOrEditAccountViewModel(IAccountService accountService, AccountViewModel account)
        {
            _accountService = accountService;
            _account = account;

            SetConstantLists();

            _accountName = _account.Name;
            _accountType = _account.Type;
            _accountNumber = _account.Number;
            _accountCurrency = _account.Currency;
            _accountBalance = _account.Balance.ToString();
            _accountIsDefault = _account.IsDefault;
            _headerTitle = "Редактирование счёта";
        }

        private void SetConstantLists()
        {
            if (!CurrencyList.Any())
            {
                CurrencyList.Add(new Currency()
                {
                    Name = "Рубль РФ",
                    Value = "RUR"
                });
                CurrencyList.Add(new Currency()
                {
                    Name = "Доллар США",
                    Value = "USD"
                });
                CurrencyList.Add(new Currency()
                {
                    Name = "Евро",
                    Value = "EUR"
                });
            }

            if (!Types.Any())
            {
                Types.Add(new AccountTypes()
                {
                    Name = AccountType.Cash.ToString(),
                    Value = "Наличные средства"
                });
                Types.Add(new AccountTypes()
                {
                    Name = AccountType.Card.ToString(),
                    Value = "Банковская карта"
                });
                Types.Add(new AccountTypes()
                {
                    Name = AccountType.BankAccount.ToString(),
                    Value = "Банковский счёт"
                });
            }
        }

        public void CreateOrEdit()
        {
            if (!IsValid)
            {
                return;
            }

            if (_account == null)
            {//Создаём новый счёт
                try
                {
                    _accountService.CreateNewAccount(AccountName, AccountType, AccountNumber, AccountCurrency, double.Parse(AccountBalance), AccountIsDefault);

                    var resultEventArgs = new OperationDoneEventArgs();
                    resultEventArgs.Type = OperationType.Create;
                    resultEventArgs.Result = true;

                    OperationDone?.Invoke(this, resultEventArgs);
                }
                catch (Exception ex)
                {
                    var resultEventArgs = new OperationDoneEventArgs();
                    resultEventArgs.Type = OperationType.Create;
                    resultEventArgs.Message = ex.Message;
                    resultEventArgs.Result = false;

                    OperationDone?.Invoke(this, resultEventArgs);
                }
            }
            else
            {//Сохраняем изменения
                _account.Name = AccountName;
                _account.Type = AccountType;
                _account.Number = AccountNumber;
                _account.Balance = double.Parse(AccountBalance);
                _account.Currency = AccountCurrency;
                _account.IsDefault = AccountIsDefault;

                try
                {
                    _accountService.Update(_account);

                    var resultEventArgs = new OperationDoneEventArgs();
                    resultEventArgs.Type = OperationType.Update;
                    resultEventArgs.Result = true;

                    OperationDone?.Invoke(this, resultEventArgs);
                }
                catch (Exception ex)
                {
                    var resultEventArgs = new OperationDoneEventArgs();
                    resultEventArgs.Type = OperationType.Update;
                    resultEventArgs.Message = ex.Message;
                    resultEventArgs.Result = false;

                    OperationDone?.Invoke(this, resultEventArgs);
                }
            }
        }

        public void Cancel()
        {
            var resultEventArgs = new OperationDoneEventArgs();
            resultEventArgs.Type = OperationType.Cancel;
            resultEventArgs.Result = true;

            OperationDone?.Invoke(this, resultEventArgs);
        }
    }
}