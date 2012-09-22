
 
var jStudentId = $('#StudentId');
var jStudentIepId = $('#StudentIEPId_Value');
var jDataTable = $('#dataTable');

function attachOnUpload(e) {
    
    e.data = { studentId: jStudentId.val(), studentIepId: jStudentIepId.val() };
}
function attachOnSuccess(e) {
    GetIepAttachments(jStudentIepId.val());
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
                  GetIepAttachments(jStudentIepId.val());
              }
              else {
                  alert('Delete failed');
              }
          });
}
$(document).ready(function () {
    if ($('#IepDocumentCount').val() === '0') {
        jDataTable.hide();
    } else {
        jDataTable.tablesorter();
    }

    $("#FinishedButton").click(function (event) {
        event.preventDefault();
        LoadIeps();
    });

    var saveURL = '/StudentIEP/AddFile';
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