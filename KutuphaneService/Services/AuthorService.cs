using AutoMapper;
using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(
            IGenericRepository<Author> authorRepository,
            IMapper mapper,
            ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<Author>> Create(AuthorCreateDto authorCreateDto)
        {
            try
            {
                if (authorCreateDto == null)
                {
                    return Task.FromResult<IResponse<Author>>(
                        ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz.")
                    );
                }

                var entity = _mapper.Map<Author>(authorCreateDto);
                entity.CreatedDate = DateTime.Now;

                _authorRepository.Create(entity);

                _logger.LogInformation(
                    "Yazar oluşturuldu. Name: {Name}, Surname: {Surname}",
                    authorCreateDto.Name,
                    authorCreateDto.Surname
                );

                return Task.FromResult<IResponse<Author>>(
                    ResponseGeneric<Author>.Success(entity, "Yazar başarıyla oluşturuldu.")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Yazar oluşturulurken hata oluştu. Name: {Name}, Surname: {Surname}",
                    authorCreateDto?.Name,
                    authorCreateDto?.Surname
                );

                return Task.FromResult<IResponse<Author>>(
                    ResponseGeneric<Author>.Error("Bir hata oluştu.")
                );
            }
        }

        public IResponse<Author> Delete(int id)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
                }

                _authorRepository.Delete(author);

                _logger.LogInformation(
                    "Yazar silindi. Name: {Name}, Surname: {Surname}",
                    author.Name,
                    author.Surname
                );

                return ResponseGeneric<Author>.Success(author, "Yazar başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar silinirken hata oluştu.");
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<AuthorQueryDto> GetById(int id)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<AuthorQueryDto>.Error("Yazar bulunamadı.");
                }

                var authorQueryDto = _mapper.Map<AuthorQueryDto>(author);
                return ResponseGeneric<AuthorQueryDto>.Success(authorQueryDto, "Yazar başarıyla bulundu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar getirilirken hata oluştu.");
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name)
        {
            try
            {
                var authorList = _authorRepository
                    .GetAll()
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(authorList);

                if (!authorQueryDtos.Any())
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "İsme göre yazar aranırken hata oluştu. Name: {Name}",
                    name
                );

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();
                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(allAuthors);

                if (!authorQueryDtos.Any())
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazarlar listelendi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazarlar listelenirken hata oluştu.");
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<Author>> Update(Author author)
        {
            try
            {
                _logger.LogInformation(
                    "Yazar güncellendi. Name: {Name}, Surname: {Surname}",
                    author.Name,
                    author.Surname
                );

                return Task.FromResult<IResponse<Author>>(
                    ResponseGeneric<Author>.Success(author, "Yazar başarıyla güncellendi.")
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Yazar güncellenirken hata oluştu. Name: {Name}, Surname: {Surname}",
                    author?.Name,
                    author?.Surname
                );

                return Task.FromResult<IResponse<Author>>(
                    ResponseGeneric<Author>.Error("Bir hata oluştu.")
                );
            }
        }
    }

}
