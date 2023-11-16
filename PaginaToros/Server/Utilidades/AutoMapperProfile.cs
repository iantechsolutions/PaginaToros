using AutoMapper;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        { 
            #region TorosUni
            CreateMap<Torosuni, TorosuniDTO>();
            CreateMap<TorosuniDTO, Torosuni>();
            #endregion TorosUni
        }
    }
}
