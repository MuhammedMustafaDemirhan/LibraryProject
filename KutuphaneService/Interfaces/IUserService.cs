using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Interfaces
{
    public interface IUserService
    {
        IResponse<UserCreateDto> Create(UserCreateDto userCreateDto);
        IResponse<bool> LoginUser(UserLoginbDto userLoginDto);
    }
}
