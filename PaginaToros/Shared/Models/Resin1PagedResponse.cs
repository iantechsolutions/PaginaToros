using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class Resin1PagedResponse
    {
        public List<Resin1DTO> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
