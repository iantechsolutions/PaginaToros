﻿using PaginaToros.Shared.Models;
using PaginaToros.Shared.Models.Response;

namespace PaginaToros.Client.Servicios.Contrato
{
    public interface IPlantelServicio
    {
        Task<Respuesta<List<PlantelDTO>>> Lista(int page, int count);
        Task<Respuesta<List<PlantelDTO>>> ObtenerPorAnios(int anio1, int anio2);
        Task<Respuesta<int>> CantidadTotal();
        Task<Respuesta<List<PlantelDTO>>> LimitadosFiltrados(int skip, int take, string filtro = null);
        Task<Respuesta<List<PlantelDTO>>> LimitadosFiltradosNoInclude(int skip, int take, string filtro = null);
        Task<Respuesta<List<PlantelDTO>>> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);

        Task<Respuesta<List<PlantelDTO>>> GetBySocioId(int socioId);
        Task<bool> Eliminar(int id);
        Task<Respuesta<PlantelDTO>> Crear(PlantelDTO entidad);
        Task<bool> Editar(PlantelDTO entidad);
        Task<Respuesta<PlantelDTO>> Filtrar(string descripcion);
    }
}
