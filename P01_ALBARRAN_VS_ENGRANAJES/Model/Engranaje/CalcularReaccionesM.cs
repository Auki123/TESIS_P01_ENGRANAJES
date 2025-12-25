using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.RESOURCES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje
{
    internal class CalcularReaccionesM
    {

        //...........Datos transportados a utilizar
        // ANGULO DE PRESIÓN
        // DIAMETRO DE PASO DE CORONA Y PIÑÓN
        // MÓDULO
        // NUMERO DE DIENTES DE PIÑÓN Y CORONA

        public DTO_ResultCargasYcinematica CalcularCargasyCinematica(DTO_ResultGeometrico resultadoG, double VangularPinonRPM, double PotenciaHP)
        {
            DTO_ResultCargasYcinematica resultCargasYcinematica = new DTO_ResultCargasYcinematica();
            if (VangularPinonRPM != 0 && PotenciaHP != 0)
            {
                int Np = resultadoG.NP;
                int Ng = resultadoG.NG;
                double DP = resultadoG.DP;
                double DG = resultadoG.DG;
                int anguloPresion = resultadoG.ANGULOPRESION;

                if (CalcularVlineaPaso(DP, VangularPinonRPM) <= 50.8)
                {
                    resultCargasYcinematica.POTENCIAHP = PotenciaHP;
                    resultCargasYcinematica.VANGULARPINON = VangularPinonRPM;
                    resultCargasYcinematica.VANGULARCORONA = CalcularWangularCorona(Np, Ng, VangularPinonRPM);
                    resultCargasYcinematica.VLINEAPASO = CalcularVlineaPaso(DP, VangularPinonRPM);

                    resultCargasYcinematica.TORQUEPINON = CalcularTorque(PotenciaHP, VangularPinonRPM);
                    resultCargasYcinematica.TORQUECORONA = CalcularTorque(PotenciaHP, resultCargasYcinematica.VANGULARCORONA);

                    resultCargasYcinematica.WAXIALPINON = CalcularCargaAxial(resultCargasYcinematica.TORQUEPINON, DP);
                    resultCargasYcinematica.WAXIALCORONA = CalcularCargaAxial(resultCargasYcinematica.TORQUECORONA, DG);

                    resultCargasYcinematica.WRADIALPINON = CalcularCargaRadial(resultCargasYcinematica.WAXIALPINON, anguloPresion);
                    resultCargasYcinematica.WRADIALCORONA = CalcularCargaRadial(resultCargasYcinematica.WAXIALCORONA, anguloPresion);
                }

                else
                {
                    MessageBox.Show("La velocidad en línea de paso es muy elevada \nNo debe superar 50.8 m/s");
                }
                
                
            }
            

            return resultCargasYcinematica;
        }



        //...........Métodos internos estáticos.

        private static double CalcularWangularCorona(int Np, int Ng, double Wangularpinon_RPM) => Math.Round(((Wangularpinon_RPM * Np) / Ng),2); //RPM
        private static double CalcularVlineaPaso(double dp_mm, double WangularPinon_RPM) => Math.Round(((2*Math.PI*WangularPinon_RPM*dp_mm)/(120000)),3); //....m/s
        private static double CalcularTorque(double potenciaHP, double WangularRPM) => Math.Round((7120.91*potenciaHP/WangularRPM), 2); //Nm
        private static double CalcularCargaAxial(double Torque_Nm, double dpaso_mm) => Math.Round(2000 * Torque_Nm / dpaso_mm,2); //..N
        private static double CalcularCargaRadial(double CargaAxial_N, int anguloPresion) => Math.Round((CargaAxial_N*MathDeg.Tan(anguloPresion)), 2); //..N

    }
}
