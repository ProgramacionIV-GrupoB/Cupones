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
    public class ArticulosController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public ArticulosController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Articulo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticulosModel>>> GetArticulos()
        {
            // Fetch only active Articulos
            var articulos = await _context.Articulos
                .Include(a => a.Precio)
                .Where(a => a.Activo) // Excluir los que fueron eliminados
                .ToListAsync();
            Log.Information($"Se consultó al endpoint para obtener todos los artículos");
            return articulos;
        }


        // GET: api/Articulo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticulosModel>> GetArticuloModel(int id)
        {
            var articuloModel = await _context.Articulos
                .Where(a => a.Id_Articulo == id && a.Activo) 
                .FirstOrDefaultAsync();

            if (articuloModel == null)
            {
                return NotFound();
            }

            Log.Information($"Se consultó al endpoint para obtener un artículo por ID");
            return articuloModel;
        }


        // PUT: api/Articulo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticuloModel(int id, ArticulosModel articuloModel)
        {
            if (id != articuloModel.Id_Articulo)
            {
                return BadRequest();
            }

            var existingArticulo = await _context.Articulos.FindAsync(id); // Buscar si el artículo está activo
            if (existingArticulo == null || !existingArticulo.Activo) 
            {
                return NotFound(); 
            }

            _context.Entry(articuloModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                Log.Information($"Se modificó un artículo");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ArticulosModel>> PostArticuloModel(ArticulosModel articuloModel)
        {
            try
            {
                var nuevoArticulo = new ArticulosModel
                {
                    Nombre_Articulo = articuloModel.Nombre_Articulo,
                    Descripcion_Articulo = articuloModel.Descripcion_Articulo,
                    Activo = articuloModel.Activo
                };

                _context.Articulos.Add(nuevoArticulo);
                await _context.SaveChangesAsync();

                if (articuloModel.Precio != null)
                {
                    var nuevoPrecio = new PreciosModel
                    {
                        Id_Articulo = nuevoArticulo.Id_Articulo,
                        Precio = articuloModel.Precio.Precio
                    };

                    _context.Precios.Add(nuevoPrecio);
                    await _context.SaveChangesAsync();

                    nuevoArticulo.Precio = nuevoPrecio;
                }

                Log.Information($"Se creó un nuevo artículo");
                return CreatedAtAction("GetArticuloModel",
                    new { id = nuevoArticulo.Id_Articulo },
                    nuevoArticulo);
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error interno al crear un artículo: {ex.Message}");
                return StatusCode(500, "Error interno al crear el artículo: " + ex.Message);
            }
        }

        // DELETE: api/Articulo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticuloModel(int id)
        {
            var articuloModel = await _context.Articulos.FindAsync(id);
            if (articuloModel == null)
            {
                return NotFound();
            }

            articuloModel.Activo = false; // Lo cambie para cumplir con el requerimiento del trabajo, antes borraba el registro
            await _context.SaveChangesAsync();
            Log.Information($"Se eliminó un artículo");

            return NoContent();
        }

        private bool ArticuloModelExists(int id)
        {
            return _context.Articulos.Any(e => e.Id_Articulo == id);
        }
    }
}
