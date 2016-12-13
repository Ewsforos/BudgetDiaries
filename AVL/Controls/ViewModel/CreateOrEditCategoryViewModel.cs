using BLL;
using CDL.Classes;
using CDL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Mvvm;
using VML;

namespace AVL.Controls
{
    public class CreateOrEditCategoryViewModel : BindableBase
    {
        private ICategoryService _categoryService;

        private CategoryViewModel _category;
        public event EventHandler OperationDone;

        private string _categoryName;
        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                if (!Equals(_categoryName, value))
                {
                    _categoryName = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        private bool _categoryIsDefault;
        public bool CategoryIsDefault
        {
            get
            {
                return _categoryIsDefault;
            }
            set
            {
                if (!Equals(_categoryIsDefault, value))
                {
                    _categoryIsDefault = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CreateOrEditCommand));
                }
            }
        }

        private List<string> _iconsSource;
        public List<string> IconsSource
        {
            get
            {
                return _iconsSource;
            }
            set
            {
                _iconsSource = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedIcon { get; set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(CategoryName))
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

        public CreateOrEditCategoryViewModel(ICategoryService service)
        {
            _categoryService = service;
            _category = null;

            _categoryName = "";
            _categoryIsDefault = false;

            _headerTitle = "Новая категория";

            SetConstantLists();
        }

        public CreateOrEditCategoryViewModel(ICategoryService service, CategoryViewModel category)
        {
            _categoryService = service;
            _category = category;

            _categoryName = _category.Name;
            _categoryIsDefault = _category.IsDefault;

            SetConstantLists();

            var item = IconsSource.FirstOrDefault(x => Equals(x, _category.IconPath));
            if (item == null)
            {
                IconsSource.Add(item);
            }

            SelectedIcon = item;

            _headerTitle = "Редактирование категории";
        }

        private void SetConstantLists()
        {
            _iconsSource = new List<string>();
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/non.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Animals.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Bills.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Car.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Cash.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Clothing.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Entertaiment.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/FastFood.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Food.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Gift.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Healthcare.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Organization.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Phone.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Savings.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Sports.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Taxi.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Transport.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Travel.png");
            _iconsSource.Add("ms-appx:///Assets/CategoriesIcons/Washing.png");

            SelectedIcon = IconsSource.First();
        }

        public void CreateOrEdit()
        {
            if (!IsValid)
            {
                return;
            }

            if (_category == null)
            {//Создаём новую категорию
                try
                {
                    _categoryService.CreateNew(CategoryName, SelectedIcon, CategoryIsDefault);

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

                try
                {
                    _category.Name = CategoryName;
                    _category.IsDefault = CategoryIsDefault;
                    _category.IconPath = SelectedIcon;

                    _categoryService.Update(_category);

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
