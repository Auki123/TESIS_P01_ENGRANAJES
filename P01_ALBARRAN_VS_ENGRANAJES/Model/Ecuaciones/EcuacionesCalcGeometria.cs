using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using P01_ALBARRAN_VS_ENGRANAJES.RESOURCES;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.Ecuaciones
{
    public static class EcuacionesCalcGeometria   //Para engranajes cilíndricos rectos
    {

        //Calcula el minimo numero de dientes en el piñon, dado los diametros primitivos del conjunto
        public static int CalcularMinNumerodientes(double d_pinon,double d_corona,double angulo_presion)
        {
            double m_G = d_corona / d_pinon;
            double numerador = 2;
            double denominador = (1 + 2 * m_G) * Math.Pow(MathDeg.Sin(angulo_presion), 2);
            double raiz = Math.Sqrt(Math.Pow(m_G, 2) + (1 + 2 * m_G) * Math.Pow(MathDeg.Sin(angulo_presion), 2));
            int resultado = (int)Math.Ceiling((numerador / denominador) * (m_G + raiz));
            return resultado;
        }


        //....................................Revisar

        public static double CalcularRelacionContacto(double moduloEstandar,double d_pinon, double d_corona, int angulo_presion)
        {
            double adendum = moduloEstandar;
            double C = (d_pinon + d_corona) / 2; // Distancia entre centros
            double rp = d_pinon / 2;
            double rg = d_corona / 2;
            double parte1 = Math.Sqrt((Math.Pow((rp + adendum), 2)) - Math.Pow((rp * MathDeg.Cos(angulo_presion)), 2));
            double parte2 = Math.Sqrt((Math.Pow((rg + adendum), 2)) - Math.Pow((rg * MathDeg.Cos(angulo_presion)), 2));
            double parte3 = C * MathDeg.Sin(angulo_presion);
            double Z_calculado = parte1 + parte2 - parte3;

            double m_p;
            m_p = Math.Round(Z_calculado / (moduloEstandar * Math.PI * MathDeg.Cos(angulo_presion)),4);
            return m_p; //Relacion de contacto

        }

        public static double calcularAdendum(double moduloEstandar) 
        {
            return moduloEstandar;       
        }

        public static double calcularDedendum(double moduloEstandar)
        {
            return 1.25 * moduloEstandar;
        }

    }
}
