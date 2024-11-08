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
            return await _context.Cupones_Historial.ToListAsync();
        }

        // GET: api/Cupones_Historial/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupones_HistorialModel>> GetCupones_HistorialModel(int id)
        {
            var cupones_HistorialModel = await _context.Cupones_Historial.FindAsync(id);

            if (cupones_HistorialModel == null)
            {
                return NotFound();
            }

            return cupones_HistorialModel;
        }

        // PUT: api/Cupones_Historial/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupones_HistorialModel(int id, Cupones_HistorialModel cupones_HistorialModel)
        {
            if (id != cupones_HistorialModel.Id_Cupon)
            {
                return BadRequest();
            }

            _context.Entry(cupones_HistorialModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cupones_HistorialModelExists(id))
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

        // POST: api/Cupones_Historial
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cupones_HistorialModel>> PostCupones_HistorialModel(Cupones_HistorialModel cupones_HistorialModel)
        {
            _context.Cupones_Historial.Add(cupones_HistorialModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Cupones_HistorialModelExists(cupones_HistorialModel.Id_Cupon))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCupones_HistorialModel", new { id = cupones_HistorialModel.Id_Cupon }, cupones_HistorialModel);
        }

        // DELETE: api/Cupones_Historial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupones_HistorialModel(int id)
        {
            var cupones_HistorialModel = await _context.Cupones_Historial.FindAsync(id);
            if (cupones_HistorialModel == null)
            {
                return NotFound();
            }

            _context.Cupones_Historial.Remove(cupones_HistorialModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Cupones_HistorialModelExists(int id)
        {
            return _context.Cupones_Historial.Any(e => e.Id_Cupon == id);
        }
    }
}
