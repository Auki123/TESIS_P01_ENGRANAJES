using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_ALBARRAN_VS_ENGRANAJES.RESOURCES
{
    public static class MathDeg
    {
        // Esta clase calcula trigonometría con entrada de ángulos en grados
        public static double Sin(double grados) =>
        Math.Sin(grados * Math.PI / 180.0);

        public static double Cos(double grados) =>
            Math.Cos(grados * Math.PI / 180.0);

        public static double Tan(double grados) =>
            Math.Tan(grados * Math.PI / 180.0);


        // Devuelve Y_ es decir el valor Y entre y0 _y_ y1 dado una x entre x0 y x1
        //.... Para los factores J: El numero de dientes será x, le valor asignado a ese numero de dientes será Y.
                // Los valores (x0,y0), representará a los valores menores del número de dientes
                // los valores (x1,y1), representará a los valores mayores del número de dientes
                // el valor x, es el valor real del número de dientes que se tiene, la salida será un Factor J asociado a ese número de dientes
        public static double InterpolacionLineal(int x0, double y0, int x1, double y1, int x)  // x está entre esos 2 puntos 
        {
            double y = y0 + (((y1 - y0) / (x1 - x0)) * (x - x0));
            
            return Math.Round(y,4);
        }
    }
}
