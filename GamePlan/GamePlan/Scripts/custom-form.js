$(document).ready(function () {
    $("#EmailNotifyformId").submit(function (event) {
        var dataString;
        event.preventDefault();
        event.stopImmediatePropagation();
        var action = $("#EmailNotifyformId").attr("action");

        // Setting.  
        dataString = new FormData($("#EmailNotifyformId").get(0));
        contentType = false;
        processData = false;

        $.ajax({
            type: "POST",
            url: action,
            data: dataString,
            dataType: "json",
            contentType: contentType,
            processData: processData,
            success: function (result) {
                // Result.  
                onEmailNotifySuccess(result);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //do your own thing  
                alert("fail");
            }
        });

    }); //end .submit()  
});

var onEmailNotifySuccess = function (result) {
    if (result.EnableError) {
        // Setting.  
        alert(result.ErrorMsg);
    }
    else if (result.EnableSuccess) {
        // Setting.  
        alert(result.SuccessMsg);

        // Resetting form.  
        $('#EmailNotifyformId').get(0).reset();
    }
}