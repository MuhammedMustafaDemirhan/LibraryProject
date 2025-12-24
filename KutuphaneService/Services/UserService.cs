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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, ILogger<UserService> logger, IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IResponse<UserCreateDto> Create(UserCreateDto userCreateDto)
        {
            try
            {
                if (userCreateDto == null)
                {
                    return ResponseGeneric<UserCreateDto>.Error("Kullanıcı bilgileri boş olamaz.");
                }

                if (string.IsNullOrEmpty(userCreateDto.Username) || string.IsNullOrEmpty(userCreateDto.Email))
                    return ResponseGeneric<UserCreateDto>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");

                if (string.IsNullOrEmpty(userCreateDto.Password))
                {
                    return ResponseGeneric<UserCreateDto>.Error("Şifre boş olamaz.");
                }

                var hashedPassword = HashPassword(userCreateDto.Password);

                var userEntity = _mapper.Map<User>(userCreateDto);
                userEntity.Password = hashedPassword;

                _userRepository.Create(userEntity);

                _logger.LogInformation(
    "Kullanıcı başarıyla oluşturuldu. Name: {Name} {Surname}",
    userEntity.Name,
    userEntity.Surname
);

                return ResponseGeneric<UserCreateDto>.Success(null, "Kullanıcı başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kullanıcı oluşturulurken hata oluştu. Name: {Name} {Surname}",
                    userCreateDto?.Name,
                    userCreateDto?.Surname
                );

                return ResponseGeneric<UserCreateDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<bool> LoginUser(UserLoginbDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                return ResponseGeneric<bool>.Error("Giriş bilgileri boş olamaz.");
            }

            if (string.IsNullOrWhiteSpace(userLoginDto.Password))
            {
                return ResponseGeneric<bool>.Error("Şifre boş olamaz.");
            }

            if (string.IsNullOrWhiteSpace(userLoginDto.Username)
             && string.IsNullOrWhiteSpace(userLoginDto.Email))
            {
                return ResponseGeneric<bool>.Error("Kullanıcı adı veya e-posta girilmelidir.");
            }

            var checkUser = _userRepository.GetAll().FirstOrDefault(x => (x.Username == userLoginDto.Username || x.Email == userLoginDto.Email) && x.Password == HashPassword(userLoginDto.Password));


            if (checkUser == null)
                return ResponseGeneric<bool>.Error("Kullanıcı adı veya şifre hatalı.");

            return ResponseGeneric<bool>.Success(true, "Giriş başarılı.");
        }

        private string HashPassword(string password)
        {
            string secretKey = "'ug?('Zp^E!F)-kH<g1ohQG+yr}Db]";

            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + secretKey;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var hashedPassword = Convert.ToBase64String(bytes);
                return hashedPassword;
            }
        }
    }
}
