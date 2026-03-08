using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects
{
    public class DTO_ResultFactorK_Esf
    {
        //Factores de esfuerzo

        private int _ki = 1;      
        private double _ks = 1.25;

        private double _J_factorPinon;
        private double _J_factorCorona;
        private double _kv_factor;
        private double _ka_factor;
        private double _kbp_factor;
        private double _kbg_factor;
        
        private double _km_factor; //único en inicializarse en la ventana _05_

        // Factores de resistencia de material

        private double _kl=1;  // Solo para E7 ciclos
        private double _kt_factor;
        private double _kr_factor;

        public int KI { get { return _ki; } }
        public double KS { get { return _ks; } }
        public double KL { get { return _kl; } }

        public double J_FACTORPINON { get { return _J_factorPinon; }   set { _J_factorPinon = value; } }
        public double J_FACTORCORONA { get { return _J_factorCorona; } set { _J_factorCorona = value; } }
        public double KV_FACTOR { get { return _kv_factor; } set { _kv_factor = value; } } ////
        public double KA_FACTOR { get { return _ka_factor; } set { _ka_factor = value; } }
        public double KBp_FACTOR { get { return _kbp_factor; } set { _kbp_factor = value; } }
        public double KBg_FACTOR { get { return _kbg_factor; } set { _kbg_factor = value; } }
        public double KM_FACTOR { get { return _km_factor; } set { _km_factor = value; } }
        public double KT_FACTOR { get { return _kt_factor; } set { _kt_factor = value; } }
        public double KR_FACTOR { get { return _kr_factor; } set { _kr_factor = value; } }

        //Verifica si algun valor de los factores k de esfuerzo, aun no ha sido calculado devolviendo 0 si es el caso
        public bool VeificaDatoCompleto() 
        { 
            double Valor0 = _J_factorCorona * _J_factorPinon *_kv_factor* _ka_factor * KS * KI * _kbp_factor*_kbg_factor;
            if (Valor0 == 0) return false;
            else return true;
        }

        //Verifica si algun valor de los factores k de resistencia, aun no ha sido calculado devolviendo 0 si es el caso
        public bool VeificaCompletoKresist()
        {
            double Valor0 = KL * _kt_factor * _kr_factor;
            if (Valor0 == 0) return false;
            else return true;
        }
    }
}
