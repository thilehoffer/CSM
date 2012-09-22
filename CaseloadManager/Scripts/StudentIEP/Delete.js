$(document).ready(function () {
    $("#deleteIEP").click(function (event) {

        event.preventDefault();
        var url = '/StudentIEP/Delete/' + $('#StudentIEPId').val();

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
                LoadIeps();
            }
        });
    });

    $("#CancelIEP").click(function (event) {
        event.preventDefault();
        LoadIeps();
    });


});