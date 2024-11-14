using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuponesApi.Models
{
    public class Articulo_CategoriaModel
    {
        [Key]
        public int Id_Articulo_Categoria { get; set; }

        [ForeignKey("Articulo")]
        public int Id_Articulo { get; set; }
        public virtual ArticulosModel? Articulo { get; set; }

        [ForeignKey("Categoria")]
        public int Id_Categoria { get; set; }
        public virtual CategoriaModel? Categoria { get; set; }
    }
}