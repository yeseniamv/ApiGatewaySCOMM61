using ApiGateway.Gateway.Dtos;
using ApiGateway.Gateway.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<VentaDto>>> ObtenerTodos()
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var clienteMondoDB = _httpClientFactory.CreateClient(ApiClients.MongoDB.ToString());
            var response = await clienteSqlServer.GetAsync($"api/Ventas/");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<VentaDto>>(content, options);

            foreach (var ventaDto in result)
            {
                var juegoResponse = await clienteMondoDB.GetAsync($"api/Juegos/{ventaDto.JuegoId}");

                if (!juegoResponse.IsSuccessStatusCode)
                {
                    return NotFound("No se encontró el juego.");
                }

                var juegoContent = await juegoResponse.Content.ReadAsStringAsync();
                var juegoDto = JsonSerializer.Deserialize<JuegoDto>(juegoContent, options);
                ventaDto.Juego = juegoDto;
            }

            return Ok(result);
        }

        [HttpGet("guid/{id}")]
        public async Task<ActionResult<VentaDto>> ObtenerPorId(Guid id)
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var clienteMondoDB = _httpClientFactory.CreateClient(ApiClients.MongoDB.ToString());

            var ventaResponse = await clienteSqlServer.GetAsync($"api/Ventas/guid/{id}");

            if (!ventaResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró la venta.");
            }

            var ventaContent = await ventaResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var ventaDto = JsonSerializer.Deserialize<VentaDto>(ventaContent, options);

            var juegoResponse = await clienteMondoDB.GetAsync($"api/Juegos/{ventaDto.JuegoId}");

            var usuarioResponse = await clienteSqlServer.GetAsync($"api/Usuarios/guid/{ventaDto.UsuarioId}");
            

            if (!juegoResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró juego");
            }
            
            if (!usuarioResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró usuario");
            }

            var juegoContent = await juegoResponse.Content.ReadAsStringAsync();
            var juegoDto = JsonSerializer.Deserialize<JuegoDto>(juegoContent, options);
            
            var usuarioContent = await usuarioResponse.Content.ReadAsStringAsync();
            var usuarioDto = JsonSerializer.Deserialize<UsuarioDto>(usuarioContent, options);

            ventaDto.Juego = juegoDto;
            ventaDto.Usuario = usuarioDto;

            return Ok(ventaDto);
        }



        [HttpPost]
        public async Task<ActionResult> Crear([FromBody]VentaDto venta)
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var clienteMondoDB = _httpClientFactory.CreateClient(ApiClients.MongoDB.ToString());

            var juegoResponse = await clienteMondoDB.GetAsync($"api/Juegos/{venta.JuegoId}");
            var usuarioResponse = await clienteSqlServer.GetAsync($"api/Usuarios/{venta.UsuarioId}");

            if (!juegoResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró juego");
            } else if (!usuarioResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró usuario");
            }

            var ventaContent = new StringContent(
                JsonSerializer.Serialize(venta), Encoding.UTF8, "application/json");
            var response = await clienteSqlServer.PostAsync("api/Ventas", ventaContent);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
