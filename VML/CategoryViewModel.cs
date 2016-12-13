using DAL.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace VML
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private Category _object;

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

        public string IconPath
        {
            get
            {
                return _object.IconPath;
            }
            set
            {
                if (!Equals(_object.IconPath, value))
                {
                    _object.IconPath = value;
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

        public CategoryViewModel(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _object = category;
        }
    }
}
