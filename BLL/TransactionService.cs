using CDL.Enums;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VML;

namespace BLL
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int _itemsCount = 30;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TransactionViewModel> GetTransactions()
        {
            return (from t in _unitOfWork.Repository<Transaction>().Items()
                    select new TransactionViewModel(t)).ToList();
        }

        public TransactionViewModel GetTransactionById(Guid id)
        {
            var transaction = _unitOfWork.Repository<Transaction>().Where(a => Equals(a.PK_ID, id)).FirstOrDefault();

            if (transaction == null)
            {
                throw new NullReferenceException($"Transaction with ID={id.ToString()} do not exist :(");
            }

            return new TransactionViewModel(transaction);
        }

        /// <summary>
        /// Возвращает список последних операций с заданным смещением
        /// </summary>
        /// <param name="offset">Смещение</param>
        /// <returns></returns>
        public IEnumerable<TransactionViewModel> GetRecentTransactions(int? offset = null)
        {
            if (offset.HasValue)
            {
                return (from t in _unitOfWork.Repository<Transaction>().Items().Take(_itemsCount * offset.Value)
                        select new TransactionViewModel(t)).ToList();
            }
            else
            {
                return (from t in _unitOfWork.Repository<Transaction>().Items().Take(_itemsCount)
                        select new TransactionViewModel(t)).ToList();
            }
        }

        #region CRUD
        public void Update(TransactionViewModel transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("Transaction can not be null");
            }

            _unitOfWork.Repository<Transaction>().Update(transaction.ID);
            _unitOfWork.SaveChanges();
        }

        public void Remove(TransactionViewModel transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("Transaction can not be null");
            }

            _unitOfWork.Repository<Transaction>().Remove(transaction.ID);

            _unitOfWork.SaveChanges();
        }

        public void CreateNew(string title, double value, DateTime date, string note, Guid accountId, Guid categoryId, bool isRepeatable = false, DateTime? dateFrom = null, DateTime? dateTo = null, RepeatTypes repeatType = RepeatTypes.Non)
        {
            var transaction = new Transaction()
            {
                Title = title,
                Value = value,
                Date = date,
                Note = note,
                FK_Account = accountId,
                FK_Categoty = categoryId,
                IsRepeatable = isRepeatable
            };

            if (isRepeatable)
            {
                transaction.DateFrom = dateFrom;
                transaction.DateTo = dateTo;
                transaction.RepeatType = repeatType.ToString();
            }

            _unitOfWork.Repository<Transaction>().Add(transaction);
            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }
        #endregion
    }
}
