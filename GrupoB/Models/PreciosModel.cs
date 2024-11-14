using CuponesApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text;

public class PreciosModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id_Precio { get; set; }

    public int Id_Articulo { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [JsonIgnore] // Esto es para que no aparezca el campo "articulo" en precios en la solicitud
    [ForeignKey("Id_Articulo")]
    public virtual ArticulosModel? Articulo { get; set; }
}