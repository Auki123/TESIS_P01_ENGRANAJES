using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje;
using P01_ALBARRAN_VS_ENGRANAJES.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P01_ALBARRAN_VS_ENGRANAJES.VIEWS.Engranajes.Cilindricos_Rectos
{
    /// <summary>
    /// Lógica de interacción para _03_CalculoReaccionesVIEW.xaml
    /// </summary>
    public partial class _03_CalculoReaccionesVIEW : UserControl
    {

        // Objeto de transferencia de datos solo de lectura despues del cálculo geométrico
        private DTO_ResultGeometrico _DatoGeometrico;

        //Objeto de transferencia de datos, de las cargas calculadas, aqui se llenan los valores
        private DTO_ResultCargasYcinematica _ResultadoCalcularCarga = new DTO_ResultCargasYcinematica();
        public _03_CalculoReaccionesVIEW(DTO_ResultGeometrico Dato_Geometrico)
        {
            InitializeComponent();
            _DatoGeometrico= Dato_Geometrico;

        }

        private void ControlEntradaTexBox(object sender, TextCompositionEventArgs e)
        {
            ValidarEntradas ValidarTextbox = new ValidarEntradas();
            ValidarTextbox.controlEntradaTextBox(sender, e);
        }


        // .........Eventos de acción

        private void CalcularParámetrosDinámicos(object sender, RoutedEventArgs e)
        {
            ValidarEntradas validarNoNull = new ValidarEntradas();
            if (validarNoNull.validarEntradaNoNull(EntradaPotencia,EntradaVangular) == true)
            {
                double Potencia = double.Parse(EntradaPotencia.Text);
                double Vangular= double.Parse(EntradaVangular.Text);

                CalcularReaccionesM reaccionesM = new CalcularReaccionesM();
                _ResultadoCalcularCarga = reaccionesM.CalcularCargasyCinematica(_DatoGeometrico,Vangular,Potencia);

                MostrarResultados MostrarCargasCalc = new MostrarResultados();
                MostrarCargasCalc.mostrarResultadoCargas(_ResultadoCalcularCarga,MostrarCargasEnPinon,MostrarCargasEnCorona);

            }
            
        }


        //Evento de cambio de user control
        private void ContinuarAFactoresK(object sender, RoutedEventArgs e)
        {
            if (_ResultadoCalcularCarga.TORQUECORONA != 0)
            {
                var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                _04_FactoresK_EsfuerzoVIEW userControl_KFACTOR = new _04_FactoresK_EsfuerzoVIEW(_DatoGeometrico, _ResultadoCalcularCarga);

                if (ventanaPrincipal != null)
                {
                    ventanaPrincipal.windowsOperation.Content = userControl_KFACTOR;
                }
            }
            
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new _02_CalculoGeometricoVIEW();
            }
        }
    }
}
