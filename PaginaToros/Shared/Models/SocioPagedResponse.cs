using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class SocioPagedResponse
    {
        public List<SocioDTO> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
