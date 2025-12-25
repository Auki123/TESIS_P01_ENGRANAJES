using P01_ALBARRAN_VS_ENGRANAJES.DataBase;
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
    /// Lógica de interacción para _04_03_CalcularKa_SobrecargaView.xaml
    /// </summary>
    public partial class _04_03_CalcularKa_AplicacionView : Window
    {
        private readonly Action<double> _MetodoAsignarKa;
        private readonly BaseDatos MiKArepository = new BaseDatos();
        private List<TipoMaquina> miMaquinaImpulsora;
        private List<TipoMaquina> miMaquinaImpulsada;
        public _04_03_CalcularKa_AplicacionView(Action<double> MetodoAsignarKa)
        {
            InitializeComponent();
            miMaquinaImpulsora = MiKArepository.ObtenerImpulsoras();
            miMaquinaImpulsada = MiKArepository.ObtenerImpulsadas();
            cmbImpulsora.ItemsSource = miMaquinaImpulsora;
            cmbImpulsada.ItemsSource = miMaquinaImpulsada;
            _MetodoAsignarKa = MetodoAsignarKa;
        }

        private void FiltrarKAvalues(object sender, SelectionChangedEventArgs e)
        {
            if (cmbImpulsora.SelectedValue != null && cmbImpulsada.SelectedValue != null)
            {
                int idImpulsora = (int)cmbImpulsora.SelectedValue;
                int idImpulsada = (int)cmbImpulsada.SelectedValue;

                var valor = MiKArepository.ObtenerKaValor(idImpulsora, idImpulsada);
                ka_value_selected.Text = valor.HasValue ? valor.Value.ToString("F3") : "No definido";

                double ValorTransferencia = 0;

                if (valor != null)
                    
                {
                    ValorTransferencia = valor.Value;
                }

                _MetodoAsignarKa(ValorTransferencia);
            }
        }
    }
}
