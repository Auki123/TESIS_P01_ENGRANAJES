using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje
{
    internal class CalcularEsfuerzosM
    {

        private readonly DTO_ResultCargasYcinematica _ResultCargas;
        private readonly DTO_ResultFactorK_Esf _FactoresK;
        private readonly DTO_ResultGeometrico _RGeometrico;
        public CalcularEsfuerzosM(DTO_ResultCargasYcinematica cargas, DTO_ResultFactorK_Esf Kfactor, DTO_ResultGeometrico DatoG)
        {
            _ResultCargas = cargas;
            _FactoresK = Kfactor;
            _RGeometrico = DatoG;
        }
        public double CalcularSigmabCorona(double anchoCaraF)  // Calcula los sefuerzos que se producen en la corona
        {
            double sigmaBnumerador = _ResultCargas.WAXIALCORONA * _FactoresK.KA_FACTOR * _FactoresK.KM_FACTOR*_FactoresK.KS*_FactoresK.KBg_FACTOR*_FactoresK.KI;
            double sigmaBdenominador = anchoCaraF * _RGeometrico.MODULO * _FactoresK.J_FACTORCORONA * _FactoresK.KV_FACTOR;
            double sigmaB = 0;
            if (sigmaBdenominador != 0) 
            {
                sigmaB=sigmaBnumerador/sigmaBdenominador;
            }
            return Math.Round(sigmaB,3);
        
        } 

        public double CalcularSigmabPinon(double anchoCaraF)  // Calcula los efuerzos que se producen en piñón
        {
            double sigmaBnumerador = _ResultCargas.WAXIALPINON * _FactoresK.KA_FACTOR * _FactoresK.KM_FACTOR * _FactoresK.KS * _FactoresK.KBp_FACTOR * _FactoresK.KI;
            double sigmaBdenominador = anchoCaraF * _RGeometrico.MODULO * _FactoresK.J_FACTORPINON * _FactoresK.KV_FACTOR;
            double sigmaB = 0;
            if (sigmaBdenominador != 0)
            {
                sigmaB = sigmaBnumerador / sigmaBdenominador;
            }
            return Math.Round(sigmaB, 3);
        }

        public double CalcularResistenciaRealMaterial(double Sfb_prima)  // Calcula el valor real Sfb de la resistencia del material
        {
            double Sfb = 0;
            Sfb=((Sfb_prima*_FactoresK.KL)/(_FactoresK.KT_FACTOR*_FactoresK.KR_FACTOR));

            return Math.Round(Sfb,3);
        }

        public double calcularFactorSeguridad(double SigmaB, double Sfb)
        {
            return Math.Round(Sfb/SigmaB, 3);
        }


    }
}
