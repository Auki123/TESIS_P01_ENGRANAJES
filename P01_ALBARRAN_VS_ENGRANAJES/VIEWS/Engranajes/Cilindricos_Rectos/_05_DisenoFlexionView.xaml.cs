using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje;
using P01_ALBARRAN_VS_ENGRANAJES.Model.PDF_Manager;
using P01_ALBARRAN_VS_ENGRANAJES.ViewModel;
using PdfSharpCore.Pdf;
using ScottPlot.LegendLayouts;
using ScottPlot.TickGenerators.Financial;
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
    /// Lógica de interacción para _05_DisenoAresistenciaFlexionView.xaml
    /// </summary>
    public partial class _05_DisenoFlexionView : UserControl
    {
        private readonly BaseDatos materiales = new BaseDatos();
        private List<MaterialPropertyVM> datosMateriales;



        private readonly DTO_ResultGeometrico _datoGeometria;
        private readonly DTO_ResultCargasYcinematica _datosCarga;
        private DTO_ResultFactorK_Esf _factoresK;

        private DTO_ResultadoDiseno _ResCorona = new DTO_ResultadoDiseno();
        private DTO_ResultadoDiseno _ResPinon= new DTO_ResultadoDiseno();

        public _05_DisenoFlexionView(DTO_ResultGeometrico DatoG, DTO_ResultCargasYcinematica DatoC, DTO_ResultFactorK_Esf DatoEsf)
        {
            InitializeComponent();
            _datoGeometria = DatoG;
            _datosCarga= DatoC;
            _factoresK = DatoEsf;

            InicializarTextoAnchoCara();
            // inicializa con un data grid con tabla de materiales
            datosMateriales = materiales.GetMaterialProperties();
            ListaMateriales.ItemsSource = datosMateriales;

            // muestra los cuadros con valores nulos, solo para observar antes del cálculo
            MostrarResultados mostrarDiseno = new MostrarResultados();
            mostrarDiseno.MostrarResultadoDiseno(_ResCorona, _ResPinon, MostrarCorona, MostrarPinon);
        }

        #region métodos de clase
        private void InicializarTextoAnchoCara()
        {
            double AnchoCaraMin = 8 * _datoGeometria.MODULO;
            double AnchoCaraMax = 16 * _datoGeometria.MODULO;

            AnchoDeCaraSugerido.Text = "Se sugiere un valor de entre "+AnchoCaraMin.ToString()+
            " y "+ AnchoCaraMax.ToString()+ " mm tomando en cuenta el diseño actual:  ";
        }
        public void GuardarResultado()
        {
            PdfSharpDocs Plantilla = new PdfSharpDocs(_datoGeometria, _datosCarga, _factoresK, _ResPinon, _ResCorona);
            PdfManager miPdfManager = new PdfManager();

            PdfDocument documentoPdF = new PdfDocument();
            documentoPdF = Plantilla.PlantillaResultados();

            miPdfManager.GuardarYMostrarPdf(documentoPdF);
        }
        #endregion

        #region eventos

        private void FiltrarEntradaNumerica(object sender, TextCompositionEventArgs e)
        {
            ValidarEntradas ValidarTextbox = new ValidarEntradas();
            ValidarTextbox.controlEntradaTextBox(sender, e);
        }

        // eventos de cálculo
        private void SeleccionaMateriales(object sender, SelectionChangedEventArgs e)
        {
            if (ListaMateriales.SelectedItem is MaterialPropertyVM seleccion)
            {
                // Acceso directo a cada propiedad
                //int id = seleccion.IdPropiedad;
                _ResPinon.NOMBRE_MATERIAL = seleccion.NombreMaterial;
                _ResCorona.NOMBRE_MATERIAL = seleccion.NombreMaterial;

                _ResPinon.CLASE_AGMA = seleccion.ClaseAgma;
                _ResCorona.CLASE_AGMA = seleccion.ClaseAgma;

                _ResPinon.DESIGNACION_MATERIAL = seleccion.Designacion;
                _ResCorona.DESIGNACION_MATERIAL=seleccion.Designacion;

                _ResPinon.TRATAMIENTO_MATERIAL = seleccion.Tratamiento;
                _ResCorona.TRATAMIENTO_MATERIAL = seleccion.Tratamiento;

                double resistenciaMin = seleccion.ResistenciaMin_MPa;
                double resistenciaMax = seleccion.ResistenciaMax_MPa;

                if (resistenciaMax == 0)
                {
                    SeleccionMaterialText.Text = "El valor de la resistencia del material es único. ";
                    ResistenciaAGMA.IsEnabled = false;
                    ResistenciaAGMA.Text = resistenciaMin.ToString();
                }
                else 
                {
                    SeleccionMaterialText.Text = "El valor de la resistencia del material debe estar entre "+resistenciaMin.ToString()
                    + " y "+resistenciaMax.ToString()+ " MPa.";
                    ResistenciaAGMA.IsEnabled = true;
                    ResistenciaAGMA.Text = null;
                }
            }
        }

        private void CalcularKT(object sender, RoutedEventArgs e)
        {
            CalcularFactoresKM factoresKM = new CalcularFactoresKM();
            ValidarEntradas validarEntradas = new ValidarEntradas();

            if (validarEntradas.validarEntradaNoNull(KT_Temperaturavalue) == true)
            {
                double temperaturaCelcius = double.Parse(KT_Temperaturavalue.Text);
                _factoresK.KT_FACTOR = factoresKM.CalcularKT(temperaturaCelcius);
                KT_value.Text = _factoresK.KT_FACTOR.ToString();
            }
            else
            {
                MessageBox.Show("Faltan datos. Complete los campos requeridos", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void KR_SELECTION(object sender, SelectionChangedEventArgs e)
        {
            double KR;
            if (ValoresKR.SelectedItem != null)
            {
                if (ValoresKR.SelectedItem == value90) { KR = 0.85; }
                else if (ValoresKR.SelectedItem == value99) { KR = 1.0; }
                else if (ValoresKR.SelectedItem == value99_9) { KR = 1.25; }
                else if (ValoresKR.SelectedItem == value99_99) { KR = 1.5; }
                else { KR = 0; }
                _factoresK.KR_FACTOR = KR;
                KR_value.Text = KR.ToString();
            }
        }

        private void CalcularFS(object sender, RoutedEventArgs e)
        {
            ValidarEntradas validarTexbox = new ValidarEntradas();
            bool datokresistcompleto = _factoresK.VeificaCompletoKresist();
            if ((validarTexbox.validarEntradaNoNull(AnchoCaraF, ResistenciaAGMA) == true) && (datokresistcompleto == true))
            {
                double F = double.Parse(AnchoCaraF.Text);
                _datoGeometria.anchocaraF = F;

                double sfb_prima = double.Parse(ResistenciaAGMA.Text);
                _ResCorona.Sfb_prima = sfb_prima;
                _ResPinon.Sfb_prima = sfb_prima;

                CalcularFactoresKM calcularkm = new CalcularFactoresKM();
                _factoresK.KM_FACTOR = calcularkm.CalcularKm(F);
                ValorKm_box.Text = _factoresK.KM_FACTOR.ToString();

                CalcularEsfuerzosM calcularEsfuerzos = new CalcularEsfuerzosM(_datosCarga, _factoresK, _datoGeometria);
                _ResCorona.SIGMAB = calcularEsfuerzos.CalcularSigmabCorona(F);
                _ResPinon.SIGMAB = calcularEsfuerzos.CalcularSigmabPinon(F);

                double Sfb_real = calcularEsfuerzos.CalcularResistenciaRealMaterial(sfb_prima);
                _ResCorona.Sfb = Sfb_real;
                _ResPinon.Sfb = Sfb_real;

                _ResCorona.FactorSeguridad = calcularEsfuerzos.calcularFactorSeguridad(_ResCorona.SIGMAB, Sfb_real);
                _ResPinon.FactorSeguridad = calcularEsfuerzos.calcularFactorSeguridad(_ResPinon.SIGMAB, Sfb_real);

                MostrarResultados mostrarDiseno = new MostrarResultados();
                mostrarDiseno.MostrarResultadoDiseno(_ResCorona, _ResPinon, MostrarCorona, MostrarPinon);

                if (_ResCorona.FactorSeguridad >= 1 || _ResPinon.FactorSeguridad >= 1)
                {
                    _ResultadoFinalDisenoText.Text = "El diseño es válido con factores de seguridad mayores que 1.";
                }
                else { _ResultadoFinalDisenoText.Text = "El diseño no es válido."; }

            }

            else 
            {
                MessageBox.Show("Faltan datos. Complete los campos requeridos", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        // eventos de transición y guardado

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new _04_FactoresK_EsfuerzoVIEW(_datoGeometria,_datosCarga);
            }
        }

        private void GuardarResult_Click(object sender, RoutedEventArgs e)
        {
            PdfSharpDocs Plantilla = new PdfSharpDocs(_datoGeometria,_datosCarga,_factoresK,_ResPinon,_ResCorona);
            PdfManager miPdfManager = new PdfManager();
            PdfDocument documentoPdF = new PdfDocument();
            documentoPdF = Plantilla.PlantillaResultados();
            miPdfManager.GuardarYMostrarPdf(documentoPdF);
        }

        private void FinalizarTrabajo(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¡Está seguro de finalizar?, una vez de por terminado no podrá volver.", "Finalizar",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                VentanaInicialVIEWS VentanaInicial = new VentanaInicialVIEWS(); 

                if (ventanaPrincipal != null)
                {
                    ventanaPrincipal.windowsOperation.Content = null;
                    ventanaPrincipal.windowsOperation.Content = VentanaInicial;
                }
            }
        }
        #endregion

        
    }
}
