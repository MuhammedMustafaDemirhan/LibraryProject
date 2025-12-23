using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IGenericRepository<Category> categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category)
        {
            try
            {
                if (category == null)
                {
                    return Task.FromResult<IResponse<CategoryCreateDto>>(
                        ResponseGeneric<CategoryCreateDto>.Error("Kategori bilgileri boş olamaz.")
                    );
                }

                var entity = _mapper.Map<Category>(category);
                entity.CreatedDate = DateTime.Now;

                _categoryRepository.Create(entity);

                _logger.LogInformation(
                    "Kategori oluşturuldu. Name: {Name}",
                    entity.Name
                );

                return Task.FromResult<IResponse<CategoryCreateDto>>(
                    ResponseGeneric<CategoryCreateDto>.Success(null, "Kategori başarıyla oluşturuldu.")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kategori oluşturulurken hata oluştu. Name: {Name}",
                    category?.Name
                );

                return Task.FromResult<IResponse<CategoryCreateDto>>(
                    ResponseGeneric<CategoryCreateDto>.Error("Bir hata oluştu.")
                );
            }
        }

        public IResponse<CategoryQueryDto> Delete(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;

                if (category == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Error("Kategori bulunamadı.");
                }

                _categoryRepository.Delete(category);

                _logger.LogInformation(
                    "Kategori silindi. Name: {Name}",
                    category.Name
                );

                return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kategori silinirken hata oluştu. Id: {Id}",
                    id
                );

                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> GetById(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;

                if (category == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Error("Kategori bulunamadı.");
                }

                var categoryDto = _mapper.Map<CategoryQueryDto>(category);
                return ResponseGeneric<CategoryQueryDto>.Success(categoryDto, "Kategori başarıyla bulundu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kategori getirilirken hata oluştu. Id: {Id}",
                    id
                );

                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name)
        {
            try
            {
                var categories = _categoryRepository
                    .GetAll()
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .ToList();

                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (!categoryDtos.Any())
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(
                    categoryDtos,
                    "Kategoriler listelendi."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "İsme göre kategori aranırken hata oluştu. Name: {Name}",
                    name
                );

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> ListAll()
        {
            try
            {
                var categories = _categoryRepository.GetAll().ToList();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (!categoryDtos.Any())
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(
                    categoryDtos,
                    "Kategoriler listelendi."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriler listelenirken hata oluştu.");
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<CategoryUpdateDto>> Update(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var categoryEntity = _categoryRepository.GetByIdAsync(categoryUpdateDto.Id).Result;

                if (categoryEntity == null)
                    return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Error("Kategori bulunamadı."));

                if (categoryUpdateDto.Name != null)
                    categoryEntity.Name = categoryUpdateDto.Name;
                if (categoryUpdateDto.Description != null)
                    categoryEntity.Description = categoryUpdateDto.Description;

                _categoryRepository.Update(categoryEntity);

                _logger.LogInformation(
                    "Kategori güncellendi. Name: {Name}",
                    categoryEntity.Name
                );

                return Task.FromResult<IResponse<CategoryUpdateDto>>(
                    ResponseGeneric<CategoryUpdateDto>.Success(null, "Kategori başarıyla güncellendi.")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kategori güncellenirken hata oluştu. Name: {Name}",
                    categoryUpdateDto?.Name
                );

                return Task.FromResult<IResponse<CategoryUpdateDto>>(
                    ResponseGeneric<CategoryUpdateDto>.Error("Bir hata oluştu.")
                );
            }
        }
    }
}
