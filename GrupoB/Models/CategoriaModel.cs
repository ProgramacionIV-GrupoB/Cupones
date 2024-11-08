using System.ComponentModel.DataAnnotations;



namespace CuponesApi.Models
{
    public class CategoriaModel
    {
        [Key]
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Cupon_CategoriaModel>? Cupones_Categorias { get; set; }
    }

}
   


