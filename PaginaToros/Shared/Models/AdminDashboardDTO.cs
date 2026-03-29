using System;
using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class AdminDashboardDTO
    {
        public int TotalSolicitudes { get; set; }
        public int SolicitudesPendientes { get; set; }
        public int SolicitudesAutorizadas { get; set; }
        public int SolicitudesSinSocio { get; set; }
        public int InspeccionesRealizadas { get; set; }
        public int SociosRegistrados { get; set; }
        public int EstablecimientosRegistrados { get; set; }
        public double TasaAutorizacion { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<DashboardMonthlyPointDTO> ActividadMensual { get; set; } = new();
        public List<DashboardRecentSolicitudDTO> SolicitudesRecientes { get; set; } = new();
    }

    public class DashboardMonthlyPointDTO
    {
        public string Label { get; set; } = string.Empty;
        public int Solicitudes { get; set; }
        public int Inspecciones { get; set; }
    }

    public class DashboardRecentSolicitudDTO
    {
        public int Id { get; set; }
        public string NroSolicitud { get; set; } = string.Empty;
        public string SocioNombre { get; set; } = string.Empty;
        public string EstablecimientoNombre { get; set; } = string.Empty;
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public bool EstaAutorizada { get; set; }
    }
}
