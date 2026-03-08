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
using System.Windows.Shapes;

namespace P01_ALBARRAN_VS_ENGRANAJES.VIEWS.Engranajes.Cilindricos_Rectos
{
    /// <summary>
    /// Lógica de interacción para _04_02_CalcularKb_EspersoAROView.xaml
    /// </summary>
    public partial class _04_01_CalcularKb_EspersoAROView : Window
    {
        DTO_ResultGeometrico _ValoresGeometricos = new DTO_ResultGeometrico();
        private readonly Action<double, double> _MetodoReasignaValorKB;

        public _04_01_CalcularKb_EspersoAROView(DTO_ResultGeometrico MIRESULTADO, Action<double, double> AsignarKB)
        {
            InitializeComponent();
            _ValoresGeometricos = MIRESULTADO;
            htValue.Text = _ValoresGeometricos.H_T.ToString();
            dgiValue.Text = _ValoresGeometricos.DIg.ToString();
            dpiValue.Text=_ValoresGeometricos.DIp.ToString();

            _MetodoReasignaValorKB = AsignarKB;
        }

        private void PermitirSoloNumeros(object sender, TextCompositionEventArgs e)
        {
            ValidarEntradas ValidarTextbox = new ValidarEntradas();
            ValidarTextbox.controlEntradaTextBox(sender, e);
        }

        private void calcularKB(object sender, RoutedEventArgs e)
        {
            ValidarEntradas ValidarNulidad = new ValidarEntradas();
            if (ValidarNulidad.validarEntradaNoNull(dhgTextBox,dhpTextBox) == true)
            {
                double dhg_value = double.Parse(dhgTextBox.Text);
                double dhp_value = double.Parse(dhpTextBox.Text);

                //Añado los valores de los dilametros de los ejes al DTO GEOMÉTRICO

                _ValoresGeometricos.DHp = dhp_value;
                _ValoresGeometricos.DHg = dhg_value;

                CalcularFactoresKM calcularKb = new CalcularFactoresKM();
                double tRcorona;
                double tRpinon;
                double kBcorona;
                double kBpinon;

                (tRcorona, tRpinon,kBcorona,kBpinon) = calcularKb.CalcularKB(_ValoresGeometricos.H_T,_ValoresGeometricos.DIp, _ValoresGeometricos.DIg,dhp_value,dhg_value);
                _MetodoReasignaValorKB(kBpinon,kBcorona);

                tRcorona_TextBox.Text = tRcorona.ToString();
                tRpinon_TextBox.Text = tRpinon.ToString();
                KBcorona_TextBox.Text=kBcorona.ToString();
                kBpinon_TextBox.Text=kBpinon.ToString();

            }
        }
    }
}
