/*  Created By:     Joe Vago
    Created Date:   May 21, 2014 
    Description:    Requires Bootstrap 3.x - classes in separate CSS to allow .NET GridViews that are styled with bootstrap classes to have a hover and click CSS applied
    */

// ============== Start $(document).ready() ==============
$(document).ready(function () {
    $("tr").click(function (e) {
        if ($(this).index() > 0) {
            $("tr").removeClass("GridSelect");
            $(this).addClass("GridSelect");
        }
    });

    $("tr").hover(
        function () { $(this).addClass("GridHover"); }
        ,
        function () { $(this).removeClass("GridHover"); }
    );

});
// ============== End $(document).ready() ==============
