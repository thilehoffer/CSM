
function attachOnUpload(e) {

    e.data = { studentId: $('#StudentId').val(), studentParentContactId: $('#StudentParentContactId').val() };
}

function attachOnSuccess(e) {
    GetStudentParentContactAttachments($('#StudentParentContactId').val());
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
                  GetStudentParentContactAttachments($('#StudentParentContactId').val());
              }
              else {
                  alert('Delete failed');
              }
          });
}

$(document).ready(function () {
    if ($('#StudentParentContactAttachmentsCount').val === '0') {
        $('#dataTable').hide();
    } else {
        $("#dataTable").tablesorter();
    }

    $("#FinishedButton").click(function (event) {
        event.preventDefault();
        LoadParentContacts();
    });

    var saveURL = '/StudentParentContact/AddFile';
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