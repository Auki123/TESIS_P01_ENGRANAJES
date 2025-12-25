using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects
{
    public class DTO_ResultCargasYcinematica
    {
        // .................Almacena de user control de cálculo de reacciones

        private double _VangularPinon; //....RPM
        private double _VangularCorona;//....RPM
        private double _VlineaPaso; //.. m/s
        private double _potenciaHP; //....HP
        private double _torqueCorona; //....N.m
        private double _torquePinon;  //....N.m
        private double _WradialPinon; //....kN
        private double _WradialCorona; //...kN
        private double _WaxialPinon; //...kN
        private double _WaxialCorona;  //...kN

        public double VANGULARPINON { get { return _VangularPinon; } set { _VangularPinon = value; } }
        public double VANGULARCORONA { get { return _VangularCorona; } set { _VangularCorona = value; } }
        public double VLINEAPASO { get { return _VlineaPaso; } set { _VlineaPaso = value; } }
        public double POTENCIAHP { get { return _potenciaHP; } set { _potenciaHP = value; } }
        public double TORQUECORONA { get { return _torqueCorona; } set { _torqueCorona = value; } }
        public double TORQUEPINON { get { return _torquePinon; } set { _torquePinon = value; } }
        public double WRADIALPINON { get { return _WradialPinon; } set { _WradialPinon = value; } }
        public double WRADIALCORONA { get { return _WradialCorona; } set { _WradialCorona = value; } }
        public double WAXIALPINON { get { return _WaxialPinon; } set { _WaxialPinon = value; } }
        public double WAXIALCORONA { get { return _WaxialCorona; } set { _WaxialCorona = value; } }
    }
}
