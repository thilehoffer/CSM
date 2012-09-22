$(document).ready(function () {
    $("#dataTable").tablesorter();

    $("#createParent").click(function (event) {
        event.preventDefault();
        AddParent();
    });
});