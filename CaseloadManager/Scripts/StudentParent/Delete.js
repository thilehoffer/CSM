$(document).ready(function () {
    $("#deleteStudentParent").click(function (event) {

        event.preventDefault();
        var url = '/StudentParent/Delete/' + $('#StudentParentId').val();
        $.post(url, function (data) {

            var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
            jsonAjaxResult.init(data);

            if (jsonAjaxResult.noAccess) {
                window.location = '@Url.Action("NoAccess")';
                return;
            }
            if (jsonAjaxResult.notFound) {
                window.location = '@Url.Action("NotFound")';
                return;
            }

            if (jsonAjaxResult.success) {
                LoadParents();
            }
        });
    });

    $("#CancelDeleteStudentParent").click(function (event) {
        event.preventDefault();
        LoadParents();
    });


});