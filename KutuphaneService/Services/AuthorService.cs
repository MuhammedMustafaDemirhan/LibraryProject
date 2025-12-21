using AutoMapper;
using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
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
        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public Task<IResponse<Author>> Create(AuthorCreateDto authorCreateDto)
        {
            try
            {
                if (authorCreateDto == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
                }

                var entity = _mapper.Map<Author>(authorCreateDto);
                entity.CreatedDate = DateTime.Now;

                _authorRepository.Create(entity);

                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(entity, "Yazar başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Bir hata oluştu."));
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
                return ResponseGeneric<Author>.Success(author, "Yazar başarıyla silindi.");
            }
            catch
            {
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
            catch
            {
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name)
        {
            try
            {
                var authorList = _authorRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(authorList);

                if (authorQueryDtos == null || authorQueryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(allAuthors);

                if (authorQueryDtos.ToList().Count == 0 || authorQueryDtos == null)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazarlar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<Author>> Update(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
