using CDL.Enums;
using System;
using System.Collections.Generic;
using VML;

namespace BLL
{
    public interface ITransactionService
    {
        /// <summary>
        /// Возвращает список всех операций
        /// </summary>
        /// <returns>List<Transaction> Список операций</returns>
        IEnumerable<TransactionViewModel> GetTransactions();

        /// <summary>
        /// Возвращает операцию по указанному ID
        /// </summary>
        /// <param name="id">Уникальный идентификатор операции</param>
        /// <returns>Операция</returns>
        TransactionViewModel GetTransactionById(Guid id);

        /// <summary>
        /// Создаёт новую транзакцию
        /// </summary>
        /// <param name="title">Заголовок операции</param>
        /// <param name="value">Сумма транзакции; + если операция пополнения и - если операция расхода</param>
        /// <param name="date">Дата транзакции</param>
        /// <param name="note">Примечание к транзакции</param>
        /// <param name="accountId">Уникальный идентификатор счёта по которому осуществлена проводка транзакции</param>
        /// <param name="categoryId">Уникальный идентификатор категории к которой относится транзакция</param>
        /// <param name="isRepeatable">Является ли транзакция разовой=false или повторяющейся = true</param>
        /// <param name="dateFrom">Если операция повторяющаято, то это поле - дата первой транзакции</param>
        /// <param name="dateTo">Если операция повторяющаято, то это поле - дата последней транзакции</param>
        /// <param name="repeatType">Если операция повторяющаято, то это поле указывает на париод повторения транзакции</param>
        void CreateNew(string title, double value, DateTime date, string note, Guid accountId, Guid categoryId, bool isRepeatable = false, DateTime? dateFrom = null, DateTime? dateTo = null, RepeatTypes repeatType = RepeatTypes.Non);

        /// <summary>
        /// Сохраняет изменения в операции
        /// </summary>
        /// <param name="transaction">Изменяемый счёт</param>
        void Update(TransactionViewModel transaction);

        /// <summary>
        /// Удаляет операцию
        /// </summary>
        /// <param name="account">Удаляемая операция</param>
        void Remove(TransactionViewModel transaction);
    }
}
