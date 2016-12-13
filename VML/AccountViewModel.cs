using CDL.Enums;
using DAL.Entities;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VML
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private Account _object;

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            var handler = this.PropertyChanged;
            //if is not null
            if (!object.Equals(handler, null))
            {
                var args = new PropertyChangedEventArgs(propertyName);
                try
                {
                    handler.Invoke(this, args);
                }
                catch
                { }
            }
        }
        #endregion

        public Guid Id
        {
            get
            {
                return _object.PK_ID;
            }
        }

        public string Name
        {
            get
            {
                return _object.Name;
            }
            set
            {
                if (!Equals(_object.Name, value))
                {
                    _object.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NameWrapMaxLines
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                {
                    return 2;
                }

                return 1;
            }
        }

        public string Currency
        {
            get
            {
                return _object.Currency;
            }
            set
            {
                if (!Equals(_object.Currency, value))
                {
                    _object.Currency = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double Balance
        {
            get
            {
                return _object.Balance;
            }
            set
            {
                if (!Equals(_object.Balance, value))
                {
                    _object.Balance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BalanceView
        {
            get
            {
                return String.Format("{0,12:0,000.00}", Balance);
            }
        }

        public string Number
        {
            get
            {
                return _object.Number;
            }
            set
            {
                if (!Equals(_object.Number, value))
                {
                    _object.Number = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string NumberView
        {
            get
            {
                if (Type == AccountType.Card)
                {
                    StringBuilder num = new StringBuilder();
                    var parts = Number.Split(' ');
                    num.Append(parts.First());
                    num.Append(" **** **** ");
                    num.Append(parts.Last());

                    return num.ToString();
                }

                return Number;
            }
        }

        public Visibility IsNumberVisible
        {
            get
            {
                if (string.IsNullOrEmpty(Number))
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        private AccountType? _type;
        public AccountType Type
        {
            get
            {
                if (!_type.HasValue)
                {
                    if (Equals(_object.Type, AccountType.BankAccount.ToString()))
                    {
                        _type = AccountType.BankAccount;
                    }
                    if (Equals(_object.Type, AccountType.Card.ToString()))
                    {
                        _type = AccountType.Card;
                    }
                    if (Equals(_object.Type, AccountType.Cash.ToString()))
                    {
                        _type = AccountType.Cash;
                    }
                }

                return _type.Value;
            }
            set
            {
                if (!Equals(_object.Type, value.ToString()))
                {
                    _object.Type = value.ToString();
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDefault
        {
            get
            {
                return _object.IsDefault;
            }
            set
            {
                if (!Equals(_object.IsDefault, value))
                {
                    _object.IsDefault = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(IsDefaultVisibility));
                }
            }
        }

        private SolidColorBrush _background;
        public SolidColorBrush BackgroundBrush
        {
            get
            {
                return _background;
            }
            set
            {
                if (!Equals(_background, value))
                {
                    _background = value;
                    RaisePropertyChanged();
                }
            }
        }

        private SolidColorBrush _foreground;
        public SolidColorBrush ForegroundBrush
        {
            get
            {
                return _foreground;
            }
            set
            {
                if (!Equals(_foreground, value))
                {
                    _foreground = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility IsDefaultVisibility
        {
            get
            {
                if (IsDefault)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public AccountViewModel(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _object = account;
        }
    }
}
