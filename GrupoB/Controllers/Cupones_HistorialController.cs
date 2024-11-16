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
    public class Cupones_HistorialController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public Cupones_HistorialController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupones_Historial  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupones_HistorialModel>>> GetCupones_Historial()
        {
            try
            {
                Log.Information("Se llamó al endpoint para obtener el historial de cupones (cupones expirados).");
                var historials = await _context.Cupones_Historial.ToListAsync();
                return Ok(historials);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener el historial de cupones.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los datos.");
            }
        }

        // GET: api/Cupones_Historial/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupones_HistorialModel>> GetCupones_HistorialModel(int id)
        {
            try
            {
                Log.Information($"Se llamó al endpoint para obtener un cupón en el historial (expirado) por ID: {id}.");
                var cupones_HistorialModel = await _context.Cupones_Historial.FindAsync(id);

                if (cupones_HistorialModel == null)
                {
                    Log.Warning($"Cupón con ID: {id} no encontrado.");
                    return NotFound();
                }

                return Ok(cupones_HistorialModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener el cupón del historial.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el dato.");
            }
        }

        // PUT: api/Cupones_Historial/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupones_HistorialModel(int id, Cupones_HistorialModel cupones_HistorialModel)
        {
            if (id != cupones_HistorialModel.Id_Cupon)
            {
                Log.Warning("El ID proporcionado no coincide con el ID del cupón.");
                return BadRequest();
            }

            _context.Entry(cupones_HistorialModel).State = EntityState.Modified;
            Log.Information($"Se modificó el historial de un cupón con ID: {id}.");

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Cupones_HistorialModelExists(id))
                {
                    Log.Warning($"Cupón con ID: {id} no encontrado durante la actualización.");
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el cupón.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al modificar el cupón en el historial.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al modificar el cupón.");
            }
        }

        // POST: api/Cupones_Historial  
        [HttpPost]
        public async Task<ActionResult<Cupones_HistorialModel>> PostCupones_HistorialModel(Cupones_HistorialModel cupones_HistorialModel)
        {
            try
            {
                _context.Cupones_Historial.Add(cupones_HistorialModel);
                await _context.SaveChangesAsync();
                Log.Information($"Se utilizó un cupón y se introdujo en el historial con ID: {cupones_HistorialModel.Id_Cupon}.");

                return CreatedAtAction("GetCupones_HistorialModel", new { id = cupones_HistorialModel.Id_Cupon }, cupones_HistorialModel);
            }
            catch (DbUpdateException ex)
            {
                if (Cupones_HistorialModelExists(cupones_HistorialModel.Id_Cupon))
                {
                    Log.Warning($"Conflicto al agregar un cupón; ya existe uno con ID: {cupones_HistorialModel.Id_Cupon}.");
                    return Conflict();
                }
                Log.Error(ex, "Error al agregar el cupón al historial.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al agregar el cupón.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error inesperado al agregar el cupón.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inesperado al agregar el cupón.");
            }
        }

        // DELETE: api/Cupones_Historial/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupones_HistorialModel(int id)
        {
            try
            {
                var cupones_HistorialModel = await _context.Cupones_Historial.FindAsync(id);
                if (cupones_HistorialModel == null)
                {
                    Log.Warning($"El cupón a eliminar con ID: {id} no se encontró.");
                    return NotFound();
                }

                Log.Information($"Se eliminó el rastro de un cupón con ID: {id}.");
                _context.Cupones_Historial.Remove(cupones_HistorialModel);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al eliminar el cupón del historial.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el cupón.");
            }
        }

        private bool Cupones_HistorialModelExists(int id)
        {
            return _context.Cupones_Historial.Any(e => e.Id_Cupon == id);
        }
    }
}