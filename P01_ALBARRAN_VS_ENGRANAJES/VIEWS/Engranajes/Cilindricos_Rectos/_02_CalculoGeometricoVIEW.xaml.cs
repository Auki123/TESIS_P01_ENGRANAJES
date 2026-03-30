using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje;
using P01_ALBARRAN_VS_ENGRANAJES.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Lógica de interacción para _02_CalculoGoeometricoVIEW.xaml
    /// </summary>
    public partial class _02_CalculoGeometricoVIEW : UserControl
    {
        /* DTO_ResultGeometrico, es una clase que se utiliza para transferir datos de tipo geométrico a la siguiente
         * ventana user control, el cual es _03_calculo de cargas
        */
        private DTO_ResultGeometrico _resultadoG = new DTO_ResultGeometrico();
        private MostrarResultados mostrarResultados = new MostrarResultados();
        
        public _02_CalculoGeometricoVIEW()
        {
            InitializeComponent();
            mostrarResultados.mostrarResultadoGeometría(_resultadoG, ContenidoGeometricoDataGrid);

        }


       //........... Controla que los textBox solo permitan entradas numéricas

        private void ControlEntradaTexBox(object sender, TextCompositionEventArgs e)
        {
            ValidarEntradas ValidarTextbox = new ValidarEntradas();
            ValidarTextbox.controlEntradaTextBox(sender, e);
            
        }

        //........................Eventos de Acción

        private void CalcularGeometria(object sender, RoutedEventArgs e)
        {
            ValidarEntradas ValidarCalculo1 = new ValidarEntradas();

            if (ValidarCalculo1.validarEntradaNoNull(d_pBox, d_gBox, presure_angleBox) == true)
            {
                double valorDP = double.Parse(d_pBox.Text);
                double valorDG = double.Parse(d_gBox.Text);

                ComboBoxItem item = (ComboBoxItem)presure_angleBox.SelectedItem;
                int anguloPresion = int.Parse($"{item.Content}");

                CalcularEngranaje Calcular = new CalcularEngranaje(valorDG, valorDP, anguloPresion);

                _resultadoG = Calcular.calculGeometria();
                mostrarResultados.mostrarResultadoGeometría(_resultadoG, ContenidoGeometricoDataGrid);
            }
            else
            {
                MessageBox.Show("Faltan datos. Complete los campos requeridos", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // Evento de cambio de user control

        private void Continuar(object sender, RoutedEventArgs e)
        {
            if (_resultadoG.RELACIONCONTACTO != 0)
            {
                var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                _03_CalculoReaccionesVIEW SiguenteUserC = new _03_CalculoReaccionesVIEW(_resultadoG);

                if (ventanaPrincipal != null)
                {
                    ventanaPrincipal.windowsOperation.Content = SiguenteUserC;
                }
            }
            else
            {
                
                MessageBox.Show("No es posible continuar. No se ha realizado el cálculo, o el resultado no es válido.", "Acción no disponible", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new _01InicioEngranajesRectosVIEW();
            }
        }
    }
}
