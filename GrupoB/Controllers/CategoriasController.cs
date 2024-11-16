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
    public class CategoriasController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public CategoriasController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Categorias  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaModel>>> GetCategorias()
        {
            Log.Information("Se consultó al endpoint para obtener todas las categorías");
            try
            {
                var categorias = await _context
                    .Categorias
                    .Include(c => c.Cupones_Categorias)
                    .ThenInclude(cc => cc.Cupon)
                    .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener las categorías.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener las categorías.");
            }
        }

        // GET: api/Categorias/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaModel>> GetCategoriaModel(int id)
        {
            Log.Information($"Se consultó al endpoint para una categoría por ID: {id}");
            try
            {
                var categoriaModel = await _context.Categorias.FindAsync(id);
                if (categoriaModel == null)
                {
                    Log.Warning($"Categoría con ID: {id} no encontrada.");
                    return NotFound();
                }
                return Ok(categoriaModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al obtener la categoría con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener la categoría.");
            }
        }

        // PUT: api/Categorias/5  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaModel(int id, CategoriaModel categoriaModel)
        {
            if (id != categoriaModel.Id_Categoria)
            {
                Log.Warning($"ID proporcionado {id} no coincide con el ID del modelo {categoriaModel.Id_Categoria}.");
                return BadRequest();
            }

            _context.Entry(categoriaModel).State = EntityState.Modified;

            try
            {
                Log.Information($"Se modificó una categoría con ID: {id}.");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaModelExists(id))
                {
                    Log.Warning($"Categoría con ID: {id} no encontrada durante la actualización.");
                    return NotFound();
                }
                else
                {
                    Log.Error($"Error de concurrencia al actualizar la categoría con ID: {id}.");
                    return StatusCode(StatusCodes.Status409Conflict, "Error de concurrencia al actualizar la categoría.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error inesperado al modificar la categoría con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al modificar la categoría.");
            }

            return NoContent();
        }

        // POST: api/Categorias  
        [HttpPost]
        public async Task<ActionResult<CategoriaModel>> PostCategoriaModel(CategoriaModel categoriaModel)
        {
            try
            {
                _context.Categorias.Add(categoriaModel);
                Log.Information("Se creó una nueva categoría.");
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoriaModel), new { id = categoriaModel.Id_Categoria }, categoriaModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear una nueva categoría.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la categoría.");
            }
        }

        // DELETE: api/Categorias/5  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaModel(int id)
        {
            try
            {
                var categoriaModel = await _context.Categorias.FindAsync(id);
                if (categoriaModel == null)
                {
                    Log.Warning($"Categoría con ID: {id} no encontrada para eliminación.");
                    return NotFound();
                }

                _context.Categorias.Remove(categoriaModel);
                Log.Information($"Se eliminó la categoría con ID: {id}.");
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error al eliminar la categoría con ID: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar la categoría.");
            }
        }

        private bool CategoriaModelExists(int id)
        {
            return _context.Categorias.Any(e => e.Id_Categoria == id);
        }
    }
}