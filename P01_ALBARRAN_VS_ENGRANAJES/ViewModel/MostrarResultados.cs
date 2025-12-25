using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using ScottPlot.WPF;

namespace P01_ALBARRAN_VS_ENGRANAJES.ViewModel
{
    class MostrarResultados
    {
        public void mostrarResultadoGeometría(DTO_ResultGeometrico resultado, DataGrid midatagrid)
        {
            
            var listaVisual = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Diámetro de paso Piñón", resultado.DP.ToString()),
                new KeyValuePair<string, string>("Diámetro de paso Corona", resultado.DG.ToString()),
                new KeyValuePair<string, string>("Ángulo de Presión", resultado.ANGULOPRESION.ToString()),
                new KeyValuePair<string, string>("N° Dientes Piñón", resultado.NP.ToString()),
                new KeyValuePair<string, string>("N° Dientes Corona", resultado.NG.ToString()),   
                new KeyValuePair<string, string>("Módulo métrico", resultado.MODULO.ToString()),
                new KeyValuePair<string, string>("Relación de Contacto", resultado.RELACIONCONTACTO.ToString()),
                new KeyValuePair<string, string>("Adendum", resultado.ADDENDUM.ToString()),
                new KeyValuePair<string, string>("Dedendum", resultado.DEDDENDUM.ToString())
            };

            midatagrid.ItemsSource = listaVisual;

        }

        public void mostrarResultadoCargas(DTO_ResultCargasYcinematica resultado, DataGrid midatagridPinon, DataGrid midatagridCorona)
        {

            var listaVisualPinon = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Velocidad angular (RMP)", resultado.VANGULARPINON.ToString()),
                new KeyValuePair<string, string>("Velocidad de paso (m/s)", resultado.VLINEAPASO.ToString()),
                new KeyValuePair<string, string>("Potencia (HP)", resultado.POTENCIAHP.ToString()),
                new KeyValuePair<string, string>("Torque (Nm)", resultado.TORQUEPINON.ToString()),
                new KeyValuePair<string, string>("Carga  Axial (N)", resultado.WAXIALPINON.ToString()),
                new KeyValuePair<string, string>("Carga Radial (N)", resultado.WRADIALPINON.ToString()),
            };
            var listaVisualCorona = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Velocidad angular (RMP)", resultado.VANGULARCORONA.ToString()),
                new KeyValuePair<string, string>("Velocidad de paso (m/s)", resultado.VLINEAPASO.ToString()),
                new KeyValuePair<string, string>("Potencia (HP)", resultado.POTENCIAHP.ToString()),
                new KeyValuePair<string, string>("Torque (Nm)", resultado.TORQUECORONA.ToString()),
                new KeyValuePair<string, string>("Carga  Axial (N)", resultado.WAXIALCORONA.ToString()),
                new KeyValuePair<string, string>("Carga Radial (N)", resultado.WRADIALCORONA.ToString()),
            };

            midatagridPinon.ItemsSource = listaVisualPinon;
            midatagridCorona.ItemsSource = listaVisualCorona;
        }


        // Esta función muestra el punto donde se encuentra el factor Kv, de la ventana de factores k.
        public void mostrarKV_Graficamete(double VelocidadPaso, double Kv,WpfPlot Grilla)
        {
            // Generare arreglos para utilizar un metodo que me grafique por puntos dados
            double inicio = 0;
            double inicioVertical=0.4;
            double pasohorizontal = 0.2;
            double pasoVertical = 0.01;

            if (Kv != 0)
            {
                // el tipo de enumerable "Enumerable.Range "
                // utilizado hace que genere vectores con un inicio, un valor final, y el paso es decir cada cuanto se genera el numero


                // Para Graficar la recta vertical Vp:
                var vectorY_VP = Enumerable.Range(0, (int)((Kv - inicioVertical) / pasoVertical) + 1)
                                   .Select(i => inicioVertical + i * pasoVertical)
                                   .ToArray();

                // Este tipo de Enumerable.Range, genera un valor repetido, para que contenga la misma cantidad de valores de un vector
                var repetidoX_VP = Enumerable.Repeat(VelocidadPaso, vectorY_VP.Length).ToArray();

                // muestra un plot(x,y)

                Grilla.Plot.Add.Scatter(repetidoX_VP, vectorY_VP);


                // Hago lo mismo para graficar la recta horizontal KV
                var vectorX_KV = Enumerable.Range(0, (int)((VelocidadPaso - inicio) / pasohorizontal) + 1)
                                   .Select(i => inicio + i * pasohorizontal)
                                   .ToArray();

                // Este tipo de Enumerable.Range, genera un valor repetido, para que contenga la misma cantidad de valores de un vector
                var repetidoY_KV = Enumerable.Repeat(Kv, vectorX_KV.Length).ToArray();

                // muestra un plot(x,y)

                Grilla.Plot.Add.Scatter(vectorX_KV, repetidoY_KV);
                Grilla.Refresh();
            }

            
        }

        public void MostrarResultadoDiseno(DTO_ResultadoDiseno Corona, DTO_ResultadoDiseno Pinon, DataGrid dataCorona, DataGrid dataPinon)
        {

            var DisenoPinon = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Esfuerzo: ", Pinon.SIGMAB.ToString()),
                //new KeyValuePair<string, string>("Ancho de cara: ", Pinon.anchocaraF.ToString()),
                new KeyValuePair<string, string>("FS:", Pinon.FactorSeguridad.ToString()),
                new KeyValuePair<string, string>("Resistencia de material:", Pinon.Sfb_prima.ToString()),
                new KeyValuePair<string, string>("Reistencia real:", Pinon.Sfb.ToString()),
            };
            var DisenoCorona = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Esfuerzo: ", Corona.SIGMAB.ToString()),
                //new KeyValuePair<string, string>("Ancho de cara: ", Corona.anchocaraF.ToString()),
                new KeyValuePair<string, string>("FS:", Corona.FactorSeguridad.ToString()),
                new KeyValuePair<string, string>("Resistencia de material:", Corona.Sfb_prima.ToString()),
                new KeyValuePair<string, string>("Reistencia real:", Corona.Sfb.ToString()),
            };

            dataPinon.ItemsSource = DisenoPinon;
            dataCorona.ItemsSource = DisenoCorona;

        }
    }
}
