using ApiGateway.SqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApiGateway.SqlServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{nombreDeUsuario}")]
        public async Task<ActionResult<List<Venta>>> ObtenerPorUsuario(string nombreDeUsuario)
        {
            var ventas = await _context.Ventas.Where(u => u.Usuario.Username.Contains(nombreDeUsuario)).ToListAsync();
            return Ok(ventas);
        }

        [HttpGet]
        public async Task<ActionResult<List<Venta>>> ObtenerTodos()
        {
            var ventas = await _context.Ventas.Include(u=> u.Usuario).ToListAsync();
            return Ok(ventas);
        }

        [HttpGet("guid/{id}")]
        public async Task<ActionResult<Venta>> ObtenerPorId(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas.FirstOrDefaultAsync(u => u.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            return Ok(venta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Venta venta)
        {
            await _context.Ventas.AddAsync(venta);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*
        [HttpPut]
        public async Task<ActionResult<Usuario>> Edit(Usuario model)
        {
            Usuario usuario = await _context.Usuarios.Where(x => x.Id == model.Id).SingleOrDefaultAsync(); //Mejor que FindAsync?

            //Using ModelState.IsValid para verificar la vaildez del modelo utilizado
            if (usuario != null)
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.Entry(usuario).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return usuario;
        } 

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
