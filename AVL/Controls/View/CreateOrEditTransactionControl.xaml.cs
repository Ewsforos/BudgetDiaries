using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AVL.Controls
{
    public sealed partial class CreateOrEditTransactionControl : UserControl
    {
        public CreateOrEditTransactionViewModel ViewModel { get; set; }

        public CreateOrEditTransactionControl()
        {
            this.InitializeComponent();
        }

        public void SetContent(CreateOrEditTransactionViewModel model)
        {
            this.DataContext = ViewModel = model;
        }

        private void TransactionTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.RaiseIsValidChanged(title:(sender as TextBox).Text);
        }

        private void TransactionValueTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            ViewModel.RaiseIsValidChanged(value:(sender as TextBox).Text);
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }
    }
}
