using System;
using System.Linq.Expressions;
using VML;

namespace APP.ViewModels
{
    public class AccountPageViewModel
    {
        public Expression<Func<TransactionViewModel, bool>> Filter;
    }
}
