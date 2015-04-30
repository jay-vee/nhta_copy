
function CheckMessages() {
    if ($("#cphMain_spanFeedbackMessage").text().length == 0) {
        $(".alertFeedbackMessage").alert('close');
    }
    else {
        $("#cphMain_spanFeedbackMessage:contains('ERROR')").parent().parent().removeClass("alert-success").removeClass("alert-warning").addClass("alert-danger");
        $("#cphMain_spanFeedbackMessage:contains('SUCCESS')").parent().parent().removeClass("alert-danger").removeClass("alert-warning").addClass("alert-success");
        $("#cphMain_spanFeedbackMessage:contains('WARNING')").parent().parent().removeClass("alert-danger").removeClass("alert-success").addClass("alert-warning");
        $(".alertFeedbackMessage").slideDown("slow", function () { $(this).alert(); });
    }
}

function ClearMessages() {
    $("#cphMain_spanFeedbackMessage").text("");
    $(".alertFeedbackMessage").slideUp("slow", function () { $(this).alert('close'); });
}

$(document).ready(function () {
    ////omit the <button> controls data-dismiss="alert" attribute from Bootstrap 3 alerts to have alert close button 'slideUp'
    $(".alert button.close").click(function (e) {
        $(this).parent().slideUp('slow');
    });
});