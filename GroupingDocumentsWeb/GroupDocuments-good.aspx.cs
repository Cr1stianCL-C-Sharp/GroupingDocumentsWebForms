using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GroupingDocumentsWeb.clases;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using Novacode;
using System.Diagnostics;

namespace GroupingDocumentsWeb
{
    public partial class GroupDocuments : System.Web.UI.Page
    {

        protected Functions fn = new Functions();       
        protected string fileName = string.Empty;
        protected string UploadPath = string.Empty;
        protected String rootPathUpload = ("~/uploads");

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadMultipleFiles(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                try
                {
                    //Stcring rootPath = Server.MapPath("~/uploadsPDF");                    
                    //String FinalPath = String.Empty;
                   
                    Boolean cargado = false;
                    //String SUploads = @"C:\localfolderpdf";
                    foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                    {
                        if (postedFile.FileName != "")
                        {
                            fileName = Path.GetFileName(postedFile.FileName);
                            //FinalPath = Server.MapPath(rootPath) + @"\" + fileName;
                            postedFile.SaveAs(Server.MapPath(rootPathUpload) + @"\" + fileName);
                            cargado = true;
                        }
                        else
                        {
                            lblSuccess.Text = string.Format("No se puede cargar porque no has seleccionado nada...Selecciona un Archivo PDF");
                            lblSuccess.ForeColor = System.Drawing.Color.Red;
                            lblSuccess.Font.Bold = true;
                        }                        
                    }
                    if (cargado)
                    {
                        loadFileOnFramePDF(fileName); //carga el pdf al iframe
                        lblSuccess.Text = string.Format("{0} Archivos Se Han Cargado Correctamente.", FileUpload1.PostedFiles.Count);
                        lblSuccess.ForeColor = System.Drawing.Color.Green;
                        lblSuccess.Font.Bold = true;
                    }
                    
                    
                }
                catch (WebException)
                {
                    lblSuccess.Text = string.Format("{0} - Se genero este error, porque no se soportan archivos tan grandes.", "Error de Limite de carga...");
                }

            }
        }////end UploadMultipleFiles


        public void SaveAsDoc(){
            //String FolderPath = String.Empty;

            try
            {
                //FolderPath = fn.CreateFolderUpload(fileName);
                if (fileName != "")
                {
                    String PathToFile = ConvertPDFToDoc(fileName);
                    //(Server.MapPath(rootPathUpload) + @"\" + fileName);
                }
            }
            catch(Exception)
            {

            }     
        }


        public void loadFileOnFramePDF(string FileName) ///carga al iframe la ruta dada.
        {
            try
            {
                String SPath = "/uploads/"+ FileName;
                HtmlControl contentPanel1 = (HtmlControl)this.FindControl("IframeDocument");
                if (contentPanel1 != null)
                contentPanel1.Attributes["src"] = SPath;
            }
            catch (WebException)
            {

            }
        }

        public void unloadPdf(object sender, EventArgs e) ////limpia el iframe
        {
            try
            {
                String Blankpath = "./BlankPDF.pdf";
                HtmlControl contentPanel1 = (HtmlControl)this.FindControl("IframeDocument");
                if (contentPanel1 != null)
                    contentPanel1.Attributes["src"] = Blankpath;
            }
            catch(WebException)
            {
            
            }            
        }

        public void ConvertPDFToJPG(string Spath)
        {
            ////Ghostscript.NET
            //string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //Ghostscript.NET.Rasterizer.GhostscriptRasterizer rasterizer = null;
            //Ghostscript.NET.GhostscriptVersionInfo vesion = new Ghostscript.NET.GhostscriptVersionInfo(new Version(0, 0, 0), path + @"\gsdll32.dll", string.Empty, Ghostscript.NET.GhostscriptLicense.GPL);

            //using (rasterizer = new Ghostscript.NET.Rasterizer.GhostscriptRasterizer())
            //{
            //    rasterizer.Open(file, vesion, false);

            //    for (int i = 1; i <= rasterizer.PageCount; i++)
            //    {
            //        string pageFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file) + "-p" + i.ToString() + ".jpg");

            //        Image img = rasterizer.GetPage(dpi, dpi, i);
            //        img.Save(pageFilePath, ImageFormat.Jpeg);
            //    }

            //    rasterizer.Close();
            //}

        }
        public void ConvertJPGToPDF(string Spath)
        {
            //ITEXSHARP

            //Document document = new Document();
            //using (var stream = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    PdfWriter.GetInstance(document, stream);
            //    document.Open();
            //    using (var imageStream = new FileStream("test.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //    {
            //        var image = Image.GetInstance(imageStream);
            //        document.Add(image);
            //    }
            //    document.Close();
            //}

        }
        public void ConvertPDFToTIFF(string Spath)
        {


        }

        protected internal String ConvertPDFToDoc(string PDFpath)
        {
            try
            {
                PDDocument PDFdoc = null;
                PDFTextStripper textstrip = new PDFTextStripper();
                String StringDocx = String.Empty;
                String DocxPath = String.Empty;

                PDFdoc = PDDocument.load(PDFpath);
                StringDocx = textstrip.getText(PDFdoc);
                PDFdoc.close(); //cierra el pdf

                ///DocxPath = fn.CreateFolderToSaveDocs(fn.GenerateName()); ///genera la ruta para guardar el archivo.
                DocxPath = fn.CreateFolderToSaveDocs(fileName); ///genera la ruta para guardar el archivo.
                var wordDoc = DocX.Create(DocxPath);
                wordDoc.InsertParagraph(StringDocx);
                wordDoc.Save();
                ////Process.Start("winword.exe", DocxPath);
                return DocxPath;
            }
            catch (Exception)
            {
                return "";
            }   
        }

       


    }
}