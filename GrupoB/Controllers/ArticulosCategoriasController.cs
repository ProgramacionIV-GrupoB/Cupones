using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CuponesApi.Data;
using CuponesApi.Models;
using Serilog;


namespace CuponesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosCategoriasController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public ArticulosCategoriasController(DataBaseContext context)
        {
            _context = context;
        }

        public class AsignacionRequest
        {
            public int Id_Categoria { get; set; }
            public List<int> Id_Articulos { get; set; }
        }

        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarArticulosACategoria([FromBody] AsignacionRequest request)
        {
            try
            {
                if (request.Id_Articulos == null || !request.Id_Articulos.Any())
                {
                    Log.Error($"Se intentó asignar un artículo una categoría, pero no se especifíco al menos un artículo.");
                    return BadRequest("Debe especificar al menos un artículo");
                }

                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Id_Categoria == request.Id_Categoria);

                if (categoria == null)
                {
                    Log.Error($"Se intentó asignar un artículo una categoría, pero no se encontró la categoría con ID {request.Id_Categoria}.");
                    return NotFound($"No se encontró la categoría con ID {request.Id_Categoria}");
                }

                var articulosExistentes = await _context.Articulos
                    .Where(a => request.Id_Articulos.Contains(a.Id_Articulo))
                    .Select(a => a.Id_Articulo)
                    .ToListAsync();

                if (articulosExistentes.Count != request.Id_Articulos.Count)
                {
                    var articulosNoEncontrados = request.Id_Articulos
                        .Except(articulosExistentes)
                        .ToList();

                    Log.Error($"Se intentó asignar artículos a una categoría, pero no se encontraron los siguientes:  {string.Join(", ", articulosNoEncontrados)}.");
                    return BadRequest($"No se encontraron los siguientes artículos: {string.Join(", ", articulosNoEncontrados)}");
                }

                var asignacionesExistentes = await _context.Articulos_Categorias
                    .Where(ac => ac.Id_Categoria == request.Id_Categoria &&
                           request.Id_Articulos.Contains(ac.Id_Articulo))
                    .Select(ac => ac.Id_Articulo)
                    .ToListAsync();

                var nuevosArticulos = request.Id_Articulos
                    .Except(asignacionesExistentes)
                    .ToList();

                if (!nuevosArticulos.Any())
                {
                    Log.Information($"Se intentó agregar artículos a una categoria pero ya estaban asignados, no hay cambios.");
                    return Ok("Todos los artículos ya estaban asignados a la categoría");
                }

                var nuevasAsignaciones = nuevosArticulos.Select(idArticulo => new Articulo_CategoriaModel
                {
                    Id_Articulo = idArticulo,
                    Id_Categoria = request.Id_Categoria
                }).ToList();

                await _context.Articulos_Categorias.AddRangeAsync(nuevasAsignaciones);
                await _context.SaveChangesAsync();

                Log.Information($"Se agregaron {nuevasAsignaciones.Count} artículos a una categoría");
                return Ok(new
                {
                    Mensaje = $"Se asignaron {nuevasAsignaciones.Count} artículos a la categoría exitosamente",
                    ArticulosAsignados = nuevosArticulos
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error interno al intentar asignar artículos: {ex.Message}");
                return StatusCode(500, $"Error interno al asignar artículos: {ex.Message}");

            }
        }
    }
}