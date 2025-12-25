using System.Windows;
using PdfiumViewer;


namespace P01_ALBARRAN_VS_ENGRANAJES.VIEWS.VentanasUI
{
    /// <summary>
    /// Lógica de interacción para ManualUsuarioWiew.xaml
    /// </summary>
    /// 
    public partial class ManualUsuarioView : Window
    {
        private PdfViewer _pdfViewer;
        private PdfDocument _pdfDocument;

        public ManualUsuarioView()
        {
            InitializeComponent();
            _pdfViewer =  new PdfViewer();
            winFormsHost.Child= _pdfViewer;
        }

        public void AbrirPDF(string ruta)
        {
            _pdfDocument?.Dispose();
            _pdfDocument = PdfDocument.Load(ruta);
            _pdfViewer.Document = _pdfDocument;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _pdfViewer?.Dispose();
            _pdfDocument?.Dispose();
        }


    }
}
