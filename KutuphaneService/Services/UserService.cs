using KutuphaneCore.Entities;
using KutuphaneDataAccess.Repository;
using KutuphaneService.Interfaces;
using KutuphaneService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IResponse<User>> Create(User user)
        {
            try
            {
                if (user == null)
                {
                    return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Kullanıcı bilgileri boş olamaz."));
                }

                _userRepository.Create(user);

                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<User> Delete(int id)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(id).Result;

                if (user == null)
                {
                    return ResponseGeneric<User>.Error("Kullanıcı bulunamadı.");
                }

                _userRepository.Delete(user);
                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<User>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<User> GetById(int id)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(id).Result;

                if (user == null)
                {
                    return ResponseGeneric<User>.Success(null, "Kullanıcı bulunamadı.");
                }

                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<User>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<User>> GetByName(string name)
        {
            try
            {
                var userList = _userRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

                if (userList == null || userList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<User>>.Error("Kullanıcı bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<User>>.Success(userList, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<User>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<User>> ListAll()
        {
            try
            {
                var allUsers = _userRepository.GetAll().ToList();

                if (allUsers.Count == 0 || allUsers == null)
                {
                    return ResponseGeneric<IEnumerable<User>>.Error("Kullanıcı bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<User>>.Success(allUsers, "Kullanıcılar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<User>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<User>> Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
