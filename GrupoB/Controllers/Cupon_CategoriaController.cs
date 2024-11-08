using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CuponesApi.Data;
using CuponesApi.Models;

namespace CuponesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cupon_CategoriaController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public Cupon_CategoriaController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupon_Categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupon_CategoriaModel>>> GetCupones_Categorias()
        {
            return await _context.Cupones_Categorias.ToListAsync();
        }

        // GET: api/Cupon_Categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupon_CategoriaModel>> GetCupon_CategoriaModel(int id)
        {
            var cupon_CategoriaModel = await _context.Cupones_Categorias.FindAsync(id);

            if (cupon_CategoriaModel == null)
            {
                return NotFound();
            }

            return cupon_CategoriaModel;
        }

        // PUT: api/Cupon_Categoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupon_CategoriaModel(int id, Cupon_CategoriaModel cupon_CategoriaModel)
        {
            if (id != cupon_CategoriaModel.Id_Cupones_Categorias)
            {
                return BadRequest();
            }

            _context.Entry(cupon_CategoriaModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cupon_CategoriaModelExists(id))
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

        // POST: api/Cupon_Categoria
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cupon_CategoriaModel>> PostCupon_CategoriaModel(Cupon_CategoriaModel cupon_CategoriaModel)
        {
            _context.Cupones_Categorias.Add(cupon_CategoriaModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCupon_CategoriaModel", new { id = cupon_CategoriaModel.Id_Cupones_Categorias }, cupon_CategoriaModel);
        }

        // DELETE: api/Cupon_Categoria/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupon_CategoriaModel(int id)
        {
            var cupon_CategoriaModel = await _context.Cupones_Categorias.FindAsync(id);
            if (cupon_CategoriaModel == null)
            {
                return NotFound();
            }

            _context.Cupones_Categorias.Remove(cupon_CategoriaModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Cupon_CategoriaModelExists(int id)
        {
            return _context.Cupones_Categorias.Any(e => e.Id_Cupones_Categorias == id);
        }
    }
}
