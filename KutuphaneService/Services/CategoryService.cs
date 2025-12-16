using KutuphaneCore.Entities;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryResository;

        public CategoryService(IGenericRepository<Category> categoryResository)
        {
            _categoryResository = categoryResository;
        }


        public Task<IResponse<Category>> Create(Category category)
        {
            if (category == null)
            {
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bilgileri boş olamaz."));
            }

            _categoryResository.Create(category);

            return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Success(category, "Kategori başarıyla oluşturuldu."));
        }

        public IResponse<Category> Delete(int id)
        {
            var category = _categoryResository.GetByIdAsync(id).Result;
            
            if (category == null)
            {
                return ResponseGeneric<Category>.Error("Kategori bulunamadı.");
            }

            _categoryResository.Delete(category);

            return ResponseGeneric<Category>.Success(category, "Kategori başarıyla silindi.");
        }

        public IResponse<Category> GetById(int id)
        {
            var category = _categoryResository.GetByIdAsync(id).Result;

            if (category == null)
            {
                return ResponseGeneric<Category>.Success(null, "Kategori bulunamadı.");
            }
            return ResponseGeneric<Category>.Success(category, "Kategori başarıyla bulundu.");
        }

        public IResponse<IEnumerable<Category>> GetByName(string name)
        {
            var categories = _categoryResository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            if(categories == null || categories.Count == 0)
            {
                return ResponseGeneric<IEnumerable<Category>>.Error("Kategori bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<Category>>.Success(categories, "Kategoriler listelendi.");
        }

        public IResponse<IEnumerable<Category>> ListAll()
        {
            var categories = _categoryResository.GetAll().ToList();

            if(categories == null || categories.Count == 0)
            {
                return ResponseGeneric<IEnumerable<Category>>.Error("Kategori bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<Category>>.Success(categories, "Kategoriler listelendi.");
        }

        public Task<IResponse<Category>> Update(Category author)
        {
            throw new NotImplementedException();
        }
    }
}
