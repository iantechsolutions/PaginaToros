using AutoMapper;
using PaginaToros.Shared.Models;

namespace PaginaToros.Server.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Centrosium
            CreateMap<Centrosium, CentrosiumDTO>();
            CreateMap<CentrosiumDTO, Centrosium>();
            #endregion Centrosium

            #region Certifseman
            CreateMap<Certifseman, CertifsemanDTO>();
            CreateMap<CertifsemanDTO, Certifseman>();
            #endregion Certifseman

            #region Desepla1
            CreateMap<Desepla1, Desepla1DTO>();
            CreateMap<Desepla1DTO, Desepla1>();
            #endregion Desepla1


            #region Desepla3
            CreateMap<Desepla3, Desepla3DTO>();
            CreateMap<Desepla3DTO, Desepla3>();
            #endregion Desepla3

            #region Estable
            CreateMap<Estable, EstableDTO>();
            CreateMap<EstableDTO, Estable>();
            #endregion Estable

            #region Futcontrol
            CreateMap<Futcontrol, FutcontrolDTO>();
            CreateMap<FutcontrolDTO, Futcontrol>();
            #endregion Futcontrol

            #region Inspect
            CreateMap<Inspect, InspectDTO>();
            CreateMap<InspectDTO, Inspect>();
            #endregion Inspect

            #region Plantel
            CreateMap<Plantel, PlantelDTO>();
            CreateMap<PlantelDTO, Plantel>();
            #endregion Plantel

            #region Resin1
            CreateMap<Resin1, Resin1DTO>();
            CreateMap<Resin1DTO, Resin1>();
            #endregion Resin1

            #region Resin2
            CreateMap<Resin2, Resin2DTO>();
            CreateMap<Resin2DTO, Resin2>();
            #endregion Resin2

            #region Resin3
            CreateMap<Resin3, Resin3DTO>();
            CreateMap<Resin3DTO, Resin3>();
            #endregion Resin3

            #region Resin4
            CreateMap<Resin4, Resin4DTO>();
            CreateMap<Resin4DTO, Resin4>();
            #endregion Resin4

            #region Resin6
            CreateMap<Resin6, Resin6DTO>();
            CreateMap<Resin6DTO, Resin6>();
            #endregion Resin6

            #region Resin8
            CreateMap<Resin8, Resin8DTO>();
            CreateMap<Resin8DTO, Resin8>();
            #endregion Resin8

            #region Socio
            CreateMap<Socio, SocioDTO>();
            CreateMap<SocioDTO, Socio>();
            #endregion Socio

            #region Solici1
            CreateMap<Solici1, Solici1DTO>();
            CreateMap<Solici1DTO, Solici1>();
            #endregion Solici1

            #region TorosUni
            CreateMap<Torosuni, TorosuniDTO>();
            CreateMap<TorosuniDTO, Torosuni>();
            #endregion TorosUni

            #region Transan
            CreateMap<Transan, TransanDTO>();
            CreateMap<TransanDTO, Transan>();
            #endregion Transan

            #region Transsb
            CreateMap<Transsb, TranssbDTO>();
            CreateMap<TranssbDTO, Transsb>();
            #endregion Transsb
        }
    }
}
