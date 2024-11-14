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
    public class Cupones_DetallesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public Cupones_DetallesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupones_Detalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupones_DetallesModel>>> GetCupones_Detalles()
        {
            Log.Information($"Se llamó al endpoint para obtener los detalles de los cupones.");
            return await _context.Cupones_Detalle.ToListAsync();
        }

        // GET: api/Cupones_Detalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Cupones_DetallesModel>>> GetCupones_DetallesModel(int id)
        {
            var cupones_DetallesModel = await _context.Cupones_Detalle.Where(cd=>cd.Id_Cupon==id).ToListAsync();

            if (cupones_DetallesModel == null)
            {
                return NotFound();
            }

            Log.Information($"Se llamó al endpoint para obtener el detalle de un cupón por ID.");
            return cupones_DetallesModel;
        }

        // PUT: api/Cupones_Detalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupones_DetallesModel(int id ,  Cupones_DetallesModel cupones_DetallesModel)
        {
            if (id != cupones_DetallesModel.Id_Cupon)
            {
                return BadRequest();
            }

            Log.Information($"Se modificó el detalle de un cupón.");
            _context.Entry(cupones_DetallesModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cupones_DetallesModelExists(id))
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

        // POST: api/Cupones_Detalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cupones_DetallesModel>> PostCupones_DetallesModel(Cupones_DetallesModel cupones_DetallesModel)
        {
            _context.Cupones_Detalle.Add(cupones_DetallesModel);
            await _context.SaveChangesAsync();

            Log.Information($"Se crearon los detalles de un cupón.");
            return CreatedAtAction("GetCupones_DetallesModel", new { id = cupones_DetallesModel.Id_Cupon }, cupones_DetallesModel);
        }

        // DELETE: api/Cupones_Detalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupones_DetallesModel(int id)
        {
            var cupones_DetallesModel = await _context.Cupones_Detalle.FindAsync(id);
            if (cupones_DetallesModel == null)
            {
                return NotFound();
            }

            _context.Cupones_Detalle.Remove(cupones_DetallesModel);
            Log.Information($"Se eliminaron los detalles de un cupón.");
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Cupones_DetallesModelExists(int id)
        {
            return _context.Cupones_Detalle.Any(e => e.Id_Cupon == id);
        }
    }
}
