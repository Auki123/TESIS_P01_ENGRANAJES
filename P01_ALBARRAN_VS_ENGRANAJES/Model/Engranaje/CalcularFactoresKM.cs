using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
using ScottPlot.Interactivity.UserActionResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje
{
    public class CalcularFactoresKM
    {
        private readonly BaseDatos MiBaseDatos = new BaseDatos();

        #region Factores de la AGMA de esfuerzos

        // Factor J
        public (double factorJpinon, double factorJcorona) DameFactorJ(int Np, int Ng, int anguloPresion)
        {
            double factorJpinon = 0;
            double factorJcorona = 0;
            (factorJpinon, factorJcorona) = MiBaseDatos.ObtenerFactoresJ(Np,Ng,anguloPresion);
            return (factorJpinon,factorJcorona);
        }


        // Devuelve el valor de kv
        public double CalcularKv(double velocidadPaso, int IcalidadQv)
        {
            double KvFactorCalculated = 0;
            double LocalCalidadQv = 0;

            if (IcalidadQv >= 6 && IcalidadQv <= 14)
            {
                LocalCalidadQv = IcalidadQv;
            }
            else if (IcalidadQv >= 3 && IcalidadQv <= 5)
            {
                LocalCalidadQv = 4;
            }
            else
            { 
                MessageBox.Show("Valor Inconsistente de Qv.", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            if ((LocalCalidadQv >= 4) && (LocalCalidadQv <= 14))
            {
                if (velocidadPaso <= 50.8)
                {
                    if ((velocidadPaso <= 13 && LocalCalidadQv == 4) || (velocidadPaso <= 19.8 && LocalCalidadQv == 6) ||
                        (velocidadPaso <= 23.8 && LocalCalidadQv == 7) || (velocidadPaso <= 28.8 && LocalCalidadQv == 8) ||
                        (velocidadPaso <= 34.4 && LocalCalidadQv == 9) || (velocidadPaso <= 41.2 && LocalCalidadQv == 10) ||
                        (velocidadPaso <= 50.8 && LocalCalidadQv == 11))
                    {
                        double B = Math.Pow(12 - LocalCalidadQv, 2.0 / 3.0) / 4.0;
                        double A = 50 + 56 * (1 - B);
                        KvFactorCalculated = Math.Pow(A / (A + Math.Sqrt(200 * velocidadPaso)), B);
                    }
                    else if (LocalCalidadQv > 11 && LocalCalidadQv <= 14) { KvFactorCalculated = 0.95; }
                    else
                    {
                        KvFactorCalculated = 0;
                        MessageBox.Show("El engranaje no puede desenvolverse en este entorno con la velocidad de operación dispuesta. \n   La velocidad supera a la recomendada para cada calidad Qv.", "Resultado no factible", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Velocidad en línea de paso fuera de rango.", "Resultado no factible", MessageBoxButton.OK, MessageBoxImage.Error);
                }       

            }
            return Math.Round(KvFactorCalculated, 3);
        }


        // Calicula KB, tanto de piñ+on como de corona, se necesita profundidad de diente
                              // diámetro de raíz de piñón dpi
                              // diámetro de raiz de corona dgi
                              // diámetro de eje de piñón dhp
                              // diámetro de eje de corona dhg. devuelve espesores de aro y factor KB de ambos elementos
        public (double tR_corona, double tR_pinon, double kB_corona, double kB_pinon) CalcularKB(double ht, double dpi, double dgi, double dhp, double dhg)
        {
            double tR_corona = 0;
            double tR_pinon = 0;
            double kB_pinon = 0;
            double kB_corona = 0;

            if (dhp != 0 && dhg != 0)
            {
                if (dhp < dpi && dhg < dgi)
                {
                    tR_corona = (dgi - dhg) / 2;
                    tR_pinon = (dpi - dhp) / 2;

                    double mb_corona = tR_corona / ht;
                    if (mb_corona >= 0.5 && mb_corona <= 1.2)
                    {
                        kB_corona = Math.Round(-2 * mb_corona + 3.4, 3);
                    }
                    else if (mb_corona > 1.2)
                    { kB_corona = 1; }
                    

                    double mb_pinon = tR_pinon / ht;
                    if (mb_pinon >= 0.5 && mb_pinon <= 1.2)
                    {
                        kB_pinon = Math.Round(-2 * mb_pinon + 3.4, 3);
                    }
                    else if (mb_pinon > 1.2)
                    { kB_pinon = 1; }


                    // Se compara todo, dado que ya se ha condicionado antes, si da un resultado de 0 significa que el espesor de aro es muy pequeño y no sirve
                    if (kB_corona == 0 || kB_pinon == 0)
                    {
                        MessageBox.Show("El espesor de aro de uno o ambos elementos es muy pequeño.", "Resultado no factible", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }

                else
                {
                    MessageBox.Show("El diámetro de eje de los elementos supera al diámetro de raíz", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Los datos no pueden tener valor cero. Ingrese valores válidos.", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Error);

            }



            return (tR_corona,tR_pinon,kB_corona,kB_pinon);
        
        }

        // Calcula Factor km de distribución de carga
        public double CalcularKm(double F)
        {
            double Km = 0;
            if (F > 0 && F < 50) { Km = 1.6; }
            else if (F >= 50 && F <= 150) { Km = 1.7; }
            else if (F > 150 && F < 500) { Km = 1.8; }
            else if (F >= 500) { Km = 2; }
            return Km;

        }

        // Factor Ka se obtiene directo de la base de datos
        // Factor ks y Ki ya tiene asignado un valor

        #endregion

        #region Factores de la AGMA, de resistencia de material
        public double CalcularKT(double TemperaturaCelcius)
        {
            // definida solo para los aceros
            double KT = 0;
            if (TemperaturaCelcius >= 0 && TemperaturaCelcius <= 121.111)
            { KT = 1;}
            else if (TemperaturaCelcius > 121.111)
            { KT = ((492 + 1.8 * TemperaturaCelcius) / (620)); }
            else {  KT = 0;}
            return Math.Round((double)KT,3);
        }

        // Factor KR se realiza directo
        // Factor KL, establecido para e7 ciclos y es 1
        #endregion

    }
}
