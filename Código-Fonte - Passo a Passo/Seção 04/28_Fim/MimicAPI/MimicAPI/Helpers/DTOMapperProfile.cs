using AutoMapper;
using MimicAPI.V1.Models;
using MimicAPI.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Palavra, PalavraDTO>();
            CreateMap<PaginationList<Palavra>, PaginationList<PalavraDTO>>();
        }
    }
}
