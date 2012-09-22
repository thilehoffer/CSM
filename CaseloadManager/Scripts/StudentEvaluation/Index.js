$(document).ready(function () {
    $("#dataTable").tablesorter();

    $("#createEvaluation").click(function (event) {
        event.preventDefault();
        AddEvaluation();
    });
});