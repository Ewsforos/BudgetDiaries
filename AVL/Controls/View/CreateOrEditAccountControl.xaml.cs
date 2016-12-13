using CDL.Classes;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AVL.Controls
{
    public sealed partial class CreateOrEditAccountControl : UserControl
    {
        public CreateOrEditAccountControl()
        {
            this.InitializeComponent();
        }

        private void AccountNumberTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (!AccountNumberTextBox.Text.Any())
            {
                return;
            }

            var last = AccountNumberTextBox.Text.Last();


            if (last < '0' || last > '9' || AccountNumberTextBox.Text.Length > 20)
            {
                AccountNumberTextBox.Text = AccountNumberTextBox.Text.Remove(AccountNumberTextBox.Text.Length - 1);
                AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
                return;
            }

            if ((AccountTypeComboBox.SelectedItem as AccountTypes).Name == "AccountTypeBankAccount")
            {
                return;
            }

            if (AccountNumberTextBox.Text.Length > 19)
            {
                AccountNumberTextBox.Text = AccountNumberTextBox.Text.Remove(AccountNumberTextBox.Text.Length - 1);
                AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
                return;
            }

            var text = new StringBuilder(AccountNumberTextBox.Text);
            text.Replace(" ", "");
            var trim = new StringBuilder(text.ToString());

            if (text.Length >= 4)
            {
                trim.Insert(4, ' ');
            }
            if (text.Length >= 9)
            {
                trim.Insert(9, ' ');
            }
            if (text.Length >= 13)
            {
                trim.Insert(14, ' ');
            }

            AccountNumberTextBox.Text = trim.ToString();
            AccountNumberTextBox.SelectionStart = AccountNumberTextBox.Text.Length;
        }

        private void AccountNameTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {

        }

        public void SetContent(CreateOrEditAccountViewModel model)
        {
            this.DataContext = model;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountNameTextBox.Text))
            {
                AccountNameTextBox.SelectAll();
            }

            AccountNameTextBox.Focus(FocusState.Keyboard);
        }
    }
}
