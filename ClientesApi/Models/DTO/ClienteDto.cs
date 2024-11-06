using System.ComponentModel.DataAnnotations;

namespace ClientesApi.Models.DTO
{
    public class ClienteDto
    {
        public int Id_Cupon { get; set; }
        public string CodCliente { get; set; }
        public string Email { get; set; }
    }
}
