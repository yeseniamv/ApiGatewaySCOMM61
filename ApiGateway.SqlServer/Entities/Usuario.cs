﻿namespace ApiGateway.SqlServer.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Venta> Ventas {  get; set; } 
    }
}
