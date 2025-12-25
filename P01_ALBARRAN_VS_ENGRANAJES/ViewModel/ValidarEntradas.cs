using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace P01_ALBARRAN_VS_ENGRANAJES.ViewModel
{
    class ValidarEntradas
    {

        // validar entrada, este método con sobrecarga, valida que las entradas no sean nulas, si es nulo, entonces devuelve falso
        public bool validarEntradaNoNull(TextBox cajaDP, TextBox cajaDG, ComboBox cajaAngulPresion) 
        {
            //True bloquea las entradas por teclado en un evento preview 

            if (string.IsNullOrWhiteSpace(cajaDP.Text)) return false;
            if (string.IsNullOrWhiteSpace(cajaDG.Text)) return false;

            // Validar que los valores sean numéricos
            if (!double.TryParse(cajaDP.Text, out _)) return false;
            if (!double.TryParse(cajaDG.Text, out _)) return false;

            // Validar que el ComboBox tenga una selección válida
            if (cajaAngulPresion.SelectedItem == null) return false;

            return true;

        }

        public bool validarEntradaNoNull(TextBox caja1, TextBox caja2)
        {
            //True bloquea las entradas por teclado en un evento preview 

            if (string.IsNullOrWhiteSpace(caja1.Text)) return false;
            if (string.IsNullOrWhiteSpace(caja2.Text)) return false;

            // Validar que los valores sean numéricos
            if (!double.TryParse(caja1.Text, out _)) return false;
            if (!double.TryParse(caja2.Text, out _)) return false;

            return true;

        }

        public bool validarEntradaNoNull(TextBox cajaTextBox)
        {
            //True bloquea las entradas por teclado en un evento preview 

            if (string.IsNullOrWhiteSpace(cajaTextBox.Text)) return false;

            // Validar que los valores sean numéricos
            if (!double.TryParse(cajaTextBox.Text, out _)) return false;

            return true;

        }



        //Para controlar las entradas de texto de un TextBox Sean numéricas.
        public void controlEntradaTextBox(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox textBox)
            {
                e.Handled = true;
                return;
            }

            string textoSimulado = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // ❌ Bloquear múltiples comas
            int cantidadComas = textoSimulado.Count(c => c == ',');
            if (cantidadComas > 1)
            {
                e.Handled = true;
                return;
            }

            // ❌ Bloquear puntos (no son válidos en es-EC como separador decimal)
            if (e.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // ✅ Validar con cultura ecuatoriana
            try
            {
                double.Parse(textoSimulado, new CultureInfo("es-EC"));
                e.Handled = false;
            }
            catch
            {
                e.Handled = true;
            }
        }


    }
}
