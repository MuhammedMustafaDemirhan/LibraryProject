using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<Book>> ListAll();
        IResponse<Book> GetById(int id);
        Task<IResponse<Book>> Create(Book author);
        Task<IResponse<Book>> Update(Book author);
        IResponse<Book> Delete(int id);
        Task<IResponse<Book>> GetByName(string name);
    }
}
