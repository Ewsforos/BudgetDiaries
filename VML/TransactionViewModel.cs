using CDL.Enums;
using DAL.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VML
{
    public class TransactionViewModel : INotifyPropertyChanged
    {
        private Transaction _object;

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

        public TransactionViewModel(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            _object = transaction;
        }

        public Guid ID
        {
            get
            {
                return _object.PK_ID;
            }
        }

        public double Value
        {
            get
            {
                return _object.Value;
            }
            set
            {
                if (!Equals(_object.Value, value))
                {
                    _object.Value = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ValueView
        {
            get
            {
                return String.Format("{0,12:0.00}", Math.Abs(Value));
            }
        }

        public string Type
        {
            get
            {
                if (Value > 0)
                {
                    return "+";
                }
                else
                {
                    return "-";
                }
            }
        }

        public DateTime Date
        {
            get
            {
                return _object.Date;
            }
            set
            {
                if (!Equals(_object.Date, value))
                {
                    _object.Date = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DateView
        {
            get
            {
                return _object.Date.ToString();
            }
        }

        public string Title
        {
            get
            {
                return _object.Title;
            }
            set
            {
                if (!Equals(_object.Title, value))
                {
                    _object.Title = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Note
        {
            get
            {
                return _object.Note;
            }
            set
            {
                if (!Equals(_object.Note, value))
                {
                    _object.Note = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsRepeatable
        {
            get
            {
                return _object.IsRepeatable;
            }
            set
            {
                if (!Equals(_object.IsRepeatable, value))
                {
                    _object.IsRepeatable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility IsRepeatIconVisible
        {
            get
            {
                if (IsRepeatable)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
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

        public DateTime? DateFrom
        {
            get
            {
                return _object.DateFrom;
            }
            set
            {
                if (!Equals(_object.DateFrom, value))
                {
                    _object.DateFrom = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DateTime? DateTo
        {
            get
            {
                return _object.DateTo;
            }
            set
            {
                if (!Equals(_object.DateTo, value))
                {
                    _object.DateTo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RepeatTypes RepeatType
        {
            get
            {
                switch (_object.RepeatType)
                {
                    case "EveryDay":
                        return RepeatTypes.EveryDay;
                    case "EveryWeek":
                        return RepeatTypes.EveryWeek;
                    case "EveryMonth":
                        return RepeatTypes.EveryMonth;
                    case "EveryYear":
                        return RepeatTypes.EveryYear;
                    default:
                        return RepeatTypes.Non;
                }
            }
            set
            {
                if (!Equals(_object.RepeatType, value.ToString()))
                {
                    _object.RepeatType = value.ToString();
                    RaisePropertyChanged();
                }
            }
        }

        public Guid AccountID
        {
            get
            {
                return _object.FK_Account;
            }
            set
            {
                if (!Equals(_object.FK_Account, value))
                {
                    _object.FK_Account = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Guid CategoryID
        {
            get
            {
                return _object.FK_Categoty;
            }
            set
            {
                if (!Equals(_object.FK_Categoty, value))
                {
                    _object.FK_Categoty = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
