using Microsoft.Win32;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
namespace P01_ALBARRAN_VS_ENGRANAJES.Model.PDF_Manager
{
    class PdfManager
    {
        public void GuardarYMostrarPdf(PdfDocument doc)
        {
            // Abrir cuadro de diálogo "Guardar como"
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                Title = "Guardar documento PDF",
                FileName = "Documento.pdf" // nombre sugerido
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Guardar en la ruta seleccionada
                    doc.Save(saveFileDialog.FileName);

                    // Abrir con visor externo predeterminado
                    Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                } 
                catch (System.IO.IOException)
                {
                    MessageBox.Show("El documento está en uso, cierre el archivo e inténtelo de nuevo","Error",MessageBoxButton.OK);
                }
            }
        }

        public void AbrirManualUsuario()
        {
            // Construir ruta relativa al ejecutable
            string rutaManual = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                             "Assets", "ArchivosEditables", "ManualDeUsuario.pdf");

            if (File.Exists(rutaManual))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(rutaManual) { UseShellExecute = true });
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo abrir el manual. Verifique que tenga instalado un lector de PDF.",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el manual de usuario en la carpeta del proyecto.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
