
function attachOnUpload(e) {

    e.data = { studentId: $('#StudentId').val(), studentEvaluationId: $('#StudentEvaluationId').val() };
}
function attachOnSuccess(e) {
    GetEvaluationAttachments($('#StudentEvaluationId').val());
}

function deleteDoc(id) {
    var proceed = confirm('Are you sure you want to permantly delete this document?');
    if (!proceed) {
        return;
    }

    var url = '/Document/Delete/' + id;
    $.post(url,
          function (result) {
              if (result.success) {
                  GetEvaluationAttachments($('#StudentEvaluationId').val());
              }
              else {
                  alert('Delete failed');
              }
          });

}


$(document).ready(function () {


    if ($('#EvaluationDocumentsCount').val() === '0') {
        $('#dataTable').hide();
    } else {
        $("#dataTable").tablesorter();
    }

    $("#FinishedButton").click(function (event) {
        event.preventDefault();
        LoadEvaluations();
    });

    var saveURL = '/StudentEvaluation/AddFile';
    $("#attachments").kendoUpload({
        async: {
            saveUrl: saveURL,
            autoUpload: true
        },
        upload: attachOnUpload,
        success: attachOnSuccess,
        multiple: false,
        showFileList: false
    });
});