using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientesApi.Data;
using ClientesApi.Models;
using ClientesApi.Interfaces;
using ClientesApi.Models.DTO;
using Serilog;

namespace ClientesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public ClientesController(DataBaseContext context)
        {
            _context = context;
        }

        // Alta: Crear un nuevo cliente
        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClientesModel cliente)
        {
            if (cliente == null)
            {
                Log.Error($"Se intentó dar de alta un cliente, pero no se especificó nada :/");
                return BadRequest("El cliente no puede ser nulo.");
            }

            // Verificar si el CodCliente existe en la base de datos
            if (await _context.Clientes.AnyAsync(c => c.CodCliente == cliente.CodCliente))
            {
                Log.Error($"Se intentó dar de alta un cliente, pero ocurrió un conflicto: Ya existe el CodCliente.");
                return Conflict("El código de cliente ya existe.");
            }

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            Log.Information($"¡Se dió de alta un cliente!");
            return CreatedAtAction(nameof(ObtenerClientePorCodCliente), new { codCliente = cliente.CodCliente }, new { mensaje = "Cliente creado exitosamente.", cliente });
        }

        // Baja: Eliminar un cliente por CodCliente
        [HttpDelete("{codCliente}")]
        public async Task<IActionResult> EliminarCliente(string codCliente)
        {
            var cliente = await _context.Clientes.FindAsync(codCliente);
            if (cliente == null)
            {
                Log.Error($"Se intentó dar de baja un cliente, pero no se encontró");
                return NotFound("Cliente no encontrado.");
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            Log.Information($"Se dió de baja a un cliente.");
            return Ok(new { mensaje = "Cliente eliminado exitosamente." });
        }

        // Modificar un cliente existente por código de cliente
        [HttpPut("{codCliente}")]
        public async Task<IActionResult> ActualizarCliente(string codCliente, [FromBody] ClientesModel clienteActualizado)
        {
            if (clienteActualizado == null)
            {
                Log.Error($"Se intentó modificar un cliente, pero no se especificó nada :/");
                return BadRequest("El cliente actualizado no puede ser nulo.");
            }

            if (codCliente != clienteActualizado.CodCliente)
            {
                Log.Error($"Se intentó modificar un cliente, pero el CodCliente no coincide con la solicitud.");
                return BadRequest("El código del cliente no coincide.");
            }

            var clienteExistente = await _context.Clientes.FindAsync(codCliente);
            if (clienteExistente == null)
            {
                Log.Error($"Se intentó modificar un cliente, pero no se encontró.");
                return NotFound("Cliente no encontrado.");
            }

            // Realizar actualización de campos
            clienteExistente.Nombre_Cliente = clienteActualizado.Nombre_Cliente;
            clienteExistente.Apellido_Cliente = clienteActualizado.Apellido_Cliente;
            clienteExistente.Direccion = clienteActualizado.Direccion;
            clienteExistente.Email = clienteActualizado.Email;

            await _context.SaveChangesAsync();

            Log.Information($"Se modificó un cliente.");
            return Ok(new { mensaje = "Cliente modificado exitosamente.", cliente = clienteExistente });
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesModel>>> ObtenerClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();

            if (!clientes.Any())
            {
                return NotFound(new { mensaje = "No hay clientes registrados." });
            }

            Log.Information($"Se llamó al endpoint para obtener todos los clientes.");
            return Ok(clientes);
        }


        // Obtener un cliente por CodCliente
        [HttpGet("{codCliente}")]
        public async Task<ActionResult<ClientesModel>> ObtenerClientePorCodCliente(string codCliente)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.CodCliente == codCliente);
            Log.Information($"Se llamó al endpoint para obtener un cliente por CodCliente.");

            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            return Ok(cliente);
        }
    }
}



