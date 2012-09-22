$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#ScheduledDate").kendoDatePicker();
    $("#DateCompleted").kendoDatePicker();

    $("#saveEvaluation").click(function (event) {

        event.preventDefault();

        var postData = $('#CreateEvaluationForm').serialize();
        var url = '/StudentEvaluation/Create';

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
                    LoadEvaluations();
                }
                else {
                    $('#postCreateEvaluationDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
    });

});