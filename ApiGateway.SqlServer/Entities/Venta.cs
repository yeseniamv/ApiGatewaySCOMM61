namespace ApiGateway.SqlServer.Entities
{
    public class Venta
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string JuegoId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
