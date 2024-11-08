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
            return await _context.Tipo_Cupon.ToListAsync();
        }

        // GET: api/Tipo_Cupon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipo_CuponModel>> GetTipo_CuponModel(int id)
        {
            var tipo_CuponModel = await _context.Tipo_Cupon.FindAsync(id);

            if (tipo_CuponModel == null)
            {
                return NotFound();
            }

            return tipo_CuponModel;
        }

        // PUT: api/Tipo_Cupon/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tipo_Cupon
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tipo_CuponModel>> PostTipo_CuponModel(Tipo_CuponModel tipo_CuponModel)
        {
            _context.Tipo_Cupon.Add(tipo_CuponModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipo_CuponModel", new { id = tipo_CuponModel.Id_Tipo_Cupon }, tipo_CuponModel);
        }

        // DELETE: api/Tipo_Cupon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo_CuponModel(int id)
        {
            var tipo_CuponModel = await _context.Tipo_Cupon.FindAsync(id);
            if (tipo_CuponModel == null)
            {
                return NotFound();
            }

            _context.Tipo_Cupon.Remove(tipo_CuponModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Tipo_CuponModelExists(int id)
        {
            return _context.Tipo_Cupon.Any(e => e.Id_Tipo_Cupon == id);
        }
    }
}
