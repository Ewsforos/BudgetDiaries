using System.Collections.ObjectModel;

namespace DAL.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string Currency { get; set; }
        public double Balance { get; set; }
        public bool IsDefault { get; set; }

        public ObservableCollection<Transaction> Transactions
        {
            get;
            set;
        }

        public Account()
        {
            Transactions = new ObservableCollection<Transaction>();
            Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        private void Transactions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Transaction).FK_Account = this.PK_ID;
            }
        }
    }
}
