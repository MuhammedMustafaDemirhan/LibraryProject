using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Interfaces
{
    public interface IUserService
    {
        IResponse<IEnumerable<User>> ListAll();
        IResponse<User> GetById(int id);
        Task<IResponse<User>> Create(User author);
        Task<IResponse<User>> Update(User author);
        IResponse<User> Delete(int id);
        Task<IResponse<User>> GetByName(string name);
    }
}
