using BLL;
using CDL.Classes;
using CDL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Mvvm;
using VML;
using Windows.UI.Xaml.Controls;

namespace AVL.Controls
{
    public class CreateOrEditTransactionViewModel : BindableBase
    {
        private IAccountService _accountService;
        public ObservableCollection<AccountViewModel> Accounts
        {
            get
            {
                var accs = new ObservableCollection<AccountViewModel>(_accountService.GetAccounts() ?? new List<AccountViewModel>());

                if (_transaction != null)
                {
                    SelectedAccount = accs.FirstOrDefault(a => Equals(a.Id, _transaction.AccountID));
                }
                else
                {
                    SelectedAccount = accs.FirstOrDefault(a => a.IsDefault);
                }

                return accs;
            }
        }
        private AccountViewModel _selectedAccount;
        public AccountViewModel SelectedAccount
        {
            get
            {
                return _selectedAccount;
            }
            set
            {
                if (!Equals(_selectedAccount, value))
                {
                    _selectedAccount = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ICategoryService _categoryService;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                var cats = new ObservableCollection<CategoryViewModel>(_categoryService?.GetCategores() ?? new List<CategoryViewModel>());

                if (_transaction != null)
                {
                    SelectedCategory = cats.FirstOrDefault(a => Equals(a.Id, _transaction.CategoryID));
                }
                else
                {
                    SelectedCategory = cats.FirstOrDefault(c => c.IsDefault);
                }

                return cats;
            }
        }
        private CategoryViewModel _selectedCategory;
        public CategoryViewModel SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                if (!Equals(_selectedCategory, value))
                {
                    _selectedCategory = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ITransactionService _transactionService;
        private TransactionViewModel _transaction;

        private TransactionType _type;

        private string _title;
        public string TransactionTitle
        {
            get
            {
                return _title;
            }
            set
            {
                if (!Equals(_title, value))
                {
                    _title = value;
                    RaisePropertyChanged();
                    CreateOrEditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _value;
        public string TransactionValue
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    double val;

                    if (double.TryParse(value, out val))
                    {
                        _value = value;
                    }

                    RaisePropertyChanged();
                    CreateOrEditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private DateTime _date;
        public DateTime TransactionDate
        {
            get
            {
                return _date;
            }
            set
            {
                if (!Equals(_date, value))
                {
                    _date = value;
                    RaisePropertyChanged();
                    CreateOrEditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _note;
        public string TransactionNote
        {
            get
            {
                return _note;
            }
            set
            {
                if (!Equals(_note, value))
                {
                    _note = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isRepeatable;
        public bool IsTransactioRepeatable
        {
            get
            {
                return _isRepeatable;
            }
            set
            {
                if (!Equals(_isRepeatable, value))
                {
                    _isRepeatable = value;
                    RaisePropertyChanged();
                }
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

        private DelegateCommand _createOrEditCommand;
        public DelegateCommand CreateOrEditCommand
        {
            get
            {
                return _createOrEditCommand ?? (_createOrEditCommand = new DelegateCommand(() => CreateOrEdit(), () => IsValid));
            }
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new DelegateCommand(() => Cancel(), () => true));
            }
        }

        public event EventHandler OperationDone;

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(TransactionTitle) || string.IsNullOrWhiteSpace(TransactionTitle))
                {
                    return false;
                }

                double value;
                if (!double.TryParse(TransactionValue, out value))
                {
                    return false;
                }


                if (_type == TransactionType.Expensess && (SelectedAccount.Balance - value < 0))
                {
                    return false;
                }

                if (SelectedAccount == null)
                {
                    return false;
                }

                if (SelectedCategory == null)
                {
                    return false;
                }

                return true;
            }
        }

        public CreateOrEditTransactionViewModel() { }

        /// <summary>
        /// Открыть транзакцию на редактирование
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="categoryService"></param>
        /// <param name="transactionService"></param>
        /// <param name="transaction"></param>
        public CreateOrEditTransactionViewModel(IAccountService accountService, ICategoryService categoryService, ITransactionService transactionService, TransactionType type, TransactionViewModel transaction = null)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _transactionService = transactionService;
            _type = type;

            if (transaction != null)
            {
                _transaction = transaction;
                _title = _transaction.Title;
                _value = String.Format("{0,12:0,000.00}", Math.Abs(_transaction.Value));
                _date = _transaction.Date;
                _note = _transaction.Note;
                _isRepeatable = _transaction.IsRepeatable;

                switch (type)
                {
                    case TransactionType.Income:
                        _headerTitle = "Редактирование операции пополнения";
                        break;
                    case TransactionType.Expensess:
                        _headerTitle = "Редактирование операции расхода";
                        break;
                }
            }
            else
            {
                _date = DateTime.Now;

                switch (type)
                {
                    case TransactionType.Income:
                        _headerTitle = "Новое пополнения";
                        break;
                    case TransactionType.Expensess:
                        _headerTitle = "Новый расхода";
                        break;
                }
            }
        }

        public void CreateOrEdit()
        {
            if (!IsValid)
            {
                return;
            }

            if (_transaction == null)
            {//Создаём новую транзакцию
                try
                {
                    double value;
                    if (!double.TryParse(TransactionValue, out value))
                    {
                        return;
                    }

                    if (_type == TransactionType.Expensess)
                    {
                        value *= -1;
                    }

                    _transactionService.CreateNew(TransactionTitle, value, TransactionDate, TransactionNote, SelectedAccount.Id, SelectedCategory.Id, IsTransactioRepeatable);

                    var account = _accountService.GetAccountById(SelectedAccount.Id);
                    account.Balance += value;
                    _accountService.Update(account);

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
            {//Редактируем транзакцию
                try
                {
                    double value;
                    if (!double.TryParse(TransactionValue, out value))
                    {
                        return;
                    }

                    if (_type == TransactionType.Expensess)
                    {
                        value *= -1;
                    }

                    bool isValueChanged = false, isAccountChanged = false;
                    var oldValue = value;
                    var oldAccount = _transaction.AccountID;

                    _transaction.Title = TransactionTitle;
                    if (!Equals(_transaction.Value, value))
                    {
                        _transaction.Value = value;
                        isValueChanged = true;
                    }

                    if (!Equals(_transaction.AccountID, SelectedAccount.Id))
                    {
                        _transaction.AccountID = SelectedAccount.Id;
                        isAccountChanged = true;
                    }

                    _transaction.CategoryID = SelectedCategory.Id;
                    _transaction.Date = TransactionDate;
                    _transaction.Note = TransactionNote;
                    _transaction.IsRepeatable = IsTransactioRepeatable;

                    _transactionService.Update(_transaction);

                    if(isAccountChanged)
                    {
                        var acc = _accountService.GetAccountById(oldAccount);
                        acc.Balance -= oldValue;
                    }

                    if (isValueChanged)
                    {
                        var acc = _accountService.GetAccountById(_transaction.AccountID);

                        if (!isAccountChanged)
                        {
                            acc.Balance -= oldValue;                            
                        }

                        acc.Balance += _transaction.Value;
                    }

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

        public void RaiseIsValidChanged(string title = null, string value = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this._title = title;
            }

            if (!string.IsNullOrEmpty(value))
            {
                this.TransactionValue = value;
            }

            CreateOrEditCommand.RaiseCanExecuteChanged();
        }

        public void TransactionTitleTextChanged(object sender, TextChangedEventArgs e)
        {
            CreateOrEditCommand.RaiseCanExecuteChanged();
        }

        public void TransactionValueTextChanged(object sender, TextChangedEventArgs e)
        {
            CreateOrEditCommand.RaiseCanExecuteChanged();
        }
    }
}
