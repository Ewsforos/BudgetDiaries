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
    public class CategoriesPageViewModel : BindableBase
    {
        private ICategoryService _service { get; }

        private SolidColorBrush _accentBrush = null;
        private SolidColorBrush _backgroundBrush = null;
        private SolidColorBrush _foregroundBrush = null;

        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                var cats = new ObservableCollection<CategoryViewModel>(_service?.GetCategores() ?? new List<CategoryViewModel>());

                if (_accentBrush != null)
                {
                    var i = -1;
                    foreach (var cat in cats)
                    {
                        if (i > 0)
                        {
                            cat.BackgroundBrush = null;
                        }
                        else
                        {
                            cat.BackgroundBrush = _backgroundBrush;
                        }

                        if (!cat.IsDefault)
                        {
                            cat.ForegroundBrush = _foregroundBrush;
                        }
                        else
                        {
                            cat.ForegroundBrush = _accentBrush;
                            Selected = cat;
                        }

                        i *= -1;
                    }
                }

                return cats;
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

        private bool _isCategoryDetailsLoading;
        public bool IsCategoryDetailsLoading
        {
            get
            {
                return _isCategoryDetailsLoading;
            }
            set
            {
                if (!Equals(_isCategoryDetailsLoading, value))
                {
                    _isCategoryDetailsLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isCategoriesLoading;
        public bool IsCategoriesLoading
        {
            get
            {
                return _isCategoriesLoading;
            }
            set
            {
                if (!Equals(_isCategoriesLoading, value))
                {
                    _isCategoriesLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        DelegateCommand<CategoryViewModel> _deleteCategoryCommand;
        public DelegateCommand<CategoryViewModel> DeleteCategoryCommand
            => _deleteCategoryCommand ?? (_deleteCategoryCommand = new DelegateCommand<CategoryViewModel>(async (CategoryViewModel category) =>
            {
                await DeleteCategory(category);
            }, (CategoryViewModel param) => true));
        
        DelegateCommand<CategoryViewModel> _editCategoryCommand;
        public DelegateCommand<CategoryViewModel> EditCategoryCommand
            => _editCategoryCommand ?? (_editCategoryCommand = new DelegateCommand<CategoryViewModel>((CategoryViewModel category) =>
            {
                EditCategory(category);
            }, (CategoryViewModel param) => true));

        public CategoriesPageViewModel()
        {
            _service = IoC.Resolve<ICategoryService>();
        }

        public void SetBrushes(SolidColorBrush accentBrush, SolidColorBrush backgroundBrush, SolidColorBrush foregroundBrush)
        {
            _accentBrush = accentBrush;

            _backgroundBrush = backgroundBrush;
            _backgroundBrush.Opacity = 0.2;

            _foregroundBrush = foregroundBrush;

            RaisePropertyChanged("Categories");
        }

        public void UpdateCategoriesView()
        {
            RaisePropertyChanged("Categories");
        }

        public void UpdateSelectedCategory()
        {
            RaisePropertyChanged("Selected");
        }

        private async Task DeleteCategory(CategoryViewModel category)
        {
            var dialog = new MessageDialog($"Вы действительно хотите удалить категорию {category.Name} и перевести все операции, привязанные к этой категории, в не распределённые?");
            dialog.Title = "Внимание";
            dialog.Commands.Add(new UICommand { Label = "Да", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Нет", Id = 1 });
            dialog.DefaultCommandIndex = 0;

            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                try
                {
                    IoC.Resolve<ICategoryService>().Remove(category);

                    UpdateCategoriesView();
                }
                catch (Exception ex) { }
            }
        }
        private void EditCategory(CategoryViewModel category)
        {
            Shell.HamburgerMenu.NavigationService.Navigate(typeof(CreateOrEditCategoryPage), category.Id);
        }
    }
}
