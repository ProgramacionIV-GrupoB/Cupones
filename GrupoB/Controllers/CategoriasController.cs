﻿using System;
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
            Log.Information($"Se consultó al endpoint para obtener todas las categorías");
            return await _context
                .Categorias
                .Include(c=>c.Cupones_Categorias)
                .ThenInclude(cc=>cc.Cupon)
                .ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaModel>> GetCategoriaModel(int id)
        {
            Log.Information($"Se consultó al endpoint para una categoría por ID");
            var categoriaModel = await _context.Categorias.FindAsync(id);

            if (categoriaModel == null)
            {
                return NotFound();
            }

            return categoriaModel;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaModel(int id, CategoriaModel categoriaModel)
        {
            if (id != categoriaModel.Id_Categoria)
            {
                return BadRequest();
            }

            _context.Entry(categoriaModel).State = EntityState.Modified;

            try
            {
                Log.Information($"Se modificó una categoría.");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaModelExists(id))
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

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoriaModel>> PostCategoriaModel(CategoriaModel categoriaModel)
        {
            _context.Categorias.Add(categoriaModel);
            Log.Information($"Se creó una categoría nueva.");
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoriaModel", new { id = categoriaModel.Id_Categoria }, categoriaModel);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaModel(int id)
        {
            var categoriaModel = await _context.Categorias.FindAsync(id);
            if (categoriaModel == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoriaModel);
            Log.Information($"Se eliminó una categoría.");
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaModelExists(int id)
        {
            return _context.Categorias.Any(e => e.Id_Categoria == id);
        }
    }
}
