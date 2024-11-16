using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CuponesApi.Data;
using CuponesApi.Models;
using Serilog;

namespace CuponesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public CuponesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupones  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuponModel>>> GetCupones()
        {
            Log.Information("Se llamó al endpoint para obtener todos los cupones.");

            try
            {
                var cupones = await _context
                    .Cupones
                    .Where(c => c.Activo) // Agregamos el filtro de activos  
                    .Include(c => c.Cupones_Categorias)
                    .ThenInclude(cc => cc.Categoria)
                    .Include(c => c.Tipo_Cupon)
                    .ToListAsync();

                return Ok(cupones);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener la lista de cupones.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los cupones.");
            }
        }

        // GET: api/Cupones/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<CuponModel>> GetCuponModel(int id)
        {
            Log.Information($"Se llamó al endpoint para obtener un cupón por ID: {id}.");

            try
            {
                var cuponModel = await _context.Cupones
                    .Where(c => c.Activo)
                    .FirstOrDefaultAsync(c => c.Id_Cupon == id);

                if (cuponModel == null)
                {
                    Log.Warning($"Cupón con ID: {id} no encontrado.");
                    return NotFound();
                }

                return Ok(cuponModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al obtener el cupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el cupón.");
            }
        }

        public class CuponClienteDTO
        {
            public CuponModel Cupon { get; set; }
            public string NroCupon { get; set; }
            public DateTime? FechaAsignado { get; set; }
        }

        [HttpGet("cliente/{codCliente}")]
        public async Task<ActionResult<IEnumerable<CuponClienteDTO>>> GetCuponesByCliente(string codCliente)
        {
            Log.Information($"Se llamó al endpoint para obtener cupones por CodCliente: {codCliente}.");

            try
            {
                var cuponesCliente = await _context.Cupones_Clientes
                    .Where(cc => cc.CodCliente == codCliente)
                    .Select(cc => new CuponClienteDTO
                    {
                        Cupon = _context.Cupones
                            .Include(c => c.Cupones_Categorias)
                            .ThenInclude(cc => cc.Categoria)
                            .Include(c => c.Tipo_Cupon)
                            .FirstOrDefault(c => c.Id_Cupon == cc.Id_Cupon && c.Activo),
                        NroCupon = cc.NroCupon,
                        FechaAsignado = cc.FechaAsignado
                    })
                    .Where(dto => dto.Cupon != null) // Solo incluir si el cupón existe  
                    .ToListAsync();

                if (!cuponesCliente.Any())
                {
                    Log.Warning($"No se encontraron cupones para el cliente {codCliente}.");
                    return NotFound($"No se encontraron cupones para el cliente {codCliente}");
                }

                return Ok(cuponesCliente);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al obtener cupones para el cliente {codCliente}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los cupones del cliente.");
            }
        }

        // PUT: api/Cupones/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuponModel(int id, CuponModel cuponModel)
        {
            if (id != cuponModel.Id_Cupon)
            {
                Log.Warning($"El ID proporcionado {id} no coincide con el ID del cupón {cuponModel.Id_Cupon}.");
                return BadRequest();
            }

            _context.Entry(cuponModel).State = EntityState.Modified;

            try
            {
                Log.Information($"Se modificó un cupón con ID: {id}.");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CuponModelExists(id))
                {
                    Log.Warning($"Cupón con ID: {id} no encontrado durante la actualización.");
                    return NotFound();
                }
                Log.Error(ex, "Error de concurrencia al actualizar el cupón.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el cupón.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error inesperado al modificar el cupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inesperado al modificar el cupón.");
            }

            return NoContent();
        }

        // POST: api/Cupones  
        [HttpPost]
        public async Task<ActionResult<CuponModel>> PostCuponModel(CuponModel cuponModel)
        {
            try
            {
                _context.Cupones.Add(cuponModel);
                Log.Information("Se creó un cupón.");
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCuponModel", new { id = cuponModel.Id_Cupon }, cuponModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear un nuevo cupón.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el cupón.");
            }
        }

        // DELETE: api/Cupones/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuponModel(int id)
        {
            try
            {
                var cuponModel = await _context.Cupones.FindAsync(id);
                if (cuponModel == null)
                {
                    Log.Warning($"El cupón a eliminar con ID: {id} no se encontró.");
                    return NotFound();
                }

                Log.Information($"Se eliminó un cupón (ya no es válido) con ID: {id}.");
                cuponModel.Activo = false; // Cambiar a no activo en lugar de eliminar  
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al eliminar el cupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el cupón.");
            }
        }

        private bool CuponModelExists(int id)
        {
            return _context.Cupones.Any(e => e.Id_Cupon == id);
        }
    }
}