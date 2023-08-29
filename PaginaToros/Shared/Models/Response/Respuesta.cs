namespace PaginaToros.Shared.Models.Response
{
    public class Respuesta<T>
    {
        public int Exito { get; set; }
        public string Mensaje { get; set; }
        public T List { get; set; }

        public Respuesta()
        {
            this.Exito = 0;
        }

    }
}
