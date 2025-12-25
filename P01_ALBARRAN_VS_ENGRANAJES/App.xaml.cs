using System.Configuration;
using System.Data;
using System.Windows;
using P01_ALBARRAN_VS_ENGRANAJES.VIEWS.VentanasUI;

namespace P01_ALBARRAN_VS_ENGRANAJES
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow MiVentana = new MainWindow();
            MiVentana.Show();
            
        }

    }

}
