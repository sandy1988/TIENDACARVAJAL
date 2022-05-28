using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SandyStore.Models
{
    [MetadataType(typeof(Metadata))]
    public partial class Productos
    {
        public int ProID { get; set; }
        public int DepID { get; set; }
        public string DepDescripcion { get; set; }
        public string ProNombre { get; set; }
        public string ProDescripcion { get; set; }
        public double? ProValor { get; set; }
        public string ProRutaImagen { get; set; }
        private class Metadata
        {
            [DisplayName("ID")]
            [DataType(DataType.Text)]
            public int ProID { get; set; }

            [DisplayName("Departamento")]
            [DataType(DataType.Text)]
            public string DepDescripcion { get; set; }

            [DisplayName("Nombre")]
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Debe ingresar el nombre del producto")]
            public string ProNombre { get; set; }

            [DisplayName("Descripcion")]
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Debe ingresar la descripcion del producto")]
            public string ProDescripcion { get; set; }

            [DisplayName("Valor")]
            [Range(1, 9999999999999999.99), DataType(DataType.Currency)]
            [Required(ErrorMessage = "Debe ingresar el valor del producto")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal ProValor { get; set; }

            [DisplayName("Imagen")]
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Debe ingresar la ruta de la imagen del producto")]
            public string ProRutaImagen { get; set; }
            
        }
    }
}