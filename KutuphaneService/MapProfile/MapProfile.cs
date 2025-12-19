using AutoMapper;
using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.MapProfile
{
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
        }
    }
}
