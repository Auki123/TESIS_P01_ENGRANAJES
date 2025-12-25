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
    /// Lógica de interacción para _04_FactoresK_EsfuerzoVIEW.xaml
    /// </summary>
    public partial class _04_FactoresK_EsfuerzoVIEW : UserControl
    {
        // Los objetos de datos geométricos y datos de carga, son objetos que ya tienen valores asignados en cálculos anteriores
        private DTO_ResultGeometrico _DatosGeometricos;
        private DTO_ResultCargasYcinematica _DatosDeCargas;

        // _DTO_ValoresK, se agregará valores en esta ventana de cálculo
        private DTO_ResultFactorK_Esf _DTO_ValoresK = new DTO_ResultFactorK_Esf();

        //Metodos para reasignacion de valor desde ventana nueva 
        private void AsignarValorKV(double valor)
        {
            _DTO_ValoresK.KV_FACTOR = valor;
            KVCALCULADO.Text = _DTO_ValoresK.KV_FACTOR.ToString();
        }

        private void AsignarValorKB(double KBpValue, double KBgValue)
        {
            _DTO_ValoresK.KBp_FACTOR = KBpValue;
            KBp_TextBox.Text = _DTO_ValoresK.KBp_FACTOR.ToString();

            _DTO_ValoresK.KBg_FACTOR = KBgValue;
            KBg_TextBox.Text = _DTO_ValoresK.KBg_FACTOR.ToString();
        }
        private void AsignarValorKa(double valor)
        {
            _DTO_ValoresK.KA_FACTOR = valor;
            Ka_value.Text = _DTO_ValoresK.KA_FACTOR.ToString();
        }

        //.............................................................



        public _04_FactoresK_EsfuerzoVIEW(DTO_ResultGeometrico dTO_ResultGeometrico, DTO_ResultCargasYcinematica dTO_ResultCargas)
        {
            InitializeComponent();
            _DatosGeometricos = dTO_ResultGeometrico;
            _DatosDeCargas = dTO_ResultCargas;
        }

        private void CalcularJ(object sender, RoutedEventArgs e)
        {
            CalcularFactoresKM calcularFactoresKM = new CalcularFactoresKM();
            (_DTO_ValoresK.J_FACTORPINON, _DTO_ValoresK.J_FACTORCORONA) = calcularFactoresKM.DameFactorJ(_DatosGeometricos.NP,_DatosGeometricos.NG,_DatosGeometricos.ANGULOPRESION);
            Jfactor_pinon.Text =_DTO_ValoresK.J_FACTORPINON.ToString();
            Jfactor_corona.Text = _DTO_ValoresK.J_FACTORCORONA.ToString();
        }

        private void Ir_A_CalcularKv(object sender, RoutedEventArgs e)
        {
            _04_01_CalcularKvModel VentanaCalcularKV = new _04_01_CalcularKvModel(_DatosDeCargas, AsignarValorKV);
            bool? resultado=VentanaCalcularKV.ShowDialog();

        }

        private void Ir_A_CalcularKB(object sender, RoutedEventArgs e)
        {
            _04_01_CalcularKb_EspersoAROView VentanaCalcularKB = new _04_01_CalcularKb_EspersoAROView(_DatosGeometricos,AsignarValorKB);
            bool? ResultadoCompeto = VentanaCalcularKB.ShowDialog();
        }

        private void Ir_A_CalcularKa(object sender, RoutedEventArgs e)
        {
            _04_03_CalcularKa_AplicacionView VentanaCalcularKa = new _04_03_CalcularKa_AplicacionView(AsignarValorKa);
            bool? Resultadoka = VentanaCalcularKa.ShowDialog();
        }

        private void ValidarEntradaTemp(object sender, TextCompositionEventArgs e)
        {
            ValidarEntradas ControlEntradaTextBox = new ValidarEntradas();
            ControlEntradaTextBox.controlEntradaTextBox(sender,e);
        }

        private void Ir_A_CalcularKT(object sender, RoutedEventArgs e)
        {
            CalcularFactoresKM factoresKM = new CalcularFactoresKM();
            ValidarEntradas validarEntradas = new ValidarEntradas();

            if (validarEntradas.validarEntradaNoNull(KT_Temperaturavalue) ==true)
            {
                double temperaturaCelcius = double.Parse(KT_Temperaturavalue.Text);
                _DTO_ValoresK.KT_FACTOR = factoresKM.CalcularKT(temperaturaCelcius);
                KT_value.Text=_DTO_ValoresK.KT_FACTOR.ToString();
            }
        }

        private void KR_SELECTION(object sender, SelectionChangedEventArgs e)
        {
            double KR;
            if (ValoresKR.SelectedItem != null) 
            {
                if (ValoresKR.SelectedItem == value90){ KR = 0.85; }
                else if (ValoresKR.SelectedItem == value99) { KR = 1.0; }
                else if (ValoresKR.SelectedItem == value99_9) { KR = 1.25; }
                else if (ValoresKR.SelectedItem == value99_99) { KR = 1.5; }
                else { KR = 0; }
                _DTO_ValoresK.KR_FACTOR = KR;
                KR_value.Text = KR.ToString();
            }
        }

        private void Ir_A_CalcularEsfuerzo(object sender, RoutedEventArgs e)
        {
            double VerificarSiHayValor0;
            VerificarSiHayValor0 = _DTO_ValoresK.VeificaDatoCompleto();
            if (VerificarSiHayValor0 != 0)
            {
                var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

                _05_DisenoFlexionView SiguenteUserC = new _05_DisenoFlexionView(_DatosGeometricos,_DatosDeCargas, _DTO_ValoresK);

                if (ventanaPrincipal != null)
                {
                    ventanaPrincipal.windowsOperation.Content = SiguenteUserC;
                }

            }
            else { MessageBox.Show("Todavía hay campos incompletos."); }
        }

        private void Regresar_Click(object sender, RoutedEventArgs e)
        {
            var ventanaPrincipal = Window.GetWindow(this) as MainWindow;

            if (ventanaPrincipal != null)
            {
                ventanaPrincipal.windowsOperation.Content = new _03_CalculoReaccionesVIEW(_DatosGeometricos);
            }
        }
    }
}
