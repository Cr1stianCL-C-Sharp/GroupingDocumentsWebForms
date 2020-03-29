<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectDocumentPages.aspx.cs" Inherits="GroupingDocumentsWeb.SelectDocumentPages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <title>Edita documento.</title>   

     <%-- Carousel JAVASCRIPT  --%> 
    <script src="js/jquery-3.1.1.min.js"></script>  
    <script src="js/bootstrap.min.js"></script> 
    <script src="js/carousel-preview.js "></script> 

     <%-- CARGA DE CSS  --%> 
    <link href="css/carousel-preview.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/bootstrap-theme.min.css" rel="stylesheet" />
<script type="text/javascript"> 
    $('div#image_container img').click(function(){
    // set the img-source as value of image_from_list
    $('input#image_from_list').val( $(this).attr("src") );
});
</script>

</head>
<body>

<div class="container" id="main_container_boostrap">
    <div id="main_area">        
        <div class="row">  
            <div class="col-lg-6 col-md-6 col-sm-6" id="slider-thumbs">
                <%-- Bottom switcher of slider --%> 
                <ul class="hide-bullets">
                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-0">
                            <img src="http://placehold.it/150x150&text=zero"/>
                        </a>
                    </li>

                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-1"><img src="http://placehold.it/150x150&text=1"/></a>
                    </li>

                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-2"><img src="http://placehold.it/150x150&text=2"/></a>
                    </li>

                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-3"><img src="http://placehold.it/150x150&text=3"/></a>
                    </li>

                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-4"><img src="http://placehold.it/150x150&text=4"/></a>
                    </li>

                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-5"><img src="http://placehold.it/150x150&text=5"/></a>
                    </li>
                    <li class="col-sm-3">
                        <a class="thumbnail" id="carousel-selector-6"><img src="http://placehold.it/150x150&text=6"/></a>
                    </li>
                    
                </ul>
            </div> <%-- END slider-thumbs --%> 

            <div class="col-lg-6 col-md-6 col-sm-6">
                <div class="col-xs-12" id="slider">
                    <%-- Top part of the slider --%> 
                    <div class="row">
                        <div class="col-sm-12" id="carousel-bounding-box">
                            <div class="carousel slide" id="myCarousel">
                                <%-- Carousel items  --%> 
                                <div class="carousel-inner">

                                    <div class="active item" data-slide-number="0">
                                        <img src="http://placehold.it/470x480&text=zero"/>
                                    </div>

                                    <div class="item" data-slide-number="1">
                                        <img src="http://placehold.it/470x480&text=1"/>
                                    </div>

                                    <div class="item" data-slide-number="2">
                                        <img src="http://placehold.it/470x480&text=2"/>
                                    </div>

                                    <div class="item" data-slide-number="3">
                                        <img src="http://placehold.it/470x480&text=3"/>
                                    </div>

                                    <div class="item" data-slide-number="4">
                                        <img src="http://placehold.it/470x480&text=4"/>
                                    </div>

                                    <div class="item" data-slide-number="5">
                                        <img src="http://placehold.it/470x480&text=5"/>
                                    </div>
                                    
                                    <div class="item" data-slide-number="6">
                                        <img src="http://placehold.it/470x480&text=6"/>
                                    </div>                                    
                             
                                 </div>     <%-- end carousel-inner --%>

                                <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                                    <span class="glyphicon glyphicon-chevron-left"></span>
                                </a>
                                <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                                    <span class="glyphicon glyphicon-chevron-right"></span>
                                </a>
                            </div> <%-- END  myCarousel  --%> 
                        </div>   <%-- carousel-bounding-box  --%> 
                    </div> <%-- END ROW-SLIDER --%>
                </div>  <%-- END Slider --%>
            </div>  <%-- col-lg-6 col-md-6 col-sm-6 --%>          
        </div> <%-- end row --%>
    </div> <%-- end main_area --%>
</div> <%-- end container --%>

</body>

</html>
