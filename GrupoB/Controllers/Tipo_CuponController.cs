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
    public class Tipo_CuponController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public Tipo_CuponController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Tipo_Cupon  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipo_CuponModel>>> GetTipo_Cupon()
        {
            Log.Information("Se llamó al endpoint para obtener los tipos de los cupones.");
            try
            {
                var tiposCupon = await _context.Tipo_Cupon.ToListAsync();
                return Ok(tiposCupon);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los tipos de los cupones.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los tipos de cupones.");
            }
        }

        // GET: api/Tipo_Cupon/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo_CuponModel>> GetTipo_CuponModel(int id)
        {
            Log.Information($"Se llamó al endpoint para obtener el TipoCupón con ID: {id}.");
            try
            {
                var tipo_CuponModel = await _context.Tipo_Cupon.FindAsync(id);
                if (tipo_CuponModel == null)
                {
                    Log.Warning($"TipoCupón con ID: {id} no encontrado.");
                    return NotFound();
                }
                return Ok(tipo_CuponModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al obtener el TipoCupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el TipoCupón.");
            }
        }

        // PUT: api/Tipo_Cupon/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipo_CuponModel(int id, Tipo_CuponModel tipo_CuponModel)
        {
            if (id != tipo_CuponModel.Id_Tipo_Cupon)
            {
                return BadRequest();
            }

            _context.Entry(tipo_CuponModel).State = EntityState.Modified;

            try
            {
                Log.Information($"Se modificó el tipo de cupón con ID: {id}.");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tipo_CuponModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    Log.Error($"Error de concurrencia al actualizar el TipoCupón con ID: {id}.");
                    return StatusCode(StatusCodes.Status409Conflict, "Error de concurrencia al actualizar el tipo de cupón.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error inesperado al modificar el tipo de cupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al modificar el tipo de cupón.");
            }

            return NoContent();
        }

        // POST: api/Tipo_Cupon  
        [HttpPost]
        public async Task<ActionResult<Tipo_CuponModel>> PostTipo_CuponModel(Tipo_CuponModel tipo_CuponModel)
        {
            try
            {
                _context.Tipo_Cupon.Add(tipo_CuponModel);
                Log.Information("Se estableció un nuevo tipo de cupón.");
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipo_CuponModel), new { id = tipo_CuponModel.Id_Tipo_Cupon }, tipo_CuponModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear un nuevo tipo de cupón.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear el tipo de cupón.");
            }
        }

        // DELETE: api/Tipo_Cupon/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo_CuponModel(int id)
        {
            try
            {
                var tipo_CuponModel = await _context.Tipo_Cupon.FindAsync(id);
                if (tipo_CuponModel == null)
                {
                    Log.Warning($"El TipoCupón con ID: {id} no encontrado.");
                    return NotFound();
                }

                Log.Information($"Se eliminó el tipo de cupón con ID: {id}.");
                _context.Tipo_Cupon.Remove(tipo_CuponModel);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al eliminar el TipoCupón con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el tipo de cupón.");
            }
        }

        private bool Tipo_CuponModelExists(int id)
        {
            return _context.Tipo_Cupon.Any(e => e.Id_Tipo_Cupon == id);
        }
    }
}