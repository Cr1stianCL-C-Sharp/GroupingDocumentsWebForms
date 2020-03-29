<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupDocuments.aspx.cs" Inherits="GroupingDocumentsWeb.GroupDocuments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- CARGA MODULOS JAVASCRIPT--%>
    <script src="js/jquery-3.1.1.min.js"></script>
    <script src="js/bootstrap.min.js"></script>      
    <script src="js/npm.js"></script>
    

    <%-- CARGA ESTILOS CSS--%>
    <link href="css/bootstrap.min.css" rel="stylesheet" />  
    <link href="css/estilos.css" rel="stylesheet" />    
    <link href="css/bootstrap-theme.min.css" rel="stylesheet" />


    <script>


    </script>
</head>
<body>
<div class="container">
    <div class="row">
         
        <div class="col-lg-8 col-md-8 col-sm-8">
            <iframe class="col-lg-12 col-md-12 col-sm-12" src="./BlankPDF.pdf" id="IframeDocument" runat="server"> </iframe>
            <form id="form1" runat="server" class="form-inline"> 
	            
			      <div class="form-group">
			        <label class="control-label" id="labelupload">Cargue el archivo a modificar:</label>
                    <asp:FileUpload ID="FileUpload1" runat="server"/>				   
			      </div>
			      <div class="form-group">
			         <asp:Button runat="server" CssClass="btn btn-primary" Text="Cargar" OnClick="UploadMultipleFiles"></asp:Button>
			      </div>
			      <div class="form-group">
			          <asp:Button runat="server" CssClass="btn btn-primary" Text="Cerrar" OnClick="unloadPdf"></asp:Button>
			      </div>
                  <div class="form-group">
                      <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar"></asp:Button>
                      <asp:Button runat="server" CssClass="btn btn-danger" Text="Remover Hoja"></asp:Button>
                  </div>

            </form>   <%--end runat server--%>
            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
    


		</div> <%--end col-sm-8 primera columna bootstrap--%> 

		<div class="col-lg-4 col-md-4 col-sm-4">
		 
            <div class="btn-group" role="group" aria-label="..." id="btnOpciones">
                          <a href="SelectDocumentPages.aspx" class="btn btn-info" role="button">Ir a editar PDF</a>
                         <%-- <button type="button" class="btn btn-md btn-info">Ir a editar PDF</button>        --%>                 
                          <div class="btn-group">
                          <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Guardar Archivo Como <span class="caret"></span>
                          </button>
                          <ul class="dropdown-menu">
                           <%-- <li><a href="#">Guardar como PDF</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Tiff</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Word</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Multiples Imagenes</a></li>  --%>                               
                         

                           <%-- <li><input type="button" ID="btnRaisePostBack" runat="server" onclick="raisePostBack();"> hola</input></li> --%>  
                            <li><a href="#">Guardar como PDF</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Tiff</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Word</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Guardar como Multiples Imagenes</a></li>                       
                          </ul>
                        </div>
            </div>

            

                
            
           
                    <div class="thumbnail" id="ComentariosAyuda">
                    <img src="/images/wordtopdf.png" style="width:200px" alt="thumbnailpdfword"/>
                        <div class="caption">
                            <h3>Lo que puedes hacer:</h3>
                            <p>Con esta aplicacion web, puedes cargar congenido en PDF y convertirlo a Doc, ademas puedes cargar archivos en word
                                y convertirlos a PDF.Ademas puedes editar dichos archivos quitando paginas y guardandolos en el formato que quieras, mas adelante 
                                contaremos con mas opciones.
                            </p>
                            <%--<p><a href="#" class="btn btn-primary" role="button">Button</a> <a href="#" class="btn btn-default" role="button">Button</a></p>--%>
                        </div>
                    </div>
           
            
      
		</div>   <%--end col-lg-4 col-md-4 col-sm-4 segunda columna bootstrap--%> 
	</div>  <%--end row --%> 	       
</div> <%--end container --%> 
    
 
</body>
</html>
