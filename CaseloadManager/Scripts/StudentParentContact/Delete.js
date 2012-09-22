$(document).ready(function () {
    $('#cancelDeleteButton').click(function (event) {
        event.preventDefault();
        LoadParentContacts();
    });


    $("#deleteStudentParentContact").click(function (event) {

        event.preventDefault();
        var url = '/StudentParentContact/Delete/' + $('#StudentParentContactId').val();

        $.post(url, function (data) {

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
        });
    });

});
