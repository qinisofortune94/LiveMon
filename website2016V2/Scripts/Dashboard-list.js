/// <reference path="../../../js/libs/jquery-1.7.1.js" />
/// <reference path="../../../js/references.js" />



$(document).ready(function () {

    document.getElementById('DivDashboardNew').style.display = "none";
    RegisterNewSettingsButton();
    RegisterCloseNewButton();
    SaveSettings();

});
function RegisterNewSettingsButton() {

    $('#btnNewSettings').click(function () {

        document.getElementById('DivDashboardNew').style.display = "block";
        $('#DivDashboardNew').dialog({ title: "",width:400 });

    });
}

function RegisterCloseNewButton() {

    $('#btnCloseNew').click(function () {
        $('#DivDashboardNew').dialog('close');
        return false;
   
    });
}


