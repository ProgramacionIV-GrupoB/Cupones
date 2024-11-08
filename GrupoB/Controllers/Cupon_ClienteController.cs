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
    public class Cupon_ClienteController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public Cupon_ClienteController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupon_Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupon_ClienteModel>>> GetCupones_Clientes()
        {
            return await _context.Cupones_Clientes.ToListAsync();
        }

        // GET: api/Cupon_Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupon_ClienteModel>> GetCupon_ClienteModel(string id)
        {
            var cupon_ClienteModel = await _context.Cupones_Clientes.FindAsync(id);

            if (cupon_ClienteModel == null)
            {
                return NotFound();
            }

            return cupon_ClienteModel;
        }

        // PUT: api/Cupon_Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupon_ClienteModel(string id, Cupon_ClienteModel cupon_ClienteModel)
        {
            if (id != cupon_ClienteModel.NroCupon)
            {
                return BadRequest();
            }

            _context.Entry(cupon_ClienteModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cupon_ClienteModelExists(id))
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

        // POST: api/Cupon_Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cupon_ClienteModel>> PostCupon_ClienteModel(Cupon_ClienteModel cupon_ClienteModel)
        {
            cupon_ClienteModel.FechaAsignado = DateTime.Now;

            _context.Cupones_Clientes.Add(cupon_ClienteModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Cupon_ClienteModelExists(cupon_ClienteModel.NroCupon))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Se dio de alta el registro en Cupon_Cliente");        
        }

        // DELETE: api/Cupon_Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupon_ClienteModel(string id)
        {
            var cupon_ClienteModel = await _context.Cupones_Clientes.FindAsync(id);
            if (cupon_ClienteModel == null)
            {
                return NotFound();
            }

            _context.Cupones_Clientes.Remove(cupon_ClienteModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Cupon_ClienteModelExists(string id)
        {
            return _context.Cupones_Clientes.Any(e => e.NroCupon == id);
        }
    }
}
