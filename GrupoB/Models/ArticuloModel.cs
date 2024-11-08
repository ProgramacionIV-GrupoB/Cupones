using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuponesApi.Models
{
    public class ArticuloModel
    {
        [Key]
        public int Id_Articulo { get; set; }
        public string Nombre_Articulo {get; set; }
        public string Descripcion_Articulo { get; set; }
        public bool Activo {  get; set; }
        [ForeignKey("Id_Articulo")]
        public virtual PrecioModel? Precio { get; set; } 
    }
}
