using ClientesApi.Data;
using ClientesApi.Interfaces;
using ClientesApi.Models;
using ClientesApi.Models.DTO;
using ClientesApi.Services;
using Microsoft.AspNetCore.Mvc;


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
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");

            }
        }

        [HttpPost("UsarCupon")]
        public async Task<IActionResult> UsarCupon([FromBody] CuponDto cuponDto)
        {
            try
            {
                var message = await _cuponesService.QuemarCupon(cuponDto);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}