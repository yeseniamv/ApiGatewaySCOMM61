﻿using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.MongoDb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JuegosController : ControllerBase
    {
        private readonly Juego _juego;

        public JuegosController()
        {
            _juego = new Juego();
        }

        [HttpGet]
        public async Task<ActionResult<List<Juego>>> ObtenerJuegos()
        {
            return await _juego.ObtenerJuegos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Juego>> ObtenerPorId(string id)
        {
            return await _juego.ObtenerPorId(id);
        }

        [HttpPost]
        public async Task<ActionResult<Juego>> Agregar([FromBody]Juego juego)
        {
            await _juego.Agregar(juego);
            return Ok(juego);
        }
    }
}
