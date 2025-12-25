using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.ViewModel
{
    public class MaterialPropertyVM
    {  
        
        private string _name="";
        private string _claseAgma = "";
        private string _designacion = "";
        private string _tratamiento = "";
        public int IdPropiedad { get; set; }
        public string NombreMaterial { get { return _name; } set { _name = value; } }
        public string ClaseAgma { get { return _claseAgma; } set { _claseAgma = value; } }
        public string Designacion { get { return _designacion; } set { _designacion = value; } }
        public string Tratamiento { get { return _tratamiento; } set { _tratamiento = value; } }
        public double ResistenciaMin_MPa { get; set; }
        public double ResistenciaMax_MPa { get; set; }

    }
}
