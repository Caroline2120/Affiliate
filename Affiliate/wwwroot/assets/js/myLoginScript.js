$(function () {
    'use strict';

    $('#role').on('change', function () {
        $("#divbutton").css("display", "block");
        var demovalue = $(this).val();

        if (demovalue === '7') {

            $("#divCode").css("display", "block"); 
            /*$("#formDiv").css("display", "none");*/
        }
        else if (demovalue === '1') {
          
            $("#divCode").css("display", "none");
            /*$("#divRole").css("display", "none");*/
        }

    });

    //$('#register').click(function () {

    //    var urlcodes = '/Home/ValidateCode';
    //    var role = document.getElementById("role").value;
    //    var demovalue = document.getElementById("code").value;
    //    var emailvalue = document.getElementById("email").value;
    //    if (role === "Marketer") {
    //        $.post(urlcodes, { Code: demovalue, Email: emailvalue }, function (data) {
    //            alert(data);
    //            if (data === "Invalid") {
    //                let txt = "";
    //                if (confirm("Invalid code, do you want to request for new one?")) {
    //                    txt = "You pressed OK!";
    //                }
    //            }
    //        });
    //    }
    //    else {
    //        let urlIndex = '/Home/Index';
    //        window.location.href = urlIndex;
    //    }
    //});
});