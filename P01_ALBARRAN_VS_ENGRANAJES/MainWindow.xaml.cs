using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
using P01_ALBARRAN_VS_ENGRANAJES.Model;
using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.Model.PDF_Manager;
using P01_ALBARRAN_VS_ENGRANAJES.VIEWS;
using P01_ALBARRAN_VS_ENGRANAJES.VIEWS.Engranajes.Cilindricos_Rectos;
using P01_ALBARRAN_VS_ENGRANAJES.VIEWS.VentanasUI;
using System.Windows;
using PdfSharpCore.Pdf;

namespace P01_ALBARRAN_VS_ENGRANAJES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Objetos de Trnsferencia de datos, se crean directamente en el programa
        public DTO_ResultGeometrico R_GREOMETRICO { get; set; }
        public DTO_ResultCargasYcinematica R_CARGAS { get; set; }
        public DTO_ResultFactorK_Esf R_FactorK_Esf { get; set; }
        public DTO_ResultadoDiseno R_Diseno { get; set; }
             
        public MainWindow()
        {
            InitializeComponent();

            #region   Inicializar los objetos de transferencia de datos

            R_GREOMETRICO = new DTO_ResultGeometrico();
            R_CARGAS = new DTO_ResultCargasYcinematica();
            R_FactorK_Esf =  new DTO_ResultFactorK_Esf();
            R_Diseno =  new DTO_ResultadoDiseno();

            #endregion
 
            windowsOperation.Content = new VentanaInicialVIEWS();
        }


        private void ir_Click(object sender, RoutedEventArgs e)
        {
            if (windowsOperation.Content is not VentanaInicialVIEWS)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro que desea comenzar de nuevo?", "Advertencia",MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (resultado==MessageBoxResult.Yes) {

                    var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                    if (ventanaPrincipal != null)
                    {
                        ventanaPrincipal.windowsOperation.Content = new _01InicioEngranajesRectosVIEW();
                    }
                }
            }
            else 
            {
                var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                if (ventanaPrincipal != null)
                {
                    ventanaPrincipal.windowsOperation.Content = new _01InicioEngranajesRectosVIEW();
                }
            }
        }

        private void AbirAcercaDeView(object sender, RoutedEventArgs e)
        {
            var acercaDe = new AcercaDe_View();
            acercaDe.ShowDialog();
        }

        private void AbrirManualView(object sender, RoutedEventArgs e)
        {
            PdfManager ManualUsuario = new PdfManager();
            ManualUsuario.AbrirManualUsuario();
        }

        private void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            if (windowsOperation.Content is _05_DisenoFlexionView ventanaDiseño)
            {
                ventanaDiseño.GuardarResultado();

            }

            else if (windowsOperation.Content is VentanaInicialVIEWS)
            {
                MessageBox.Show("No se ha generado un trabajo. No es posible realizar el guardado.","Nada para guardar",MessageBoxButton.OK,MessageBoxImage.Error);
            }

            else
            {
                MessageBox.Show("El proceso de cálculo sigue incompleto, debe concluir con su trabajo. ", "Trabajo incompleto", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow nuevaVentana = new MainWindow();
            nuevaVentana.Show();
        }
    }

}
