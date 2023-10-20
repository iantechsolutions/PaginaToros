namespace PaginaToros.Shared.Models
{
    public class ResponseForList
    {
        public string EntitiesPricipal { get; set; }
        public string EntitiesSecundary { get; set; }
        public int AllRegisters { get; set; }
        public int ActualPage { get; set; }
    }
}
