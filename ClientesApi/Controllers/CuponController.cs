using ClientesApi.Data;
using ClientesApi.Interfaces;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;


namespace ClientesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        
        // Inyectar HttpClient a través del constructor
        public CuponController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        
        }

        // Endpoint para reclamar el cupón
        [HttpPost("ReclamarCupon")]
        public async Task<IActionResult> ReclamarCupon([FromBody] ReclamarCuponModel request)
        {
            // Aquí construimos la URL del servicio CuponesApi
            var url = "https://localhost:7269/api/SolicitudCupones/SolicitarCupon"; // Cambia esta URL al endpoint correcto de CuponesApi

            // Realizamos la solicitud HTTP al servicio CuponesApi
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error al reclamar el cupón");
            }
        }

        // Endpoint para usar el cupón
        [HttpPost("UsarCupon")]
        public async Task<IActionResult> UsarCupon([FromBody] UsarCuponModel request)
        {
            // Aquí construimos la URL del servicio CuponesApi
            var url = "https://localhost:7269/api/SolicitudCupones/QuemadoCupon"; // Cambia esta URL al endpoint correcto de CuponesApi

            // Realizamos la solicitud HTTP al servicio CuponesApi
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                //await _sendEmailService.EnviarEmailCliente(httpClient.Email, request);

                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error al usar el cupón");
            }
        }
    }

}
