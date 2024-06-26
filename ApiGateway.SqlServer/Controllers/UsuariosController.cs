using ApiGateway.SqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<Usuario>>> ObtenerNombre(string nombre)
        {
            var usuarios = await _context.Usuarios.Where(u => u.Name.Contains(nombre)).ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> ObtenerTodos()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("guid/{id}")]
        public async Task<ActionResult<Usuario>> ObtenerPorId(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody]Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarPorId(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(g => g.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Usuario>> Modificar([FromBody]Usuario usuario)
        {
            Usuario usuarioBD = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == usuario.Id);

            if (usuario == null)
            {
                return NotFound();
            }

            _context.Entry(usuarioBD).CurrentValues.SetValues(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }
        /*
        [HttpGet]
        public async Task<ActionResult<Usuario>> Delete (int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Usuario usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Usuario usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }*/
    }
}
