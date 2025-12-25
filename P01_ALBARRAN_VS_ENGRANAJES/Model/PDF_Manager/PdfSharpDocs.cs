using P01_ALBARRAN_VS_ENGRANAJES.Model.DTO_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.IO;
using System.IO;
using PdfSharpCore.Pdf.Security;
using P01_ALBARRAN_VS_ENGRANAJES.Model.Engranaje;
using System.Web;

namespace P01_ALBARRAN_VS_ENGRANAJES.Model.PDF_Manager
{
    public class PdfSharpDocs
    {
        private readonly DTO_ResultGeometrico _ResultGeometrico;
        private readonly DTO_ResultCargasYcinematica _ResultCargas;
        private readonly DTO_ResultFactorK_Esf _ResultFactorK;
        private readonly DTO_ResultadoDiseno _ResultDisP; // resultado en diseño del piñ+ón
        private readonly DTO_ResultadoDiseno _ResultDisg;  // resultado en diseño de corona

        private readonly string _miplantillaResultados;

        private DateTime GuardarFecha = DateTime.Now;

        public PdfSharpDocs(DTO_ResultGeometrico Rgeom, DTO_ResultCargasYcinematica Rcarga, DTO_ResultFactorK_Esf R_kfactor, DTO_ResultadoDiseno Rpinon, DTO_ResultadoDiseno Rcorona)
        {

            // momentáneo
            _ResultGeometrico = Rgeom;
            _ResultCargas = Rcarga;
            _ResultFactorK = R_kfactor;
            _ResultDisP = Rpinon;
            _ResultDisg = Rcorona;

            _miplantillaResultados=Path.Combine("Assets", "ArchivosEditables", "PlantillaEcilindricosRectos1.pdf");
        }


        public PdfDocument PlantillaResultados()
        {
            PdfDocument document = PdfReader.Open(_miplantillaResultados, PdfDocumentOpenMode.Modify);

            PdfPage page1 = document.Pages[0];
            PdfPage page2 = document.Pages[1];
            PdfPage page3 = document.Pages[2];
            PdfPage page4 = document.Pages[3];
            PdfPage page5 = document.Pages[4];

            #region Estilos de letra

            XFont fontR = new XFont("Century Gothic", 11, XFontStyle.Regular);
            XFont fontR_8 = new XFont("Century Gothic", 7, XFontStyle.Regular);


            #endregion

            #region MODIFICACIONES EN EL ARCHIVO

            XGraphics grapicPage1 = XGraphics.FromPdfPage(page1);

            // Tabla 1 de primera página
            
            grapicPage1.DrawString(_ResultGeometrico.DP.ToString(), fontR, XBrushes.Black, new XPoint(320,346));          // diámetro de paso piñón
            grapicPage1.DrawString(_ResultGeometrico.DG.ToString(), fontR, XBrushes.Black, new XPoint(320, 368));      // DIÁMETRO DE PASO DE CORONA
            grapicPage1.DrawString(_ResultGeometrico.ANGULOPRESION.ToString(), fontR, XBrushes.Black, new XPoint(320, 390));         // ÁNGULO DE PRESIÓN
            grapicPage1.DrawString(_ResultCargas.POTENCIAHP.ToString(), fontR, XBrushes.Black, new XPoint(320, 412));      //  POTENCIA DE TRASMISIÓN
            grapicPage1.DrawString(_ResultCargas.VANGULARCORONA.ToString()+"-"+_ResultCargas.VANGULARPINON.ToString(), fontR, XBrushes.Black, new XPoint(320, 434));      //  VELOCIDAD DE OPERACIÓ

            // Tabla 2 de primera pag
            grapicPage1.DrawString(_ResultGeometrico.DP.ToString(), fontR, XBrushes.Black, new XPoint(382, 561));      // diámetro de paso piñón
            grapicPage1.DrawString(_ResultGeometrico.NP.ToString(), fontR, XBrushes.Black, new XPoint(382, 582.2));    // número de dientes piñón
            grapicPage1.DrawString(_ResultGeometrico.DHp.ToString(), fontR, XBrushes.Black, new XPoint(382, 603.4));    // diámetro de eje piñón
            grapicPage1.DrawString(_ResultGeometrico.DIp.ToString(), fontR, XBrushes.Black, new XPoint(382, 624.6));    // diámetro de raíz piñón
            grapicPage1.DrawString(_ResultGeometrico.DEp.ToString(), fontR, XBrushes.Black, new XPoint(382, 645.8));    // diámetro exterior del piñón
            grapicPage1.DrawString(_ResultGeometrico.DG.ToString(), fontR, XBrushes.Black, new XPoint(382, 688.2));    // diámetro de paso corona
            grapicPage1.DrawString(_ResultGeometrico.NG.ToString(), fontR, XBrushes.Black, new XPoint(382, 709.4));    // número de dientes corona
            grapicPage1.DrawString(_ResultGeometrico.DHg.ToString(), fontR, XBrushes.Black, new XPoint(382, 730.6));    // DIÁMETRO DE EJE DE CORONA
            grapicPage1.DrawString(_ResultGeometrico.DIg.ToString(), fontR, XBrushes.Black, new XPoint(382, 751.8));    // DIÁMETRO DE RAÍZ DE CORONA
            grapicPage1.DrawString(_ResultGeometrico.DEg.ToString(), fontR, XBrushes.Black, new XPoint(382, 773));      //  Diámetro exterior de corona

            XGraphics grapicPage2 = XGraphics.FromPdfPage(page2); //Página 2

            //Primera tabla de segunda página
            grapicPage2.DrawString(_ResultGeometrico.ANGULOPRESION.ToString(), fontR, XBrushes.Black, new XPoint(383, 134.2));    // ángulo de presión
            grapicPage2.DrawString(_ResultGeometrico.MODULO.ToString(), fontR, XBrushes.Black, new XPoint(383, 155.4));    // módulo métric
            grapicPage2.DrawString(_ResultGeometrico.M_G.ToString(), fontR, XBrushes.Black, new XPoint(383, 176.6));    // razón de engrane
            grapicPage2.DrawString(_ResultGeometrico.DISTANCIA_CENTROS.ToString(), fontR, XBrushes.Black, new XPoint(383, 197.8));    // distancia entre centros
            grapicPage2.DrawString(_ResultGeometrico.ADDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(383, 219));      // ADENDUM
            grapicPage2.DrawString(_ResultGeometrico.DEDDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(383, 240.2));    // DEDENDUM
            grapicPage2.DrawString(_ResultGeometrico.H_T.ToString(), fontR, XBrushes.Black, new XPoint(383, 261.4));    // PROFUNDIDAD DEL DIENTE
            grapicPage2.DrawString(_ResultGeometrico.anchocaraF.ToString(), fontR, XBrushes.Black, new XPoint(383, 282.6));    // ANCHO DE CARA
            grapicPage2.DrawString(_ResultGeometrico.RELACIONCONTACTO.ToString(), fontR, XBrushes.Black, new XPoint(383, 303.8));    // razon de contacto

            //Segunda tabla de segunda página valores de cinemática
            grapicPage2.DrawString(_ResultCargas.VANGULARPINON.ToString(), fontR, XBrushes.Black, new XPoint(365, 467));      // VELOCIDAD ANGULAR EN PIÑON
            grapicPage2.DrawString(_ResultCargas.VANGULARCORONA.ToString(), fontR, XBrushes.Black, new XPoint(365, 488.5));    // velocidad angular en corona
            grapicPage2.DrawString(_ResultCargas.VLINEAPASO.ToString(), fontR, XBrushes.Black, new XPoint(365, 510));      // velocidad en linea de paso
            grapicPage2.DrawString(_ResultCargas.TORQUEPINON.ToString(), fontR, XBrushes.Black, new XPoint(365, 531.5));    // torque en piñ+on
            grapicPage2.DrawString(_ResultCargas.TORQUECORONA.ToString(), fontR, XBrushes.Black, new XPoint(365, 553));      // torque en corona
            grapicPage2.DrawString(_ResultCargas.WAXIALCORONA.ToString(), fontR, XBrushes.Black, new XPoint(365, 574.5));    // fuerza tangencial
            grapicPage2.DrawString(_ResultCargas.WRADIALPINON.ToString(), fontR, XBrushes.Black, new XPoint(365, 596));      // fuerza radial en elementos

            // Tabla de valores de los factores k, está entre la segunda y tercera página

            grapicPage2.DrawString(_ResultFactorK.J_FACTORPINON.ToString(), fontR, XBrushes.Black, new XPoint(309, 689));  //  Factor geometrico del piñón
            grapicPage2.DrawString(_ResultFactorK.J_FACTORCORONA.ToString(), fontR, XBrushes.Black, new XPoint(309, 710));  // factor geométrico corona
            grapicPage2.DrawString(_ResultFactorK.KV_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 731));  // factor dinámico

            XGraphics grapicPage3 = XGraphics.FromPdfPage(page3); //Página 3
                             // FACTORES K
            grapicPage3.DrawString(_ResultFactorK.KA_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 112));  // factor de aplicación
            grapicPage3.DrawString(_ResultFactorK.KS.ToString(), fontR, XBrushes.Black, new XPoint(309, 148));  // de tamaño
            grapicPage3.DrawString(_ResultFactorK.KBp_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 171));  // DE ESPESOR DE ARO EN PIÑ+ON
            grapicPage3.DrawString(_ResultFactorK.KBg_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 193));  // DE ESPESOR DE ARO CORONA
            grapicPage3.DrawString(_ResultFactorK.KM_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 217));  // DE DISTRIBUCIÓN DE CARGA
            grapicPage3.DrawString(_ResultFactorK.KL.ToString(), fontR, XBrushes.Black, new XPoint(309, 280));  // DE VIDA
            grapicPage3.DrawString(_ResultFactorK.KT_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 301));  // DE TEMPERATURA
            grapicPage3.DrawString(_ResultFactorK.KR_FACTOR.ToString(), fontR, XBrushes.Black, new XPoint(309, 321));  // DE CONFIABILIDAD


            // Sección de materiales, misma hoja 3.

            // Para todos los materiales
            string material = "Material: "+_ResultDisP.NOMBRE_MATERIAL+", Clase: "+_ResultDisP.CLASE_AGMA+", Designación: "+_ResultDisP.DESIGNACION_MATERIAL+", Tratamiento: "+_ResultDisP.TRATAMIENTO_MATERIAL+".";
            string resistenciaMaterial = _ResultDisP.Sfb_prima.ToString(); // resistencia de la agma
            string resistenciaReal = _ResultDisP.Sfb.ToString(); // resistencia a condiciones especificadas
            
            //_______________________________________________________

            grapicPage3.DrawString(material, fontR_8, XBrushes.Black, new XPoint(142, 390));  //Material SELECCIONADO
            grapicPage3.DrawString(resistenciaMaterial, fontR, XBrushes.Black, new XPoint(164, 442)); // Resistencia de la AGMA
            grapicPage3.DrawString(resistenciaReal, fontR, XBrushes.Black, new XPoint(164, 493));   // Resistencia real del material bajo condiciones especificadas


            // ULTIMA TABLA DE HOJA 3

            grapicPage3.DrawString(_ResultDisP.SIGMAB.ToString(), fontR, XBrushes.Black, new XPoint(182, 584)); // esfuerzo en piñ+ón
            grapicPage3.DrawString(resistenciaReal, fontR, XBrushes.Black, new XPoint(257, 584)); // ESFUERZO QUE SOPORTA EL MATERIA EN DICHAS CONDICIONES
            grapicPage3.DrawString(_ResultDisP.FactorSeguridad.ToString(), fontR, XBrushes.Black, new XPoint(355, 584)); // factor de seguridad en el piñón
            string cumplePinon = "";
            if (_ResultDisP.FactorSeguridad >= 1) { cumplePinon = "Si"; } else { cumplePinon = "No"; }
            grapicPage3.DrawString(cumplePinon, fontR, XBrushes.Black, new XPoint(465, 584));   // CONDICIÓN DE CUMPLIMIENTO

            grapicPage3.DrawString(_ResultDisg.SIGMAB.ToString(), fontR, XBrushes.Black, new XPoint(182, 607));  // ESFUERZO EN CORONA
            grapicPage3.DrawString(resistenciaReal, fontR, XBrushes.Black, new XPoint(257, 607));  // ESFUERZO QUE SOPORTA EL MATERIAL EN CONDICIONES ESPECIFICADAS
            grapicPage3.DrawString(_ResultDisg.FactorSeguridad.ToString(), fontR, XBrushes.Black, new XPoint(355, 607));  // FACTOR DE SEGURIDAD EN LA CORONA
            string cumpleCorona = "";
            if (_ResultDisg.FactorSeguridad >= 1) { cumpleCorona = "Si"; } else { cumpleCorona = "No"; }
            grapicPage3.DrawString(cumpleCorona, fontR, XBrushes.Black, new XPoint(465, 607));  // condicion de cumplimiento en corona


            // Cumplimiento de condiciones del factor de seguridad en piñ+ón y corona
            string cumpleAmbos = "";
            if ((_ResultDisP.FactorSeguridad >= 1 )&& (_ResultDisg.FactorSeguridad>=1)) { cumpleAmbos = "Si"; } else { cumpleAmbos = "No"; }
            grapicPage3.DrawString(cumpleAmbos, fontR, XBrushes.Black, new XPoint(147, 654.5));


            // Fechas
            grapicPage3.DrawString(GuardarFecha.ToString("HH'h:'mm'min'"), fontR, XBrushes.Black, new XPoint(147, 705));
            grapicPage3.DrawString(GuardarFecha.ToShortDateString(), fontR, XBrushes.Black, new XPoint(147, 731));


            // Hoja 4, plano corona, todos los valores hasta el siguiente comentario pertenecen a la corona

            XGraphics grapicPage4 = XGraphics.FromPdfPage(page4); //Página 4

            grapicPage4.DrawString(_ResultGeometrico.DEg.ToString(), fontR, XBrushes.Black, new XPoint(440, 533));  // diámetro exterior

            grapicPage4.DrawString(_ResultGeometrico.DG.ToString(), fontR, XBrushes.Black, new XPoint(440, 558));  // DIÁMETRO DE PASO
            grapicPage4.DrawString(_ResultGeometrico.DIg.ToString(), fontR, XBrushes.Black, new XPoint(440, 580));  // DIÁMETRO DE RAÍZ
            grapicPage4.DrawString(_ResultGeometrico.DHg.ToString(), fontR, XBrushes.Black, new XPoint(440, 602));  // DIÁMETRO DE EJE
            grapicPage4.DrawString(_ResultGeometrico.PASOCIRCULAR.ToString(), fontR, XBrushes.Black, new XPoint(440, 624));  // PASO CIRCULAR
            grapicPage4.DrawString(_ResultGeometrico.ADDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(440, 645));  // ADENDUM

            grapicPage4.DrawString(_ResultGeometrico.DEDDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(440, 665));  // DEDENDUM
            grapicPage4.DrawString(_ResultGeometrico.H_T.ToString(), fontR, XBrushes.Black, new XPoint(440, 686));  // PROFUNDIDAD DEL DIENTE
            grapicPage4.DrawString(_ResultGeometrico.anchocaraF.ToString(), fontR, XBrushes.Black, new XPoint(440, 707));  // ANCHO DE CARA
            grapicPage4.DrawString(_ResultGeometrico.MODULO.ToString(), fontR, XBrushes.Black, new XPoint(440, 727));  // MÓDULO METRICO

            grapicPage4.DrawString(material, fontR_8, XBrushes.Black, new XPoint(245, 781));

            // Hoja 5, plano piñ+on TODOS LOS VALORES PARA ESTA SECCIÓN SON DATOS PARA EL PIÑ+ON

            XGraphics grapicPage5 = XGraphics.FromPdfPage(page5); //Página 5

            grapicPage5.DrawString(_ResultGeometrico.DEp.ToString(), fontR, XBrushes.Black, new XPoint(440, 524));  // DIÁMETRO EXTERIOR

            grapicPage5.DrawString(_ResultGeometrico.DP.ToString(), fontR, XBrushes.Black, new XPoint(440, 549));  // diámetro de paso
            grapicPage5.DrawString(_ResultGeometrico.DIp.ToString(), fontR, XBrushes.Black, new XPoint(440, 571));  // DIÁMETRO DE RAÍZ
            grapicPage5.DrawString(_ResultGeometrico.DHp.ToString(), fontR, XBrushes.Black, new XPoint(440, 593));  // DIÁMETRO DE EJE
            grapicPage5.DrawString(_ResultGeometrico.PASOCIRCULAR.ToString(), fontR, XBrushes.Black, new XPoint(440, 615));  // PASI CIRCULAR
            grapicPage5.DrawString(_ResultGeometrico.ADDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(440, 636));  // ADENDUM

            grapicPage5.DrawString(_ResultGeometrico.DEDDENDUM.ToString(), fontR, XBrushes.Black, new XPoint(440, 656));  // DEDENDUM
            grapicPage5.DrawString(_ResultGeometrico.H_T.ToString(), fontR, XBrushes.Black, new XPoint(440, 677));  // PROFUNDIDAD TOTAL DEL DIENTE
            grapicPage5.DrawString(_ResultGeometrico.anchocaraF.ToString(), fontR, XBrushes.Black, new XPoint(440, 696));  // ANCHO DE CARA
            grapicPage5.DrawString(_ResultGeometrico.MODULO.ToString(), fontR, XBrushes.Black, new XPoint(440, 716));  // MÓDULO MÉTRICO

            grapicPage5.DrawString(material, fontR_8, XBrushes.Black, new XPoint(245, 781));



            #endregion

            #region Protección del PDF

            PdfSecuritySettings security = document.SecuritySettings;
            security.OwnerPassword = "ES_MI_DOC_NO_TIENES_DERECHO";
            security.PermitModifyDocument = false;
            security.PermitExtractContent = false;
            security.PermitAnnotations = false;
            security.PermitFormsFill=false;
            security.PermitAssembleDocument = false;
            security.PermitPrint = true;

            #endregion

            return document;

        }
    }
}
