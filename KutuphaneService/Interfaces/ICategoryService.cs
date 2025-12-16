using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<Category>> ListAll();
        IResponse<Category> GetById(int id);
        Task<IResponse<Category>> Create(Category author);
        Task<IResponse<Category>> Update(Category author);
        IResponse<Category> Delete(int id);
        Task<IResponse<Category>> GetByName(string name);
    }
}
