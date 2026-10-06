using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    /// <summary>
    /// Destino de una transferencia (establecimientos y planteles de la razon social compradora),
    /// devuelto por <c>api/TransferenciasLookup/DestinoTransferencia</c>.
    ///
    /// Cuando quien consulta es un usuario socio y el comprador es OTRA razon social, el server
    /// devuelve solo lo necesario para elegir el destino: codigo y año del plantel, su
    /// establecimiento (codigo y nombre) y nada mas. Las existencias del plantel y los datos de
    /// contacto del establecimiento del comprador no viajan; <see cref="IncluyeExistencias"/>
    /// avisa al cliente para que no muestre stock en cero como si fuera real.
    /// </summary>
    public class TransferenciaDestinoDTO
    {
        public string CompradorSocioCode { get; set; } = string.Empty;

        /// <summary>
        /// true si los planteles traen existencias (razon social propia o usuario administrativo);
        /// false cuando son datos minimos de otra razon social.
        /// </summary>
        public bool IncluyeExistencias { get; set; }

        public List<EstableDTO> Establecimientos { get; set; } = new();

        public List<PlantelDTO> Planteles { get; set; } = new();
    }
}
