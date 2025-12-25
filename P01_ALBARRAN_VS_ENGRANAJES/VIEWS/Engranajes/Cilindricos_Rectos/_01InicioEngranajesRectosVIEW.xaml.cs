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
    /// Lógica de interacción para _01InicioEngranajesRectosVIEW.xaml
    /// </summary>
    public partial class _01InicioEngranajesRectosVIEW : UserControl
    {
        public _01InicioEngranajesRectosVIEW()
        {
            InitializeComponent();
        }

        private void CONTINUAR___I_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new _02_CalculoGeometricoVIEW();
            }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new VentanaInicialVIEWS();
            }
        }
    }
}
