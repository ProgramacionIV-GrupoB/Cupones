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
            return await _context.Precios.ToListAsync();
        }

        // GET: api/Precios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PreciosModel>> GetPreciosModel(int id)
        {
            var preciosModel = await _context.Precios.FindAsync(id);

            if (preciosModel == null)
            {
                return NotFound();
            }

            return preciosModel;
        }

        // PUT: api/Precios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreciosModel(int id, PreciosModel preciosModel)
        {
            if (id != preciosModel.Id_Precio)
            {
                return BadRequest();
            }

            _context.Entry(preciosModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreciosModelExists(id))
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

        // POST: api/Precios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PreciosModel>> PostPreciosModel(PreciosModel preciosModel)
        {
            _context.Precios.Add(preciosModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreciosModel", new { id = preciosModel.Id_Precio }, preciosModel);
        }

        // DELETE: api/Precios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreciosModel(int id)
        {
            var preciosModel = await _context.Precios.FindAsync(id);
            if (preciosModel == null)
            {
                return NotFound();
            }

            preciosModel.Precio = 0;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreciosModelExists(int id)
        {
            return _context.Precios.Any(e => e.Id_Precio == id);
        }
    }
}
