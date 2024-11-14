using ClientesApi.Data;
using ClientesApi.Interfaces;
using ClientesApi.Models;
using ClientesApi.Models.DTO;
using ClientesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace ClientesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly ICuponesService _cuponesService;

        public CuponesController(DataBaseContext context, ICuponesService cuponesService)
        {
            _context = context;
            _cuponesService = cuponesService;
        }


        [HttpPost]

        public async Task<IActionResult> EnviarSolicitudCupones([FromBody] ClienteDto clienteDTO)

        {
            try
            {
                var respuesta = await _cuponesService.SolicitarCupon(clienteDTO);
                Log.Information($"¡Se solicitó un nuevo cupón!");
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                Log.Error($"Se intentó solicitar un cupón, pero ocurrió un error: {ex.Message}");
                return BadRequest($"{ex.Message}");

            }
        }

        [HttpPost("UsarCupon")]
        public async Task<IActionResult> UsarCupon([FromBody] CuponDto cuponDto)
        {
            try
            {
                var message = await _cuponesService.QuemarCupon(cuponDto);
                Log.Information($"Se usó un cupón.");
                return Ok(message);
            }
            catch (Exception ex)
            {
                Log.Error($"Se intentó usar un cupón, pero ocurrió un error: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("cliente/{codigoCliente}")]
        public async Task<IActionResult> ObtenerCuponesActivos(string codigoCliente)
        {
            try
            {
                var cupones = await _cuponesService.ObtenerCuponesActivos(codigoCliente);
                Log.Error($"Se llamó al endpoint para obtener todos los cupones activos por CodCliente.");
                return Ok(cupones);
            }
            catch (Exception ex)
            {
                Log.Error($"Se intentó obtener los cupones activos de un cliente, pero ocurrió un error: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
        }
    }
}