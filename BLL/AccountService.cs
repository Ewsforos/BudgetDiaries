using CDL.Enums;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VML;

namespace BLL
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AccountViewModel> GetAccounts()
        {
            return (from a in _unitOfWork.Repository<Account>().Items()
                    select new AccountViewModel(a)).ToList();
        }

        public AccountViewModel GetAccountById(Guid id)
        {
            var account = _unitOfWork.Repository<Account>().Where(a => Equals(a.PK_ID, id)).FirstOrDefault();

            if (account == null)
            {
                throw new NullReferenceException($"Account with ID={id.ToString()} do not exist :(");
            }

            return new AccountViewModel(account);
        }

        public void CreateNewAccount(string name, AccountType type, string number, string currency = "rur", double balance = 0, bool isDefault = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Account name can't be null or empty");
            }

            if (type == AccountType.BankAccount || type == AccountType.Card && string.IsNullOrEmpty(number))
            {
                throw new ArgumentException("Account number can't be null or empty");
            }

            var account = new Account()
            {
                Name = name,
                Type = type.ToString(),
                Number = number,
                Balance = balance,
                IsDefault = isDefault
            };

            if (isDefault)
            {
                _unitOfWork.Repository<Account>().Where(a => a.IsDefault).ToList().ForEach(a => a.IsDefault = false);
            }

            _unitOfWork.Repository<Account>().Add(account);

            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }

        public void Update(AccountViewModel account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("Account can not be null");
            }

            if (account.IsDefault)
            {
                _unitOfWork.Repository<Account>().Where(a => a.IsDefault).ToList().ForEach(a => a.IsDefault = false);
            }

            _unitOfWork.Repository<Account>().Update(account.Id);

            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }

        public void Remove(AccountViewModel account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("Account can not be null");
            }

            _unitOfWork.Repository<Account>().Remove(account.Id);

            var tUoW = _unitOfWork.Repository<Transaction>();

            var toRemove = tUoW.Where(t => Equals(t.FK_Account, account.Id)).ToList();
            foreach (var transaction in toRemove)
            {
                tUoW.Remove(transaction.PK_ID);
            }

            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }
    }
}
