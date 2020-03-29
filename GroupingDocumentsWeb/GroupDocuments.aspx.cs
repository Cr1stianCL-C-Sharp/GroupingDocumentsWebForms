using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;
using System.Collections;
using Spire.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICSharpCode.SharpZipLib.Zip;

namespace GroupingDocumentsWeb
{
    public partial class GroupDocuments : System.Web.UI.Page
    {

        protected Functions fn = new Functions();       
        //protected string fileName; //= string.Empty;
        //protected string UploadPath = string.Empty;
        protected static String rootPath = ("~/Main");
        protected static String uploadPath = ("/Upload");
        protected static String IMGexportedPath = ("/IMGexported");
        protected static String PDFexportedPath = ("/PDFexported");
        protected static String DOCXexportedPath = ("/DOCXexported");
        protected static String ZIPPEDexportedPath = ("/ZIPPEDexported");
        //protected static String rootPathSavedDocs = ("~/SavedDocs");

        protected String UploadPath
        {
            get { return ViewState["UploadPath"] as String; }
            set { ViewState["UploadPath"] = value; }
        }
        protected String fileName
        {
            get { return ViewState["fileName"] as String; }
            set { ViewState["fileName"] = value; }
        }

        protected String AssignedFolder
        {
            get { return ViewState["AssignedFolder"] as String; }
            set { ViewState["AssignedFolder"] = value; }
        }

        protected String zipfName
        {
            get { return ViewState["zipfName"] as String; }
            set { ViewState["zipfName"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //LabelShowAlert.Visible = false;

            if (AssignedFolder == null) //|| AssignedFolder=="")
            AssignedFolder = fn.AssignFolder();

        }


        protected void UploadMultipleFiles(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                try
                {
                    //Stcring rootPath = Server.MapPath("~/uploadsPDF");                    
                    String SaveDocumentPath = String.Empty;
                   
                    Boolean cargado = false;
                    //String SUploads = @"C:\localfolderpdf";
                    foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                    {
                        if (postedFile.FileName != "" && AssignedFolder!=null)
                        {
                            fileName = Path.GetFileName(postedFile.FileName);
                            //String FinalPath = (String.Format("{0}\\{1}\\{2}\\{3}", rootPath, AssignedFolder, uploadPath));
                            SaveDocumentPath = fn.CreateFolderUpload(String.Format("{0}\\{1}\\{2}", rootPath, AssignedFolder, uploadPath));
                            //postedFile.SaveAs(Server.MapPath(String.Format("{0}\\{1}\\{2}\\{3}", rootPath, AssignedFolder, uploadPath, fileName)));                           
                            postedFile.SaveAs(Server.MapPath(String.Format("{0}\\{1}", SaveDocumentPath,fileName)));
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
        public void SaveAsDoc(object sender, EventArgs e)
        {   
            try
            {               
                if (fileName != ""  && fileName!=null)
                {
                    String PathToFile = ConvertPdfToDoc(fileName);       
                    String outFilename = fileName.Replace(".pdf", ".docx");                  
                    String ReturFile = String.Format("{0}\\{1}", rootPath, PathToFile);  

                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Transfer-Encoding", "Binary");                    
                    Response.AddHeader("Content-disposition", "attachment; filename=\""+ outFilename + "\"");                   
                    Response.WriteFile(ReturFile);                   
                    Response.End();     
                    #region Commented
                    //(Server.MapPath(rootPathUpload) + @"\" + fileName);
                    //Response.AddHeader("Content-disposition", "attachment; filename=\"IdeaPark_ER_diagram.docx\"");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + @"ideaPark\DesktopModules\ResourceModule\pdf_resources\IdeaPark_ER_diagram.pdf");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + ReturFile);
                    //String ReturFile = String.Format("{0}\\{1}", @"/SavedDocs", PathToFile);
                    #endregion
                }
            }
            catch(WebException s)
            {
                Response.Write(String.Format("{0}\\{1}\\{2}", s.StackTrace, s.Message, s.InnerException));
            }     
        }
       
        public void SaveAsJpg(object sender, EventArgs e)
        {
            try
            {
                if (fileName != "" && fileName != null)
                {                    
                    String PathToPDFSplited= SplitPdfPages(fileName);
                    String PathToFiles = ConvertPdfToImage(imgformat.jpg, PathToPDFSplited);  
                    String PathToZip = CompressToZip(PathToFiles);
                    String outFilename = String.Format("{0}.zip", zipfName);

                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Transfer-Encoding", "Binary");
                    Response.AddHeader("Content-disposition", "attachment; filename=\"" + outFilename + "\"");
                    Response.WriteFile(PathToZip);
                    Response.Write("true");
                    Response.End();
                    #region Commented
                    //(Server.MapPath(rootPathUpload) + @"\" + fileName);
                    //Response.AddHeader("Content-disposition", "attachment; filename=\"IdeaPark_ER_diagram.docx\"");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + @"ideaPark\DesktopModules\ResourceModule\pdf_resources\IdeaPark_ER_diagram.pdf");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + ReturFile);
                    //String ReturFile = String.Format("{0}\\{1}", @"/SavedDocs", PathToFile);
                    #endregion
                }
            }
            catch (WebException)
            {
                Response.Write("false");
                //Response.Write(String.Format("{0}\\{1}\\{2}", s.StackTrace, s.Message, s.InnerException));
            }
        }
        public void SaveAsTiff(object sender, EventArgs e)
        {
            try
            {
                if (fileName != "" && fileName != null)
                {
                    String PathToPDFSplited = SplitPdfPages(fileName);
                    String PathToFiles = ConvertPdfToImage(imgformat.tiff, PathToPDFSplited);
                    String PathToZip = CompressToZip(PathToFiles);
                    String outFilename = String.Format("{0}.zip", zipfName);

                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Transfer-Encoding", "Binary");
                    Response.AddHeader("Content-disposition", "attachment; filename=\"" + outFilename + "\"");
                    Response.WriteFile(PathToZip);
                    Response.Write("true");
                    Response.End();
                    #region Commented
                    //(Server.MapPath(rootPathUpload) + @"\" + fileName);
                    //Response.AddHeader("Content-disposition", "attachment; filename=\"IdeaPark_ER_diagram.docx\"");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + @"ideaPark\DesktopModules\ResourceModule\pdf_resources\IdeaPark_ER_diagram.pdf");
                    //Response.WriteFile(HttpRuntime.AppDomainAppPath + ReturFile);
                    //String ReturFile = String.Format("{0}\\{1}", @"/SavedDocs", PathToFile);
                    #endregion
                }
            }
            catch (WebException)
            {
                Response.Write("false");
                //Response.Write(String.Format("{0}\\{1}\\{2}", s.StackTrace, s.Message, s.InnerException));
            }
        }

        ////////public void SaveAsTiff(object sender, EventArgs e)
        ////////{
        ////////    try
        ////////    {
        ////////        if (fileName != "" && fileName != null)
        ////////        {
        ////////            String PathToPDFSplited = SplitPdfPages(fileName);
        ////////            String PathToFiles = ConvertPdfToImage(imgformat.tiff, PathToPDFSplited);


        ////////            String outFilename = fileName.Replace(".pdf", ".docx");
        ////////            String ReturFile = String.Format("{0}\\{1}", rootPath, PathToFiles);

        ////////            Response.AddHeader("Content-Type", "application/octet-stream");
        ////////            Response.AddHeader("Content-Transfer-Encoding", "Binary");
        ////////            Response.AddHeader("Content-disposition", "attachment; filename=\"" + outFilename + "\"");
        ////////            Response.WriteFile(ReturFile);
        ////////            Response.Write("true");
        ////////            Response.End();
        ////////            #region Commented
        ////////            //(Server.MapPath(rootPathUpload) + @"\" + fileName);
        ////////            //Response.AddHeader("Content-disposition", "attachment; filename=\"IdeaPark_ER_diagram.docx\"");
        ////////            //Response.WriteFile(HttpRuntime.AppDomainAppPath + @"ideaPark\DesktopModules\ResourceModule\pdf_resources\IdeaPark_ER_diagram.pdf");
        ////////            //Response.WriteFile(HttpRuntime.AppDomainAppPath + ReturFile);
        ////////            //String ReturFile = String.Format("{0}\\{1}", @"/SavedDocs", PathToFile);
        ////////            #endregion
        ////////        }
        ////////    }
        ////////    catch (WebException)
        ////////    {
        ////////        Response.Write("false");
        ////////        //Response.Write(String.Format("{0}\\{1}\\{2}", s.StackTrace, s.Message, s.InnerException));
        ////////    }
        ////////}
        public void loadFileOnFramePDF(string FileName) ///carga al iframe la ruta dada.
        {
            try
            {
                //String SPath = "/uploads/"+ FileName;

                String SPath = String.Format("{0}\\{1}\\{2}\\{3}", rootPath, AssignedFolder, uploadPath, FileName);
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

        public String CompressToZip(String route)
        {
            try
            {
                //FileInfo[] FilesPathArray = new DirectoryInfo(route).GetFiles()
                //                                      .OrderBy(f => f.LastWriteTime)
                //                                      .ToArray();
            route = Server.MapPath(route);
            String dirwhereZip = String.Format("{0}\\{1}\\{2}",rootPath,AssignedFolder, ZIPPEDexportedPath);
            fn.CreateFolderUpload(dirwhereZip);
            String dirwhereZipMap =Server.MapPath(dirwhereZip);

            string[] filenames = Directory.GetFiles(route);
            DateTime current = DateTime.Now;
            zipfName = "Request" + current.Date.Day.ToString() + current.Date.Month.ToString() + current.Date.Year.ToString() + current.TimeOfDay.Duration().Hours.ToString() + current.TimeOfDay.Duration().Minutes.ToString() + current.TimeOfDay.Duration().Seconds.ToString();
            // Zip up the files - From SharpZipLib Demo Code
            using (ZipOutputStream s = new ZipOutputStream(File.Create(dirwhereZipMap + "\\" + zipfName + ".zip")))
            {
                s.SetLevel(9); // 0-9, 9 being the highest level of compression

                byte[] buffer = new byte[4096];

                foreach (string file in filenames)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);

                        }
                        while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Close();
            }

            return String.Format("{0}\\{1}.zip" , dirwhereZip, zipfName);

            }
            catch (Exception)
            {
                return null;
            }
        }

        public String SplitPdfPages(String fileName)
        {
            string FileDataDir = Server.MapPath(String.Format("{0}\\{1}\\{2}\\{3}", rootPath, AssignedFolder, uploadPath,fileName));
            string OutDataDir = (String.Format("{0}\\{1}\\{2}", rootPath, AssignedFolder, PDFexportedPath));

            try
                {
                    fn.CreateFolderUpload(OutDataDir);
                    PdfCopy copy;                   
                    PdfReader reader = new PdfReader(FileDataDir);    
                   
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {                        
                        Document document = new Document();
                        String OutFilename = fileName.Replace(".pdf", "");
                        String ruta = String.Format("{0}\\[{1}][{2}].pdf", OutDataDir, OutFilename,i );                        
                        copy = new PdfCopy(document, new FileStream(Server.MapPath(String.Format("{0}\\{2}[{1}].pdf", OutDataDir, OutFilename, i)), FileMode.Create));
                        document.Open();                      
                        copy.AddPage(copy.GetImportedPage(reader, i));                     
                        document.Close();                    
                    }
                    return OutDataDir;              
                }
            catch (Exception)
            {
                return null;                
            }


        }

        public enum imgformat
        {
            jpg,
            tiff,
            png,
            bmp
        };

        public String ConvertPdfToImage(imgformat img,String Route)
        {           
            string dataDir = Server.MapPath(Route);
            ////string[] extensions = { ".jpg", ".txt", ".asp", ".css", ".cs", ".xml" };
            string[] extensions = { ".pdf" };
            String Outfilename = String.Empty;
            String FinalSaveJPG = String.Empty;
            try
            {
                //////////////    String[] FilesPath = Directory.GetFiles(dataDir, "*.*")
                //////////////            .Where(f => extensions.Contains(new FileInfo(f).Extension.ToLower())).ToArray();                                           

                FileInfo[] FilesPathArray = new DirectoryInfo(dataDir).GetFiles()
                                                  .OrderBy(f => f.LastWriteTime)
                                                  .ToArray();

                int FilesCount = FilesPathArray.Length;
                for (int i = 0; i < FilesCount; i++)
                {

                    Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                    //doc.LoadFromFile(FilesPath[i]);
                    String kookoko = (String.Format("{0}\\{1}", dataDir, FilesPathArray[i]));

                    doc.LoadFromFile(String.Format("{0}\\{1}", dataDir, FilesPathArray[i]));
                    System.Drawing.Image bmp = doc.SaveAsImage(0);

                    System.Drawing.Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
                    System.Drawing.Image zoomImg = new System.Drawing.Bitmap((int)(emf.Size.Width * 2), (int)(emf.Size.Height * 2));
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(zoomImg))
                    {
                        g.ScaleTransform(2.0f, 2.0f);
                        g.DrawImage(emf, new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), emf.Size), new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), emf.Size), System.Drawing.GraphicsUnit.Pixel);
                    }

                    Outfilename = fileName.Replace(".pdf", "");
                    FinalSaveJPG = String.Format("{0}\\{1}\\{2}", rootPath, AssignedFolder, IMGexportedPath);
                    fn.CreateFolderUpload(FinalSaveJPG);

                    switch (img)
                    {
                        case imgformat.jpg:
                            zoomImg.Save(Server.MapPath(String.Format("{0}\\{1}[{2}].jpg", FinalSaveJPG, i, Outfilename)), ImageFormat.Jpeg);
                            break;
                        case imgformat.bmp:
                            zoomImg.Save(Server.MapPath(String.Format("{0}\\{1}[{2}].bmp", FinalSaveJPG, i, Outfilename)), ImageFormat.Bmp);
                            break;
                        case imgformat.png:
                            zoomImg.Save(Server.MapPath(String.Format("{0}\\{1}[{2}].png", FinalSaveJPG, i, Outfilename)), ImageFormat.Png);
                            break;
                        case imgformat.tiff:
                            zoomImg.Save(Server.MapPath(String.Format("{0}\\{1}[{2}].tiff", FinalSaveJPG, i, Outfilename)), ImageFormat.Tiff);
                            break;
                    }
                }
                return FinalSaveJPG;

            }
            catch(Exception)
            {
                return null;
            }
            #region comenteed
            //String aaa = "asddasdasmkld";
            //emf.Save(Server.MapPath(rootPathUpload)+ @"\" + "convertToBmp.jpg", ImageFormat.Jpeg);

            //emf.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToBmp.bmp", ImageFormat.Bmp);
            ////System.Diagnostics.Process.Start("convertToBmp.bmp");            
            //emf.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToEmf.png", ImageFormat.Png);            
            //System.Diagnostics.Process.Start("convertToEmf.png");            
            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Tiff);

            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Wmf);
            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Exif);
            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Gif);
            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Icon);
            //zoomImg.Save(Server.MapPath(rootPathUpload) + @"\" + "convertToZoom.png", ImageFormat.Wmf);

            //System.Diagnostics.Process.Start("convertToZoom.png");

            /////return "";
            #endregion

        }


        //string namepng = fileName.Replace(".pdf", ".png");
        //string dataspng = Server.MapPath(rootPathUpload) + @"\" + namepng;
        //string dataDir = Server.MapPath(rootPathUpload) + @"\" + fileName;
        //try
        //{
        //    using (var document = PdfiumViewer.PdfDocument.Load(dataDir))
        //    {
        //        var image = document.Render(1, 300, 300, true);
        //        image.Save(dataspng, ImageFormat.Png);
        //        //image.Save(@"output.png", ImageFormat.Png);


        //        return "";
        //    }
        //}
        //catch (Exception e)
        //{
        //    String error = String.Format("{0}\\{0}\\{0}", e.StackTrace, e.Message, e.InnerException);
        //    return "";
        //}

        //public String ConvertPDFToJPG(string Spath)
        //{
        //    //string dataDir = "~/ uploads / javascript_the_good_parts.pdf";
        //    string dataDir = Server.MapPath(rootPathUpload);// + @"\" + fileName;
        //    ArrayList ArchivosJpgList = new ArrayList();
        //    // Open document
        //    Document pdfDocument = new Document(dataDir +@"\" + fileName);

        //    for (int pageCount = 1; pageCount <= pdfDocument.Pages.Count; pageCount++)
        //    {
        //        using (FileStream imageStream = new FileStream(dataDir + @"\" + "image" + pageCount + "_out" + ".jpg", FileMode.Create))
        //        {


        //            String aaa = dataDir + @"\" + "image" + pageCount + "_out" + ".jpg";

        //            String aaaaaa = String.Format("{0}\\image{1}_out.jpg", dataDir, pageCount);

        //            ArchivosJpgList.Add("");
        //            // Create JPEG device with specified attributes
        //            // Width, Height, Resolution, Quality
        //            // Quality [0-100], 100 is Maximum
        //            // Create Resolution object
        //            Resolution resolution = new Resolution(300);

        //            // JpegDevice jpegDevice = new JpegDevice(500, 700, resolution, 100);
        //            JpegDevice jpegDevice = new JpegDevice(resolution, 100);

        //            // Convert a particular page and save the image to stream
        //            jpegDevice.Process(pdfDocument.Pages[pageCount], imageStream);

        //            // Close stream
        //            imageStream.Close();

        //        }
        //    }


        //    return "";

        //}

        //public void ConvertJPGToPDF(string Spath)
        //{
        //    //ITEXSHARP

        //    //Document document = new Document();
        //    //using (var stream = new FileStream("test.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
        //    //{
        //    //    PdfWriter.GetInstance(document, stream);
        //    //    document.Open();
        //    //    using (var imageStream = new FileStream("test.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //    //    {
        //    //        var image = Image.GetInstance(imageStream);
        //    //        document.Add(image);
        //    //    }
        //    //    document.Close();
        //    //}

        //}
        public void ConvertPDFToTIFF(string Spath)
        {


        }

        protected internal String ConvertPdfToDoc(string filename)
        {
            try
            {
                String PDFpath = Server.MapPath(String.Format("{0}\\{1}\\{2}", rootPath, DOCXexportedPath, filename));
                PDDocument PDFdoc = null;
                PDFTextStripper textstrip = new PDFTextStripper();
                String StringDocx = String.Empty;
                String DocxFolderPath = String.Empty;
                String DocxPath = String.Empty;
                String outFilename= String.Empty;

                PDFdoc = PDDocument.load(PDFpath);
                StringDocx = textstrip.getText(PDFdoc);
                PDFdoc.close(); //cierra el pdf

                ///DocxPath = fn.CreateFolderToSaveDocs(fn.GenerateName()); ///genera la ruta para guardar el archivo.
                DocxFolderPath = fn.CreateFolderToSaveDocs(fileName); ///genera la ruta para guardar el archivo.
                if (DocxFolderPath != "")
                {
                    outFilename = filename.Replace(".pdf", ".docx");
                    if(!outFilename.Contains(".pdf"))                       
                        DocxPath = Server.MapPath(String.Format("{0}\\{1}\\{2}", rootPath, DocxFolderPath, outFilename));
                }              
                

                var wordDoc = DocX.Create(DocxPath);
                wordDoc.InsertParagraph(StringDocx);
                wordDoc.Save();
                ////Process.Start("winword.exe", DocxPath);
                String ReturnString = String.Format("{0}\\{1}", DocxFolderPath, outFilename);

                return ReturnString;
            }
            catch (Exception e)
            {
                String error = String.Format("{0}\\{1}\\{2}", e.StackTrace, e.Message, e.InnerException);
                return "";
            }   
        }

       


    }
}
