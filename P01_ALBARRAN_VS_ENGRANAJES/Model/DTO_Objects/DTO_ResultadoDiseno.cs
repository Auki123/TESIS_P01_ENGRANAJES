using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects
{
    public class DTO_ResultadoDiseno
    {
        private double _sigmab;
        private double _Sfb_prima;
        private double _Sfb;
        private double _factorSeguridad;

        // Campos de las propiedades de los materiales seleccionados
        private string _nombreMaterial = "";
        private string _claseAgma="";
        private string _designacionMaterial="";
        private string _TratamientoMaterial="";

        //__________________________________________

        public double SIGMAB { get { return _sigmab; } set { _sigmab = value; } }
        public double Sfb_prima { get { return _Sfb_prima; } set { _Sfb_prima = value; } }
        public double Sfb { get { return _Sfb; } set { _Sfb = value; } }
        public double FactorSeguridad { get { return _factorSeguridad; } set { _factorSeguridad = value; } }

        public string NOMBRE_MATERIAL { get { return _nombreMaterial; } set { _nombreMaterial = value; } }
        public string CLASE_AGMA { get { return _claseAgma; } set { _claseAgma = value; } }
        public string DESIGNACION_MATERIAL { get { return _designacionMaterial; } set { _designacionMaterial = value; } }
        public string TRATAMIENTO_MATERIAL { get { return _TratamientoMaterial; } set { _TratamientoMaterial = value; } }
    }
}
