﻿namespace ApiGateway.Gateway.Dtos
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<VentaDto>? Ventas { get; set; }
    }
}
