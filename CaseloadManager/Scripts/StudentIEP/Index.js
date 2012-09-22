$(document).ready(function () {

    $("#dataTable").tablesorter();
    $("#createIEP").click(function (event) {
        event.preventDefault();
        AddIEP();
    });
});