using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VML;

namespace BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CategoryViewModel> GetCategores()
        {
            return (from c in _unitOfWork.Repository<Category>().Items()
                    select new CategoryViewModel(c)).ToList();
        }

        public CategoryViewModel GetCategoryById(Guid id)
        {
            var category = _unitOfWork.Repository<Category>().Where(a => Equals(a.PK_ID, id)).FirstOrDefault();

            if (category == null)
            {
                throw new NullReferenceException($"Categoty with ID={id.ToString()} do not exist :(");
            }

            return new CategoryViewModel(category);
        }

        public void Remove(CategoryViewModel category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Categoty can not be null");
            }

            _unitOfWork.Repository<Category>().Remove(category.Id);

            //var tUoW = _unitOfWork.Repository<Transaction>();

            //var toRemove = tUoW.Where(t => Equals(t.FK_Categoty, category.Id)).ToList();
            //foreach (var transaction in toRemove)
            //{
            //transaction.
            //}

            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }

        public void CreateNew(string name, string iconPath, bool isDefault = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Categoty Name can not be null or empty");
            }

            if (string.IsNullOrEmpty(iconPath))
            {
                throw new ArgumentNullException("Categoty IconPath can not be null or empty");
            }

            var category = new Category()
            {
                Name = name,
                IconPath = iconPath,
                IsDefault = isDefault
            };

            if (isDefault)
            {
                _unitOfWork.Repository<Category>().Where(c => c.IsDefault).ToList().ForEach(c => c.IsDefault = false);
            }

            _unitOfWork.Repository<Category>().Add(category);

            var result = _unitOfWork.SaveChanges();

            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
        }

        public void Update(CategoryViewModel category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Categoty can not be null");
            }

            if (category.IsDefault)
            {
                _unitOfWork.Repository<Category>().Where(a => a.IsDefault).ToList().ForEach(a => a.IsDefault = false);
            }

            _unitOfWork.Repository<Category>().Update(category.Id);
            _unitOfWork.SaveChanges();
        }
    }
}
