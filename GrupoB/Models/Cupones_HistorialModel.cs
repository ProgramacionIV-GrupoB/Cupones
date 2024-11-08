using System.ComponentModel.DataAnnotations;

namespace CuponesApi.Models
{
    public class Cupones_HistorialModel
    {
        [Key]
        public int Id_Cupon { get; set; }
        [Key]
        public string NroCupon { get; set; }
        public DateTime? FechaUso { get; set; }
        public string CodCliente { get; set; }  
    }
}
