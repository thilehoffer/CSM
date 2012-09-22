

function ShowNotes(id) {

    $("#notesWindow").html('Loading....');

    var onClose = function () {
        $("#notesWindow").data("kendoWindow").destroy();
    };

    var window = $("#notesWindow").kendoWindow({
        actions: ["Close"],
        draggable: false,
        height: "300px",
        modal: true,
        resizable: false,
        title: "Notes",
        width: "500px",
        close: onClose
    }).data("kendoWindow");
    window.center();
    window.open();

    var url = '/StudentParentContact/GetNotes/' + id;
    $.post(url, function (data) {
        $("#notesWindow").data("kendoWindow").content(data);
    });
}

$(document).ready(function () {

    $("#dataTable").tablesorter();

    $("#createParentContact").click(function (event) {
        event.preventDefault();
        AddParentContact();
    });
});