using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.RESOURCES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje
{
    class CalcularEngranaje
    {
        private readonly double dg; //Diametro de paso de corona
        private readonly double dp; // Diametro de paso de piñón
        private readonly int anguloPresion;  //Angulo de presión
        private readonly BaseDatos db = new BaseDatos();


        private int localNp = 0;
        private int localNg=0;
        private double localmodulo=0;
        private double local_m_p=0;
        private double localadendum=0;
        private double localdedendum=0;

        private double local_mG = 0;
        private double localDistanciaCentros=0;
        private double local_ht=0;
        private double localPasocircular = 0;

        private double local_deg = 0;
        private double local_dig = 0;

        private double local_dep = 0;
        private double local_dip = 0;



        public CalcularEngranaje(double dg, double dp, int anguloPresion)
        {
            this.dg = dg;
            this.dp = dp;
            this.anguloPresion = anguloPresion;
        }
        
        public DTO_ResultGeometrico calculGeometria()
        {

            DTO_ResultGeometrico resultadoR = new DTO_ResultGeometrico();

            List<double> moduloGeometrico = new List<double>();
            moduloGeometrico = db.ObtenerModulo();

            /// Comenzar por aqui
            if (dg != 0 && dp != 0)
            {
                local_mG = dg / dp;

                if (local_mG >= 1)
                {
                    List<int> posiblesValoresNp = new List<int>();
                    double posibleNp;
                    double posibleNg;
                    double posible_m_p;

                    // Define el número mínimo de dientes en el piñón posibilitados por el factor J.
                    double FactorJminNp;
                    if (anguloPresion == 20)
                    { FactorJminNp = 21; }
                    else if (anguloPresion == 25) { FactorJminNp = 14; }
                    else { FactorJminNp = 0; }
                    //....................................................................

                    int numMinDientespinon = CalcularMinNumerodientes(dp, dg, anguloPresion); // Calculo el número mínimo de dientes posibles en piñon

                    for (int i =0; i <moduloGeometrico.Count; i++)
                    {
                        posibleNp = dp / moduloGeometrico[i];
                        posibleNg = dg / moduloGeometrico[i];
                        posible_m_p = CalcularRelacionContacto(moduloGeometrico[i], dp, dg, anguloPresion);
                        if (posibleNp % 1 == 0 && posibleNg % 1 == 0 && posible_m_p >= 1.4 && posibleNp>=numMinDientespinon
                            &&posibleNg<=135&& posibleNp>=FactorJminNp)
                        {
                            posiblesValoresNp.Add((int)posibleNp);
                        }
                    }

                    

                    if (posiblesValoresNp.Count != 0)
                    {
                        List <int> posibleNpReal = new List<int>();

                        for (int i = 0; i < posiblesValoresNp.Count; i++)
                        {
                            localNp = posiblesValoresNp[i];
                            localmodulo = dp / localNp;
                            localNg = (int)(dg / localmodulo);

                            if ((localNp > 14 && localNp < 17 && localNg > 14 && localNg < 17) ||
                                (localNp > 17 && localNp < 21 && localNg > 17 && localNg < 21) ||
                                (localNp > 21 && localNp < 26 && localNg > 21 && localNg < 26) ||
                                (localNp > 26 && localNp < 35 && localNg > 26 && localNg < 35) ||
                                (localNp > 35 && localNp < 55 && localNg > 35 && localNg < 55) ||
                                (localNp > 55 && localNp < 135 && localNg > 55 && localNg < 135))
                            {
                                // No hace nada
                            }
                            else
                            {
                                posibleNpReal.Add(localNp);
                            }

                        }

                        if (posibleNpReal.Count != 0)

                        {

                            localNp = posibleNpReal.Min();
                            localmodulo = dp / localNp;
                            localNg = (int)(dg / localmodulo);
                            local_m_p = CalcularRelacionContacto(localmodulo, dp, dg, anguloPresion);
                            localadendum = calcularAdendum(localmodulo);
                            localdedendum = calcularDedendum(localmodulo);

                            localDistanciaCentros = (dp + dg) / 2;
                            local_ht = localadendum + localdedendum;
                            localPasocircular = Math.Round(Math.PI * localmodulo, 3);

                            local_deg = dg + 2 * localadendum;
                            local_dig = dg - 2 * localdedendum;
                            local_dep = dp + 2 * localadendum;
                            local_dip = dp - 2 * localdedendum;
                        }
                        else
                        
                        {
                            localNp = posiblesValoresNp.Min();
                            localmodulo = dp / localNp;
                            localNg = (int)(dg / localmodulo);
                            
                            MessageBox.Show("Delimitado por la tabla de factor geométrico: " + "Np: " + localNp + ". Ng: " + localNg, "Resultado no factible", MessageBoxButton.OK, MessageBoxImage.Error);

                        }

                    }
                    else
                    {
                        MessageBox.Show("No existe solución a la forma geométrica.", "Resultado no factible", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("El diámetro del piñón no puede ser mayor que la coróna.", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Los datos no pueden tener valor cero. Ingrese valores válidos.", "Datos inválidos", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            resultadoR.DP = dp;
            resultadoR.DG = dg;
            resultadoR.ANGULOPRESION = anguloPresion;
            resultadoR.ADDENDUM = localadendum;
            resultadoR.DEDDENDUM = localdedendum;
            resultadoR.MODULO = localmodulo;
            resultadoR.NG = localNg;
            resultadoR.NP = localNp;
            resultadoR.RELACIONCONTACTO = local_m_p;

            resultadoR.M_G = local_mG;
            resultadoR.DISTANCIA_CENTROS = localDistanciaCentros;
            resultadoR.H_T = local_ht;
            resultadoR.PASOCIRCULAR = localPasocircular;
            resultadoR.DEg = local_deg;
            resultadoR.DIg = local_dig;
            resultadoR.DEp = local_dep;
            resultadoR.DIp = local_dip;
            return resultadoR;

        }


        #region  Métodos estáticos solo para esta clase
        //Calcula el minimo numero de dientes en el piñon, dado los diametros primitivos del conjunto
        private static int CalcularMinNumerodientes(double d_pinon, double d_corona, double angulo_presion)
        {
            double m_G = d_corona / d_pinon;
            double numerador = 2;
            double denominador = (1 + 2 * m_G) * Math.Pow(MathDeg.Sin(angulo_presion), 2);
            double raiz = Math.Sqrt(Math.Pow(m_G, 2) + (1 + 2 * m_G) * Math.Pow(MathDeg.Sin(angulo_presion), 2));
            int resultado = (int)Math.Ceiling((numerador / denominador) * (m_G + raiz));
            return resultado;
        }

        private static double CalcularRelacionContacto(double moduloEstandar, double d_pinon, double d_corona, int angulo_presion)
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
            m_p = Math.Round(Z_calculado / (moduloEstandar * Math.PI * MathDeg.Cos(angulo_presion)), 4);
            return m_p; //Relacion de contacto

        }

        private static double calcularAdendum(double moduloEstandar)
        {
            return moduloEstandar;
        }

        private static double calcularDedendum(double moduloEstandar)
        {
            return 1.25 * moduloEstandar;
        }


        #endregion

    }
}
