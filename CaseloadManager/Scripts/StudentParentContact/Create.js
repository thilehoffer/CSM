
function CloseAddParentWindow() {
    $("#addParentWindow").data("kendoWindow").close();
}

function onClose(e) {
    $("#addParentWindow").data("kendoWindow").destroy();
}

function AddParent(id) {
    $("#addParentWindow").html('Loading....');

    var window = $("#addParentWindow").kendoWindow({
        actions: ["Close"],
        draggable: false,
        height: "500px",
        modal: true,
        resizable: false,
        title: "Notes",
        width: "500px",
        close: onClose
    }).data("kendoWindow");
    window.center();
    window.open();

    var url = '/StudentParent/CreateGet/' + id;
    var postData = { isWindow: true };
    $.post(url, postData, function (data) {
        $("#addParentWindow").data("kendoWindow").content(data);
    });

}


$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#DateOfContact").kendoDatePicker();
    $("#TimeOfContact").kendoTimePicker();
    $("#StudentParentId").kendoDropDownList();

    $('#cancelButton').click(function (event) {
        event.preventDefault();
        LoadParentContacts();
    });

    $("#saveContactRecord").click(function (event) {
        event.preventDefault();

        var postData = $('#CreateStudentParentContactForm').serialize();
        var url = '/StudentParentContact/Create';

        $.post(url, postData,
            function (data) {
                var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
                jsonAjaxResult.init(data);

                if (jsonAjaxResult.noAccess) {
                    window.location = '/NoAccess';
                    return;
                }
                if (jsonAjaxResult.notFound) {
                    window.location = '/NotFound';
                    return;
                }
                if (jsonAjaxResult.success) {

                    LoadParentContacts();
                }
                else {
                    $('#postSaveDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
    });

});