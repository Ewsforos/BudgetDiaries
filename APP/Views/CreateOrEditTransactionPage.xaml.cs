using AVL.Controls;
using BLL;
using CDL.Enums;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateOrEditTransactionPage : Page
    {
        public CreateOrEditTransactionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var serializationService = Template10.Services.SerializationService.SerializationService.Json;

            base.OnNavigatedTo(e);

            var accountService = IoC.Resolve<IAccountService>();
            var categoryService = IoC.Resolve<ICategoryService>();
            var transactionService = IoC.Resolve<ITransactionService>();

            CreateOrEditTransactionViewModel CoETVM = null;

            if (e.Parameter != null)
            {
                var param = serializationService.Deserialize(e.Parameter?.ToString());

                Guid transactionId;
                if (Guid.TryParse(param.ToString(), out transactionId))
                {
                    var transaction = transactionService.GetTransactionById(transactionId);
                    if (transaction != null)
                    {
                        if (transaction.Value >= 0)
                        {
                            CoETVM = new CreateOrEditTransactionViewModel(accountService, categoryService, transactionService, TransactionType.Income, transaction);
                        }
                        else
                        {
                            CoETVM = new CreateOrEditTransactionViewModel(accountService, categoryService, transactionService, TransactionType.Expensess, transaction);
                        }
                    }
                }
                else
                {
                    CoETVM = new CreateOrEditTransactionViewModel(accountService, categoryService, transactionService, (TransactionType)param);
                }
            }

            CoETVM.OperationDone += CoEAVM_OperationDone;

            CreateOrEditControl.SetContent(CoETVM);
        }

        private void CoEAVM_OperationDone(object sender, EventArgs e)
        {
            if (Shell.HamburgerMenu.NavigationService.CanGoBack)
            {
                Shell.HamburgerMenu.NavigationService.GoBack();
                return;
            }

            Shell.HamburgerMenu.NavigationService.Navigate(typeof(MainPage));
        }
    }
}
