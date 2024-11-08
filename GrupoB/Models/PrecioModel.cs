using System.ComponentModel.DataAnnotations;

namespace CuponesApi.Models
{
    public class PrecioModel
    {
        [Key]
        public int Id_Precio { get; set; }
        public int Id_Articulo { get; set; }
        public decimal Precio { get; set; }

    }
}
