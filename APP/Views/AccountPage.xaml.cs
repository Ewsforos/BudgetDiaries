using APP.ViewModels;
using BLL;
using CDL.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using VML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    public sealed partial class AccountPage : Page
    {
        private AccountViewModel _selectedAccount;
        private IAccountService _accService;

        public AccountPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!String.IsNullOrEmpty(e.Parameter?.ToString()))
            {
                var param = JsonConvert.DeserializeObject<NavigationParametrs>(e.Parameter.ToString());

                if (_accService == null)
                {
                    _accService = IoC.Resolve<IAccountService>();
                }

                var idStr = param.Data.Replace("\"", "");
                var id = new Guid(idStr);
                _selectedAccount = _accService.GetAccountById(id);
            }

            UpdateForVisualState(AdaptiveStates.CurrentState);

            DisableContentTransitions();
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
        {
            var isNarrow = newState == VisualStateNarrow;

            if (isNarrow && oldState == VisualStateNormal && _selectedAccount != null)
            {
                // Resize down to the detail item. Don't play a transition.
                //Frame.Navigate(typeof(DetailPage), _lastSelectedItem.ItemId, new SuppressNavigationTransitionInfo());
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            MasterListView.SelectedItem = _selectedAccount;
        }

        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        private void DisableContentTransitions()
        {
            if (DetailContentPresenter != null)
            {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void transactionsViewTypeSelectButton_Checked(object sender, RoutedEventArgs e)
        {
            transactionViewTypeSelectPopUp.IsOpen = true;
        }

        private void TopMenuSecondButton_Click(object sender, RoutedEventArgs e)
        {
            var content = transactionsViewTypeSelectButton.Content;
            transactionsViewTypeSelectButton.Content = (sender as Button).Content;
            (sender as Button).Content = content;

            transactionsViewTypeSelectButton.IsChecked = false;
        }

        private void transactionsViewTypeSelectButton_Unchecked(object sender, RoutedEventArgs e)
        {
            transactionViewTypeSelectPopUp.IsOpen = false;
            HideTransactionTypeView.Begin();
        }

        private void transactionViewTypeSelectPopUp_Closed(object sender, object e)
        {
            transactionsViewTypeSelectButton.IsChecked = false;
        }

        private void transactionViewTypeSelectPopUp_Opened(object sender, object e)
        {
            ShowTransactionTypeView.Begin();
        }

        private void appBarPeriod_Checked(object sender, RoutedEventArgs e)
        {
            transactionPeriodSelectPopUp.IsOpen = true;
        }

        private void appBarPeriod_Unchecked(object sender, RoutedEventArgs e)
        {
            transactionPeriodSelectPopUp.IsOpen = false;
        }

        private void transactionPeriodSelectPopUp_Closed(object sender, object e)
        {
            HidePeriodChooser.Begin();
        }

        private void transactionPeriodSelectPopUp_Opened(object sender, object e)
        {
            ShowPeriodChooser.Begin();
        }

        private void dayPeriodType_Checked(object sender, RoutedEventArgs e)
        {
            if (weekPeriodType.IsChecked.HasValue && weekPeriodType.IsChecked.Value)
            {
                weekPeriodType.IsChecked = false;
            }
            if (customPeriodType.IsChecked.HasValue && customPeriodType.IsChecked.Value)
            {
                customPeriodType.IsChecked = false;
            }

            if (PeriodChooser.SelectedDates?.Any() ?? false)
            {
                while (PeriodChooser.SelectedDates.Count > 1)
                {
                    PeriodChooser.SelectedDates.RemoveAt(1);
                }
            }

            PeriodChooser.SelectionMode = CalendarViewSelectionMode.Single;
        }

        private void dayPeriodType_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void weekPeriodType_Checked(object sender, RoutedEventArgs e)
        {
            if (dayPeriodType.IsChecked.HasValue && dayPeriodType.IsChecked.Value)
            {
                dayPeriodType.IsChecked = false;
            }
            if (customPeriodType.IsChecked.HasValue && customPeriodType.IsChecked.Value)
            {
                customPeriodType.IsChecked = false;
            }

            if (PeriodChooser.SelectedDates?.ToList().Any() ?? false)
            {
                while (PeriodChooser.SelectedDates.Count > 1)
                {
                    PeriodChooser.SelectedDates.RemoveAt(1);
                }

                var firstDay = PeriodChooser.SelectedDates.First();
                switch (firstDay.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(1));
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(2));
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(3));
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(4));
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(5));
                        PeriodChooser.SelectedDates.Add(firstDay.AddDays(6));
                        break;
                }
            }
        }

        private void weekPeriodType_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void customPeriodType_Checked(object sender, RoutedEventArgs e)
        {
            if (dayPeriodType.IsChecked.HasValue && dayPeriodType.IsChecked.Value)
            {
                dayPeriodType.IsChecked = false;
            }
            if (weekPeriodType.IsChecked.HasValue && weekPeriodType.IsChecked.Value)
            {
                weekPeriodType.IsChecked = false;
            }

            PeriodChooser.SelectionMode = CalendarViewSelectionMode.Multiple;
        }

        private void customPeriodType_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void applySelection_Click(object sender, RoutedEventArgs e)
        {
            if (dayPeriodType.IsChecked.HasValue && dayPeriodType.IsChecked.Value)
            {
                var day = PeriodChooser.SelectedDates.FirstOrDefault();

                if (day == null)
                {
                    return;
                }

                ViewModel.Filter = t => Equals(t.Date.Date, day);

                return;
            }

        }

        private void AccountTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (AccountNumberTextBox == null)
            //{
            //    return;
            //}

            //switch((AccountTypeComboBox.SelectedItem as ComboBoxItem).Name)
            //{
            //    case "AccountTypeCash":
            //        AccountNumberTextBox.Visibility = Visibility.Collapsed;
            //        break;
            //    case "AccountTypeBankAccount":
            //    case "AccountTypeCard":
            //        AccountNumberTextBox.Visibility = Visibility.Visible;
            //        break;
            //}            
        }

        private void AccountNumberTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            //if (!AccountNumberTextBox.Text.Any())
            //{
            //    return;
            //}

            //var last = AccountNumberTextBox.Text.Last();


            //if (last < '0' || last > '9' || AccountNumberTextBox.Text.Length > 20)
            //{
            //    AccountNumberTextBox.Text = AccountNumberTextBox.Text.Remove(AccountNumberTextBox.Text.Length - 1);
            //    AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
            //    return;
            //}

            //if ((AccountTypeComboBox.SelectedItem as ComboBoxItem).Name == "AccountTypeBankAccount")
            //{
            //    return;
            //}

            //if(AccountNumberTextBox.Text.Length > 19)
            //{
            //    AccountNumberTextBox.Text = AccountNumberTextBox.Text.Remove(AccountNumberTextBox.Text.Length - 1);
            //    AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
            //    return;
            //}

            //var text = new StringBuilder(AccountNumberTextBox.Text);
            //text.Replace(" ", "");
            //var trim = new StringBuilder(text.ToString());

            //if (text.Length >= 4)
            //{
            //    trim.Insert(4, ' ');
            //}
            //if (text.Length >= 9)
            //{
            //    trim.Insert(9, ' ');
            //}
            //if (text.Length >= 13)
            //{
            //    trim.Insert(14, ' ');
            //}

            //AccountNumberTextBox.Text = trim.ToString();
            //AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
        }

        private void SaveAccount_Click(object sender, RoutedEventArgs e)
        {
            var accountService = IoC.Resolve<IAccountService>();

            //accountService.CreateNewAccount(AccountNameTextBox.Text,)
        }

        private bool isAccountValid()
        {
            //if (string.IsNullOrEmpty(AccountNameTextBox.Text))
            //{
            //    return false;
            //}

            //if((AccountTypeComboBox.SelectedItem as ComboBoxItem).Name!= "AccountTypeCash" && string.IsNullOrEmpty(AccountNumberTextBox.Text))
            //{
            //    return false;
            //}



            return true;
        }

        private void appBarIncomeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
