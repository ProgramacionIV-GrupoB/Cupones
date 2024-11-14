using CuponesApi.Data;
using CuponesApi.Interfaces;
using CuponesApi.Models;
using CuponesApi.Models.DTO;
using CuponesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace CuponesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudCuponesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly ICuponesService _cuponesService;
        private readonly ISendEmailService _sendEmailService;

        public SolicitudCuponesController(DataBaseContext context, ICuponesService cuponesService, ISendEmailService sendEmailService)
        {

            _context = context;
            _cuponesService = cuponesService;
            _sendEmailService = sendEmailService;
        }

        [HttpPost("SolicitarCupon")]

        public async Task<IActionResult> SolicitarCupon(ClienteDto clienteDto)
        {
            try
            {
                if (clienteDto.CodCliente.IsNullOrEmpty())
                    throw new Exception("El Cod-Cliente del cliente no puede estar vacío");

                var cliente = await _context.Cupones_Clientes.FirstOrDefaultAsync(c => c.CodCliente == clienteDto.CodCliente);

                // Verificar si el cliente ya tiene asignado el cupón solicitado
                var cuponCliente = await _context.Cupones_Clientes.FirstOrDefaultAsync(cc => cc.CodCliente == clienteDto.CodCliente && cc.Id_Cupon == clienteDto.Id_Cupon);
                if (cuponCliente != null)
                    return BadRequest($"El cliente con Cod-Cliente {clienteDto.CodCliente} ya tiene asignado el cupón con ID {clienteDto.Id_Cupon}.");


                string nroCupon = await _cuponesService.GenerarNroCupon();

                Cupon_ClienteModel cupon_Cliente = new Cupon_ClienteModel()
                {
                    Id_Cupon = clienteDto.Id_Cupon,
                    CodCliente = clienteDto.CodCliente,
                    FechaAsignado = DateTime.Now,
                    NroCupon = nroCupon

                };

                _context.Cupones_Clientes.Add(cupon_Cliente);
                await _context.SaveChangesAsync();

                await _sendEmailService.EnviarEmailReclamo(clienteDto.Email, nroCupon);

                return Ok($"Se dió de alta el cupón. ¡Revisá tu correo electrónico!");


            }
            catch (Exception ex)

            {
                return BadRequest($"Ocurrió un error: {ex.Message}");
            }
        }

        [HttpPost("QuemadoCupon")]
        public async Task<IActionResult> QuemadoCupon([FromBody] CuponDto cuponDto)
        {
            try
            {
                var cuponCliente = await _context.Cupones_Clientes
                    .FirstOrDefaultAsync(cc => cc.NroCupon == cuponDto.NroCupon && cc.CodCliente == cuponDto.CodCliente);

                if (cuponCliente == null)
                {
                    return BadRequest($"El cupón {cuponDto.NroCupon} no pertenece al cliente con Cod-Cliente {cuponDto.CodCliente}.");
                }

                var cuponHistorial = new Cupones_HistorialModel
                {
                    Id_Cupon = cuponCliente.Id_Cupon,
                    CodCliente = cuponCliente.CodCliente,
                    FechaUso = DateTime.Now,
                    NroCupon = cuponCliente.NroCupon
                };

                _context.Cupones_Historial.Add(cuponHistorial);

                _context.Cupones_Clientes.Remove(cuponCliente);
                await _context.SaveChangesAsync();

                await _sendEmailService.EnviarEmailUso(cuponDto.Email, cuponDto.NroCupon);

                return Ok($"El cupón {cuponDto.NroCupon} fue utilizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrió un error: {ex.Message}");
            }
        }


    }
}