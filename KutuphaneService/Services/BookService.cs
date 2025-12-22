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
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<BookCreateDto>> Create(BookCreateDto createBookModel)
        {
            try
            {
                if (createBookModel == null)
                {
                    return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Kitap bilgileri boş olamaz."));
                }

                var book = _mapper.Map<Book>(createBookModel);
                book.CreatedDate = DateTime.Now;

                _bookRepository.Create(book);

                _logger.LogInformation("Kitap başarıyla oluşturuldu.", book.Title);

                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Success(null, "Kitap başarıyla oluşturuldu."));
            }
            catch
            {
                _logger.LogError("Kitap oluşturulurken hata oluştu.", createBookModel.Title);
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<BookQueryDto> Delete(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                if (book == null)
                {
                    return ResponseGeneric<BookQueryDto>.Error("Kitap bulunamadı.");
                }

                _bookRepository.Delete(book);
                _logger.LogInformation("Kitap başarıyla silindi.", book.Title);
                return ResponseGeneric<BookQueryDto>.Success(null, "Kitap başarıyla silindi.");
            }
            catch
            {
                _logger.LogError("Kitap silinirken hata oluştu.");
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<BookQueryDto> GetById(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                var bookDto = _mapper.Map<BookQueryDto>(book);

                if (book == null)
                {
                    return ResponseGeneric<BookQueryDto>.Success(null, "Kitap bulunamadı.");
                }

                return ResponseGeneric<BookQueryDto>.Success(bookDto, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetByName(string title)
        {
            try
            {
                var bookList = _bookRepository.GetAll().Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(bookList);

                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
            
        }

        public IResponse<IEnumerable<BookQueryDto>> ListAll()
        {
            try
            {
                var allBooks = _bookRepository.GetAll().ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(allBooks);

                if (bookDtos.ToList().Count == 0 || bookDtos == null)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<Book>> Update(Book book)
        {
            _logger.LogInformation("Kitap bilgileri başarıyla güncellendi.", book.Title);
            _logger.LogWarning("Kitap bilgileri güncellenirken bir hata oluştu.", book.Title);
            throw new NotImplementedException();
        }
    }
}
