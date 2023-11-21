$(function () {
    'use strict';

    $('#role').on('change', function () {
        
        var demovalue = $(this).val();
        if (demovalue === 'Marketer') {

            $("#divCode").css("display", "block");
            $("#formDiv").css("display", "none");
        }
        else if (demovalue === 'Freelance'){
            $("#formDiv").css("display", "block");
            $("#divCode").css("display", "none");
            $("#divRole").css("display", "none");
        }
        
    });

    $('#gender').on('change', function () {

        //Load programs from DB
        //---------------------
        let divS = $("#divPrograms");

        var urlPrograms = '/Home/GetPrograms';

        $.post(urlPrograms, function (data) {

            $("#SelectedProgramIds").empty();
            $("#divPrograms").empty();

            $.each(data, function (i, response) {

                let myHtml = '<div style="display: inline-flex;align-items: center;margin-right: 0.75rem;"><label class="container">' + response.text + '<input type = "checkbox" value="' + response.value + '" class="chkPrograms"><span class="checkmark"></span></label></div>';
                divS.append(myHtml);

                //let myHtml = '';
                //for (let t = 0; t < response.length; t++) {
                //    let pcat = response[t].programCategory;
                //    let pname = '';
                //    let pid = '';
                //    myHtml = '';
                //    if (response[t].programs.length > 0) {

                //        myHtml = '';
                //        for (let p = 0; p < response[t].programs.length; p++) {

                //            pname = response[t].programs[p].name;
                //            pid = response[t].programs[p].id;

                //            myHtml += '<div style="display: inline-flex;align-items: center;margin-right: 0.75rem;"><label class="container">' + pname + '<input type = "checkbox" value="' + pid + '" class="chkPrograms"><span class="checkmark"></span></label></div>';
                //        }

                //        //let categoryDiv = '<div style="margin-bottom: 1rem !important;"><label style="margin-bottom: 0.5rem;">' + pcat + '</label><div>' + myHtml + '</></div>';

                //        //divS.append(categoryDiv);
                //    }                    
                //    let categoryDiv = '<div style="margin-bottom: 1rem !important;"><label style="margin-bottom: 0.5rem;">' + pcat + '</label><div>' + myHtml + '</></div>';

                //    divS.append(categoryDiv);
                //}
               
            });

            $("#acctDiv").css("display", "block");
            $("#programDiv").css("display", "block"); 

            //Load nigeria states from DB
            //---------------------------
            let divSt = $("#divStates");

            var urlStates = '/Home/GetStates';

            $.post(urlStates, { CountryId: 163 }, function (data) {

                $("#SelectedstateIds").empty();
                $("#divStates").empty();

                $.each(data, function (i, response) {
                    let myHtml = '<div style="display: inline-flex;align-items: center;margin-right: 0.75rem;"><label class="container">' + response.text + '<input type = "checkbox" value="' + response.value + '" class="chkStates"><span class="checkmark"></span></label></div>';
                    divSt.append(myHtml);
                });
            });
            $("#statesDiv").css("display", "block");
        });
    });


    $('#country').on('change', function () {

        //Load nigeria states from DB
        //---------------------------
        var demovalue = $(this).val();

        let divSt = $("#divStates");

        var urlStates = '/Home/GetStates';

        $.post(urlStates, { CountryId: demovalue }, function (data) {

            $("#SelectedstateIds").empty();
            $("#divStates").empty();

            $.each(data, function (i, response) {
                let myHtml = '<div style="display: inline-flex;align-items: center;margin-right: 0.75rem;"><label class="container">' + response.text + '<input type = "checkbox" value="' + response.value + '" class="chkStates"><span class="checkmark"></span></label></div>';
                divSt.append(myHtml);
            });
        });
        $("#statesDiv").css("display", "block");

    });

    function selectedPrograms() {
        var items = '';
        var checkedboxes = document.querySelectorAll('input.chkPrograms:checked');
        for (let i = 0; i < checkedboxes.length; i++) {

            var res = checkedboxes[i].value;

            items += "<option selected value = '" + res + "'>" + res + "</option>";
        }
        $("#SelectedProgramIds").html(items);
    }

    function selectedStates() {
        var items = '';
        var checkedboxes = document.querySelectorAll('input.chkStates:checked');
        for (let i = 0; i < checkedboxes.length; i++) {
            
            var res = checkedboxes[i].value;
            items += "<option selected value = '" + res + "'>" + res + "</option>";
        }
        $("#SelectedstateIds").html(items);
    }

    $("#btnSubmit").click(function () {
           
        selectedPrograms();
        selectedStates();
    });

      
 });