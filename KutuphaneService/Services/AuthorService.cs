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
        public AuthorService(IGenericRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<IResponse<Author>> Create(Author author)
        {
            if (author == null)
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
            }

            _authorRepository.Create(author);

            return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(author, "Yazar başarıyla oluşturuldu."));
        }

        public IResponse<Author> Delete(int id)
        {
            var author = _authorRepository.GetByIdAsync(id).Result;

            if(author == null)
            {
                return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
            }

            _authorRepository.Delete(author);
            return ResponseGeneric<Author>.Success(author, "Yazar başarıyla silindi.");
        }

        public IResponse<Author> GetById(int id)
        {
            var author = _authorRepository.GetByIdAsync(id).Result;

            if (author == null)
            {
                return ResponseGeneric<Author>.Success(null, "Yazar bulunamadı.");
            }

            return ResponseGeneric<Author>.Success(author, "Yazar başarıyla bulundu.");
        }

        public IResponse<IEnumerable<Author>> GetByName(string name)
        {
            var authorList = _authorRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            if (authorList == null || authorList.Count == 0)
            {
                return ResponseGeneric<IEnumerable<Author>>.Error("Yazar bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<Author>>.Success(authorList, "Yazar başarıyla bulundu.");
        }

        public IResponse<IEnumerable<Author>> ListAll()
        {
            var allAuthors = _authorRepository.GetAll().ToList();

            if (allAuthors.Count == 0 || allAuthors == null)
            {
                return ResponseGeneric<IEnumerable<Author>>.Error("Yazar bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<Author>>.Success(allAuthors, "Yazarlar listelendi.");
        }

        public Task<IResponse<Author>> Update(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
