using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
using Microsoft.Extensions.Logging;

public class BookService : IBookService
{
    private readonly IGenericRepository<Book> _bookRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IGenericRepository<Book> bookRepository,
        IMapper mapper,
        ILogger<BookService> logger)
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
                return Task.FromResult<IResponse<BookCreateDto>>(
                    ResponseGeneric<BookCreateDto>.Error("Kitap bilgileri boş olamaz.")
                );
            }

            var book = _mapper.Map<Book>(createBookModel);
            book.CreatedDate = DateTime.Now;

            _bookRepository.Create(book);

            _logger.LogInformation(
                "Kitap oluşturuldu. Title: {Title}",
                book.Title
            );

            return Task.FromResult<IResponse<BookCreateDto>>(
                ResponseGeneric<BookCreateDto>.Success(null, "Kitap başarıyla oluşturuldu.")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Kitap oluşturulurken hata oluştu. Title: {Title}",
                createBookModel?.Title
            );

            return Task.FromResult<IResponse<BookCreateDto>>(
                ResponseGeneric<BookCreateDto>.Error("Bir hata oluştu.")
            );
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

            _logger.LogInformation(
                "Kitap silindi. Title: {Title}",
                book.Title
            );

            return ResponseGeneric<BookQueryDto>.Success(null, "Kitap başarıyla silindi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitap silinirken hata oluştu.");
            return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
        }
    }

    public IResponse<BookQueryDto> GetById(int id)
    {
        try
        {
            var book = _bookRepository.GetByIdAsync(id).Result;

            if (book == null)
            {
                return ResponseGeneric<BookQueryDto>.Error("Kitap bulunamadı.");
            }

            var bookDto = _mapper.Map<BookQueryDto>(book);
            return ResponseGeneric<BookQueryDto>.Success(bookDto, "Kitap başarıyla bulundu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitap getirilirken hata oluştu. Id: {Id}", id);
            return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
        }
    }

    public IResponse<IEnumerable<BookQueryDto>> GetByName(string title)
    {
        try
        {
            var bookList = _bookRepository
                .GetAll()
                .Where(x => x.Title.ToLower().Contains(title.ToLower()))
                .ToList();

            var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(bookList);

            if (!bookDtos.Any())
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitap başarıyla bulundu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "İsme göre kitap aranırken hata oluştu. Title: {Title}",
                title
            );

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
        }
    }

    public IResponse<IEnumerable<BookQueryDto>> ListAll()
    {
        try
        {
            var allBooks = _bookRepository.GetAll().ToList();
            var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(allBooks);

            if (!bookDtos.Any())
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar listelendi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitaplar listelenirken hata oluştu.");
            return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
        }
    }

    public IResponse<IEnumerable<BookQueryDto>> GetBooksByCategoryId(int categoryId)
    {
        try
        {
            var booksInCategory = _bookRepository
                .GetAll()
                .Where(x => x.CategoryId == categoryId)
                .ToList();

            var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksInCategory);

            if (!bookDtos.Any())
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(
                bookDtos,
                "Kitaplar başarıyla listelendi."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Kategoriye göre kitaplar getirilirken hata oluştu. CategoryId: {CategoryId}",
                categoryId
            );

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
        }
    }

    public IResponse<IEnumerable<BookQueryDto>> GetBooksByAuthorId(int authorId)
    {
        try
        {
            var booksByAuthor = _bookRepository
                .GetAll()
                .Where(x => x.AuthorId == authorId)
                .ToList();

            var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksByAuthor);

            if (!bookDtos.Any())
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
            }

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(
                bookDtos,
                "Kitaplar başarıyla listelendi."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Yazara göre kitaplar getirilirken hata oluştu. AuthorId: {AuthorId}",
                authorId
            );

            return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
        }
    }

    public Task<IResponse<BookUpdateDto>> Update(BookUpdateDto bookUpdateDto)
    {
        try
        {
            var bookEntity = _bookRepository.GetByIdAsync(bookUpdateDto.Id).Result;

            if (bookEntity == null)
            {
                return Task.FromResult<IResponse<BookUpdateDto>>(
                    ResponseGeneric<BookUpdateDto>.Error("Kitap bulunamadı.")
                );
            }

            if (!string.IsNullOrEmpty(bookUpdateDto.Title))
                bookEntity.Title = bookUpdateDto.Title;
            if (!string.IsNullOrEmpty(bookUpdateDto.Description))
                bookEntity.Description = bookUpdateDto.Description;
            if (bookUpdateDto.PageCount.HasValue)
                bookEntity.PageCount = bookUpdateDto.PageCount.Value;
            if (bookUpdateDto.AuthorId.HasValue)
                bookEntity.AuthorId = bookUpdateDto.AuthorId.Value;
            if (bookUpdateDto.CategoryId.HasValue)
                bookEntity.CategoryId = bookUpdateDto.CategoryId.Value;

            _bookRepository.Update(bookEntity);

            _logger.LogInformation(
                "Kitap güncellendi. Title: {Title}",
                bookEntity.Title
            );

            return Task.FromResult<IResponse<BookUpdateDto>>(
                ResponseGeneric<BookUpdateDto>.Success(null, "Kitap bilgileri başarıyla güncellendi.")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Kitap güncellenirken hata oluştu. Title: {Title}",
                bookUpdateDto?.Title
            );

            return Task.FromResult<IResponse<BookUpdateDto>>(
                ResponseGeneric<BookUpdateDto>.Error("Bir hata oluştu.")
            );
        }
    }
}
