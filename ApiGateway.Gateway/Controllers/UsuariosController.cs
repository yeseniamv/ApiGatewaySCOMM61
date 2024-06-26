using ApiGateway.Gateway.Dtos;
using ApiGateway.Gateway.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<UsuarioDto>>> ObtenerNombre(string nombre)
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var response = await clienteSqlServer.GetAsync($"api/Usuarios/{nombre}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<UsuarioDto>>(content, options);

            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult> CrearUsuario([FromBody]UsuarioDto usuario)
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var usuarioContent = new StringContent(
                JsonSerializer.Serialize(usuario), Encoding.UTF8, "application/json");
            var response = await clienteSqlServer.PostAsync("api/Usuarios", usuarioContent);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
