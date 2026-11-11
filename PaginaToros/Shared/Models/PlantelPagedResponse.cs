using System.Collections.Generic;

namespace PaginaToros.Shared.Models
{
    public class PlantelPagedResponse
    {
        public List<PlantelDTO> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
