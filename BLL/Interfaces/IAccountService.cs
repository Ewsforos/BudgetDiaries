using CDL.Enums;
using System;
using System.Collections.Generic;
using VML;

namespace BLL
{
    public interface IAccountService
    {
        /// <summary>
        /// Возвращает список всех счетов
        /// </summary>
        /// <returns>List<Account> Список счетов</returns>
        IEnumerable<AccountViewModel> GetAccounts();

        /// <summary>
        /// Возвращает счёт по указанному ID
        /// </summary>
        /// <param name="id">Уникальный идентификатор счёта</param>
        /// <returns>Счёт</returns>
        AccountViewModel GetAccountById(Guid id);

        /// <summary>
        /// Создаёт новый аакаунт
        /// </summary>
        /// <param name="account"></param>
        void CreateNewAccount(string name, AccountType type, string number, string currency, double balance, bool isDefauld);

        /// <summary>
        /// Сохраняет изменения в счёте
        /// </summary>
        /// <param name="account">Изменяемый счёт</param>
        void Update(AccountViewModel account);

        /// <summary>
        /// Удаляет счёт и все операции привязанные к этому счёту
        /// </summary>
        /// <param name="account">Удаляемый счёт</param>
        void Remove(AccountViewModel account);
    }
}
