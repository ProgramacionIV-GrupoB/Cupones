using GrupoB.Data;
using GrupoB.Interfaces;
using GrupoB.Models;
using GrupoB.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GrupoB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudCuponesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly ICuponesService _cuponesServices;
        private readonly ISendEmailService _sendEmailService;
        public SolicitudCuponesController(DataBaseContext context, ICuponesService cuponesServices, ISendEmailService sendEmailService)
        { 
           _context = context;
           _cuponesServices = cuponesServices;
           _sendEmailService = sendEmailService;
        }

        [HttpPost("SolicitarCupon")]
        public async Task<IActionResult> SolicitarCupon(ClienteDto clienteDto)
        {
            try
            {
                if (clienteDto.CodCliente.IsNullOrEmpty())
                    throw new Exception("El DNI del cliente no puede estar vacío");

                string nroCupon = await _cuponesServices.GenerarNroCupon();


                Cupon_ClienteModel cupon_Cliente = new Cupon_ClienteModel()
                {
                    Id_Cupon = clienteDto.Id_Cupon,
                    CodCliente = clienteDto.CodCliente,
                    FechaAsignado = DateTime.Now,
                    NroCupon = nroCupon
                };

                _context.Cupones_Clientes.Add(cupon_Cliente);
                await _context.SaveChangesAsync();

                await _sendEmailService.EnviarEmailCliente(clienteDto.Email, nroCupon);

                return Ok(new
                {
                    Mensaje = "Se dio de alta el registro",
                    NroCupon = nroCupon,
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        
        }

        [HttpPost("QuemadoCupon")]
        public async Task<IActionResult> QuemadoCupon(string nroCupon)
        {
            try
            {
                if (string.IsNullOrEmpty(nroCupon))
                    throw new Exception("El número de cupón no puede estar vacío");

                
                var cuponHistorial = new Cupones_HistorialModel
                {
                    NroCupon = nroCupon,
                    FechaUso = DateTime.Now
                };
                _context.Cupones_Historial.Add(cuponHistorial);

                
                var cuponCliente = await _context.Cupones_Clientes.FirstOrDefaultAsync(c => c.NroCupon == nroCupon);
                if (cuponCliente != null)
                {
                    _context.Cupones_Clientes.Remove(cuponCliente);
                }

                await _context.SaveChangesAsync();

                
                return Ok(new
                {
                    Mensaje = "El cupón fue utilizado correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
}
}
