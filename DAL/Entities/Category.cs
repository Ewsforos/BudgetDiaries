using System.Collections.ObjectModel;

namespace DAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public bool IsDefault { get; set; }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            set
            {
                _transactions = value;
                if (_transactions != null)
                {
                    _transactions.CollectionChanged += Transactions_CollectionChanged;
                }
            }
        }

        public Category()
        {
            Transactions = new ObservableCollection<Transaction>();
        }

        private void Transactions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Transaction).FK_Categoty = this.PK_ID;
            }
        }
    }
}
