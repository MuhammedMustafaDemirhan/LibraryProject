using KutuphaneCore.Entities;
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
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IGenericRepository<User> userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
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

                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu.", user.Name + " " + user.Surname);
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla oluşturuldu."));
            }
            catch
            {
                _logger.LogWarning("Kullanıcı oluşturulurken bir hata oluştu.", user.Name + " " + user.Surname);
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

                _logger.LogInformation("Kullanıcı başarıyla silindi.");
                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla silindi.");
            }
            catch
            {
                _logger.LogWarning("Kullanıcı silinirken bir hata oluştu.");
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
            _logger.LogInformation("Kullanıcı bilgileri başarıyla güncellendi.");
            _logger.LogWarning("Kullanıcı bilgileri güncellenirken bir hata oluştu.");
            throw new NotImplementedException();
        }
    }
}
