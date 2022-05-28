using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SandyStore.Models
{
    [MetadataType(typeof(Departamentos.Metadata))]
    public class Departamentos
    {
        public int DepID { get; set; }
        public string DepDescripcion { get; set; }

        private class Metadata
        {
            [DisplayName("ID")]
            [DataType(DataType.Text)]
            public int ProID { get; set; }

            [DisplayName("Departamento")]
            [DataType(DataType.Text)]
            public int DepDescripcion { get; set; }
        }
    }    
}