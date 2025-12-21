using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<BookQueryDto>> ListAll();
        IResponse<BookQueryDto> GetById(int id);
        Task<IResponse<BookCreateDto>> Create(BookCreateDto book);
        Task<IResponse<Book>> Update(Book book);
        IResponse<BookQueryDto> Delete(int id);
        IResponse<IEnumerable<BookQueryDto>> GetByName(string title);
    }
}
