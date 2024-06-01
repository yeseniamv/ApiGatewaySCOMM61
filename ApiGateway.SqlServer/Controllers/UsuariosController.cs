using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.SqlServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<string>> ObtenerNombre()
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync();
            usuario.Id = Guid.NewGuid();
            return usuario.Name;
        }
    }
}
