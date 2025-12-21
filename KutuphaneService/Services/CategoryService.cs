using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
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
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryResository, IMapper mapper)
        {
            _categoryResository = categoryResository;
            _mapper = mapper;
        }


        public Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category)
        {
            try
            {
                if (category == null)
                {
                    return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Error("Kategori bilgileri boş olamaz."));
                }

                var entity = _mapper.Map<Category>(category);
                entity.CreatedDate = DateTime.Now;

                _categoryResository.Create(entity);

                return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Success(null, "Kategori başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<CategoryQueryDto> Delete(int id)
        {
            try
            {
                var category = _categoryResository.GetByIdAsync(id).Result;

                if (category == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Error("Kategori bulunamadı.");
                }

                _categoryResository.Delete(category);

                return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> GetById(int id)
        {
            try
            {
                var category = _categoryResository.GetByIdAsync(id).Result;

                var categoryDto = _mapper.Map<CategoryQueryDto>(category);

                if (categoryDto == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori bulunamadı.");
                }
                return ResponseGeneric<CategoryQueryDto>.Success(categoryDto, "Kategori başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name)
        {
            try
            {
                var categories = _categoryResository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (categoryDtos == null || categoryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos, "Kategoriler listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> ListAll()
        {
            try
            {
                var categories = _categoryResository.GetAll().ToList();

                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (categoryDtos == null || categoryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos, "Kategoriler listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<Category>> Update(Category author)
        {
            throw new NotImplementedException();
        }
    }
}
