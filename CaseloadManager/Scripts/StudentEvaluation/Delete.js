
$(document).ready(function () {
    
    $("#deleteEvaluation").click(function (event) {
        
        event.preventDefault();
        var url = '/StudentEvaluation/Delete/' + $('#StudentEvaluationId').val();
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
                LoadEvaluations();
            }
        });
    });

    $("#cancelEvaluation").click(function (event) {
        event.preventDefault();
        LoadEvaluations();
    });


});