var tabs = [];
var studentId = $('#StudentId').val();

$(document).ready(function () {
    $("#studentTabStrip").kendoTabStrip(
    {
        animation: { open: { effects: "fadeIn" } },
        select: onSelect
    });

    //Simply to create a collection with better names
    tabs = {
        'tabDemographics': $('#studentTabStrip-1'),
        'tabIEPs': $('#studentTabStrip-2'),
        'tabEvaluations': $('#studentTabStrip-3'),
        'tabParent': $('#studentTabStrip-4'),
        'tabParentContacts': $('#studentTabStrip-5')
    };
    LoadDemographics();
});

function onSelect(e) {

    var tabId = $(e.item).attr('id');
    switch (tabId) {
        case 'demographicsTab':
            LoadDemographics();
            break;
        case 'iEPsTab':
            LoadIeps();
            break;
        case 'evaluationsTab':
            LoadEvaluations();
            break;
        case 'parentTab':
            LoadParents();
            break;
        case 'parentContactTab':
            LoadParentContacts();
            break;
        default:
            alert('selection unknown');
            break;
    }
}

function ClearAll() {
    $.each(tabs, function (key, tab) {
        tab.html('Loading....');
    });
}

function LoadDemographics() {
    ClearAll();
    $.post('/Student/BasicInfoGet/' + studentId, function (data) {
        tabs['tabDemographics'].html(data);
        basicInfo.postLoad();
    });
}
function LoadIeps() {
    ClearAll();
    $.post('/StudentIEP/Get/' + studentId, function (data) {
        tabs['tabIEPs'].html(data);
    });
}
function AddIEP() {
    ClearAll();
    $.post('/StudentIEP/CreateGet/' + studentId, function (data) {
        tabs['tabIEPs'].html(data);
    });
}
function EditIep(id) {
    ClearAll();
    $.post('/StudentIEP/EditGet/' + id, function (data) {
        tabs['tabIEPs'].html(data);
    });
}
function GetDeleteIep(id) {
    ClearAll();
    $.post('/StudentIEP/DeleteGet/' + id, function (data) {
        tabs['tabIEPs'].html(data);
    });
}
function GetIepAttachments(id) {
    ClearAll();
    $.post('/StudentIEP/AttachmentsGet/' + id, function (data) {
        tabs['tabIEPs'].html(data);
    });
}
function EditEvaluation(id) {
    ClearAll();
    $.post('/StudentEvaluation/EditGet/' + id, function (data) {
        tabs['tabEvaluations'].html(data);
    });
}
function LoadEvaluations() {
    ClearAll();
    $.post('/StudentEvaluation/Get/' + studentId, function (data) {
        tabs['tabEvaluations'].html(data);
    });
}
function AddEvaluation() {
    ClearAll();
    $.post('/StudentEvaluation/CreateGet/' + studentId, function (data) {
        tabs['tabEvaluations'].html(data);
    });
}
function GetDeleteEvaluation(id) {
    ClearAll();
    $.post('/StudentEvaluation/DeleteGet/' + id, function (data) {
        tabs['tabEvaluations'].html(data);
    });
}
function GetEvaluationAttachments(id) {
    ClearAll();
    $.post('/StudentEvaluation/AttachmentsGet/' + id, function (data) {
        tabs['tabEvaluations'].html(data);
    });
}
function AddParent() {
    ClearAll();
    $.post('/StudentParent/CreateGet/' + studentId, function (data) {
        tabs['tabParent'].html(data);
    });
}
function LoadParents() {
    ClearAll();
    $.post('/StudentParent/Get/' + studentId, function (data) {
        tabs['tabParent'].html(data);
    });
}
function GetDeleteParent(id) {
    ClearAll();
    $.post('/StudentParent/DeleteGet/' + id, function (data) {
        tabs['tabParent'].html(data);
    });
}
function EditParent(id) {
    ClearAll();
    $.post('/StudentParent/EditGet/' + id, function (data) {
        tabs['tabParent'].html(data);
    });
}
function LoadParentContacts() {
    ClearAll();
    $.post('/StudentParentContact/Get/' + studentId, function (data) {
        tabs['tabParentContacts'].html(data);
    });
}
function AddParentContact() {
    ClearAll();
    $.post('/StudentParentContact/CreateGet/' + studentId, function (data) {
        tabs['tabParentContacts'].html(data);
    });
}
function GetStudentParentContactAttachments(id) {
    ClearAll();
    $.post('/StudentParentContact/AttachmentsGet/' + id, function (data) {
        tabs['tabParentContacts'].html(data);
    });
}
function EditStudentParentContact(id) {
    ClearAll();
    $.post('/StudentParentContact/EditGet/' + id, function (data) {
        tabs['tabParentContacts'].html(data);
    });
}
function GetDeleteStudentParentContact(id) {
    ClearAll();
    $.post('/StudentParentContact/DeleteGet/' + id, function (data) {
        tabs['tabParentContacts'].html(data);
    });
}

function SelectParentsTab() {
    var tabstrip = $("#studentTabStrip").kendoTabStrip().data("kendoTabStrip");
    tabstrip.select(3);
    LoadParents();
}
