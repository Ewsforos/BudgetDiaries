using System;
using System.Collections.Generic;
using VML;

namespace BLL
{
    public interface ICategoryService
    {
        /// <summary>
        /// Возвращает список всех категорий
        /// </summary>
        /// <returns>List<CategotyViewModel> Список категорий</returns>
        IEnumerable<CategoryViewModel> GetCategores();

        /// <summary>
        /// Возвращает категорию по указанному ID
        /// </summary>
        /// <param name="id">Уникальный идентификатор категории</param>
        /// <returns>Категория</returns>
        CategoryViewModel GetCategoryById(Guid id);

        /// <summary>
        /// Удаляет категорию
        /// </summary>
        /// <param name="account">Удаляемая категория</param>
        void Remove(CategoryViewModel category);

        /// <summary>
        /// Создаёт новую категорию
        /// </summary>
        /// <param name="category"></param>
        void CreateNew(string name, string iconPath, bool isDefault);

        /// <summary>
        /// Сохраняет изменения в категории
        /// </summary>
        /// <param name="сategoty">Изменяемая категория</param>
        void Update(CategoryViewModel category);
    }
}
