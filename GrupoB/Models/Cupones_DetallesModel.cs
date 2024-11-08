using System.ComponentModel.DataAnnotations;


namespace CuponesApi.Models
{
    public class Cupones_DetallesModel
    {
        [Key]
        public int Id_Cupon { get; set; }
        [Key]
        public int Id_Articulo { get; set; }
        public int Cantidad { get; set; }


    }
}
