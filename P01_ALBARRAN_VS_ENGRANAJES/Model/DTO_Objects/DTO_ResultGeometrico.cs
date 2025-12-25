using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects
{
    public sealed class DTO_ResultGeometrico
    {
        //Transferencia de datos obtenidos mediante cálculos en diferentes user controls

        //............................Almacena de userControl de cálculo geométrico.
        private double _modulo;  // módulo métrico
        private double _addendum;
        private double _dedendum;
        private double _RelacionContacto_m_p;
        private int _angulopresion;
        private double _dp;   // diámetro de paso de piñón
        private int _Np;      // número de dientes en el piñón
        private int _Ng;      // número de dientes en la corona
        private double _dg;   // diámetro de paso en la corona

        // Añadidos recientes
        private double _m_G;    // razón de engrane
        private double _distanciaCentros;   // distancia entre centros del piñón y corona
        private double _h_t;                      // Profundidad del diente


        // POR SI ACASO UTILIZADO EN RESULTADOS
        private double _pasocircular;
        private double _deg;  // Diámetro exterior de corona
        private double _dig;  // Diámetro de raíz de corona
        private double _dhg;  // Diámetro de eje de corona
        private double _dep;  // diámetro exterior de piñón
        private double _dip;  // diámetro de raíz de piñón
        private double _dhp;  // diámetro de eje de piñón
        private double _anchocaraF;
        // 
        private double _holgura;
        private double _espesorDiente;   // ESPESOR DEL DIENTE, MEDIDO DESDE EL CÍRCULO DE PASO
        private double _anchoEspacio;    // ESPACIO ENTRE CADA DIENTE, MEDIDO DESDE EL CIRCULO DE PASO

        // Propiedades

        #region Principales
        public double MODULO {get {return _modulo; } set { _modulo = value;} }
        public double ADDENDUM
        {
            get { return _addendum; }
            set { _addendum = value; }
        }
        public double DEDDENDUM
        {
            get { return _dedendum; }
            set { _dedendum = value; }
        }
        public double RELACIONCONTACTO
        {
            get { return _RelacionContacto_m_p; }
            set { _RelacionContacto_m_p = value; }
        }
        public int ANGULOPRESION
        {
            get { return _angulopresion; }
            set { _angulopresion = value; }
        }
        public double DP
        {
            get { return _dp; }
            set { _dp = value; }
        }
        public double DG
        {
            get { return _dg; }
            set { _dg = value; }
        }
        public int NP
        {
            get { return _Np; }
            set { _Np = value; }
        }
        public int NG
        {
            get { return _Ng; }
            set { _Ng = value; }
        }

        #endregion

        public double M_G { get { return _m_G; } set { _m_G = value; } }
        public double DISTANCIA_CENTROS { get { return _distanciaCentros; } set { _distanciaCentros = value; } }
        public double H_T { get { return _h_t; } set { _h_t = value; } }

        #region  POR SI ACASO
        public double PASOCIRCULAR { get { return _pasocircular; } set { _pasocircular = value; } }
        public double HOLGURA { get { return _holgura; } set { _holgura = value; } }
        public double ESPESORDIENTE { get { return _espesorDiente; } set { _espesorDiente = value; } }
        public double ANCHOESPACIO { get { return _anchoEspacio; } set { _anchoEspacio = value; } }

        #endregion
        public double DEg { get { return _deg; } set { _deg = value; } }
        public double DIg { get { return _dig; } set { _dig = value; } }
        public double DHg { get { return _dhg; } set { _dhg = value; } }
        public double DEp { get { return _dep; } set { _dep = value; } }
        public double DIp { get { return _dip; } set { _dip = value; } }
        public double DHp { get { return _dhp; } set { _dhp = value; } }
        public double anchocaraF { get { return _anchocaraF; } set { _anchocaraF = value; } }

    }
}
