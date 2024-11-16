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
    public class PreciosController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public PreciosController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Precios  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreciosModel>>> GetPrecios()
        {
            Log.Information("Solicitud para obtener todos los precios.");
            try
            {
                var precios = await _context.Precios.ToListAsync();
                return Ok(precios);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener la lista de precios.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los precios.");
            }
        }

        // GET: api/Precios/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<PreciosModel>> GetPreciosModel(int id)
        {
            Log.Information($"Solicitud para obtener un precio con ID: {id}.");
            try
            {
                var preciosModel = await _context.Precios.FindAsync(id);
                if (preciosModel == null)
                {
                    Log.Warning($"Precio con ID: {id} no encontrado.");
                    return NotFound();
                }
                return Ok(preciosModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al obtener el precio con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el precio.");
            }
        }

        // PUT: api/Precios/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreciosModel(int id, PreciosModel preciosModel)
        {
            if (id != preciosModel.Id_Precio)
            {
                Log.Warning($"El ID proporcionado {id} no coincide con el ID del modelo {preciosModel.Id_Precio}.");
                return BadRequest();
            }

            _context.Entry(preciosModel).State = EntityState.Modified;

            try
            {
                Log.Information($"Modificación del precio con ID: {id}.");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreciosModelExists(id))
                {
                    Log.Warning($"Precio con ID: {id} no encontrado durante la actualización.");
                    return NotFound();
                }
                else
                {
                    Log.Error($"Error de concurrencia al actualizar el precio con ID: {id}.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error inesperado al modificar el precio con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al modificar el precio.");
            }

            return NoContent();
        }

        // POST: api/Precios  
        [HttpPost]
        public async Task<ActionResult<PreciosModel>> PostPreciosModel(PreciosModel preciosModel)
        {
            try
            {
                _context.Precios.Add(preciosModel);
                Log.Information("Creación de un nuevo precio.");
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPreciosModel", new { id = preciosModel.Id_Precio }, preciosModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear un nuevo precio.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el precio.");
            }
        }

        // DELETE: api/Precios/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreciosModel(int id)
        {
            try
            {
                var preciosModel = await _context.Precios.FindAsync(id);
                if (preciosModel == null)
                {
                    Log.Warning($"El precio a eliminar con ID: {id} no encontrado.");
                    return NotFound();
                }

                Log.Information($"Eliminación (desactivación) del precio con ID: {id}.");
                preciosModel.Precio = 0; // Establecemos el precio a 0 en lugar de eliminar  
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al eliminar el precio con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el precio.");
            }
        }

        private bool PreciosModelExists(int id)
        {
            return _context.Precios.Any(e => e.Id_Precio == id);
        }
    }
}