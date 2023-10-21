using System;
using System.Collections.Generic;

namespace PaginaToros.Server.ModelsTempp;

public partial class TransaccionesHistorica
{
    public int Id { get; set; }

    public int? IdTransaccion { get; set; }

    public int? IdToro { get; set; }
}
