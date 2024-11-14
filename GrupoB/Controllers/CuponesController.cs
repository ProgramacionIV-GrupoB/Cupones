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
    public class CuponesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public CuponesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Cupones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuponModel>>> GetCupones()
        {
            return await _context
                .Cupones
                .Where(c => c.Activo) // Agregamos el filtro de activos
                .Include(c => c.Cupones_Categorias)
                .ThenInclude(cc => cc.Categoria)
                .Include(c => c.Tipo_Cupon)
                .ToListAsync();
        }


        // GET: api/Cupones/5
        // GET: api/Cupones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuponModel>> GetCuponModel(int id)
        {
            var cuponModel = await _context.Cupones
                .Where(c => c.Activo)
                .FirstOrDefaultAsync(c => c.Id_Cupon == id);

            if (cuponModel == null)
            {
                return NotFound();
            }

            return cuponModel;
        }

        public class CuponClienteDTO
        {
            public CuponModel Cupon { get; set; }
            public string NroCupon { get; set; }
            public DateTime? FechaAsignado { get; set; }
        }

        // Endpoint modificado para usar el DTO
        [HttpGet("cliente/{codCliente}")]
        public async Task<ActionResult<IEnumerable<CuponClienteDTO>>> GetCuponesByCliente(string codCliente)
        {
            var cuponesCliente = await _context.Cupones_Clientes
                .Where(cc => cc.CodCliente == codCliente)
                .Select(cc => new CuponClienteDTO
                {
                    Cupon = _context.Cupones
                        .Include(c => c.Cupones_Categorias)
                        .ThenInclude(cc => cc.Categoria)
                        .Include(c => c.Tipo_Cupon)
                        .FirstOrDefault(c => c.Id_Cupon == cc.Id_Cupon && c.Activo),
                    NroCupon = cc.NroCupon,
                    FechaAsignado = cc.FechaAsignado
                })
                .Where(dto => dto.Cupon != null) // Solo poner si el cupón existe
                .ToListAsync();

            if (!cuponesCliente.Any())
            {
                return NotFound($"No se encontraron cupones para el cliente {codCliente}");
            }

            return cuponesCliente;
        }

        // PUT: api/Cupones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuponModel(int id, CuponModel cuponModel)
        {
            if (id != cuponModel.Id_Cupon)
            {
                return BadRequest();
            }

            _context.Entry(cuponModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuponModelExists(id))
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

        // POST: api/Cupones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CuponModel>> PostCuponModel(CuponModel cuponModel)
        {
            _context.Cupones.Add(cuponModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCuponModel", new { id = cuponModel.Id_Cupon }, cuponModel);
        }

        // DELETE: api/Cupones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuponModel(int id)
        {
            var cuponModel = await _context.Cupones.FindAsync(id);
            if (cuponModel == null)
            {
                return NotFound();
            }

            cuponModel.Activo = false; // Lo cambie para cumplir con el requerimiento del trabajo, antes borraba el registro
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CuponModelExists(int id)
        {
            return _context.Cupones.Any(e => e.Id_Cupon == id);
        }
    }
}
