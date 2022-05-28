using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaCarvajal.Models
{
    [MetadataType(typeof(Usuarios.Metadata))]
    public class Usuarios
    {
        [Key]
        public int UsuID { get; set; }
        public string UsuNombre { get; set; }
        public string UsuPass { get; set; }
        private class Metadata
        {
            [DisplayName("ID")]
            [DataType(DataType.Text)]
            public int UsuID { get; set; }

            [DisplayName("DESCRIPCION")]
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Debe ingresar el nombre")]
            public string UsuNombre { get; set; }

            [DisplayName("DESCRIPCION")]
            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Debe ingresar el password")]
            public string UsuPass { get; set; }
        }
    }
}